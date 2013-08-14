namespace Riktig.ImageRetrievalService
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Contracts.Services.Commands;
    using Contracts.Services.Events;
    using Magnum.Extensions;
    using MassTransit;


    public class RetrieveImageConsumer :
        Consumes<RetrieveImage>.Context
    {
        public void Consume(IConsumeContext<RetrieveImage> context)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(context.SourceAddress).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string localFileName = Path.GetFullPath(NewId.NextGuid().ToString());

                        using (FileStream stream = File.Create(localFileName))
                        {
                            bool completed = response.Content.CopyToAsync(stream).Wait(30.Seconds());
                            if (completed)
                            {
                                stream.Close();

                                var fileInfo = new FileInfo(localFileName);


                                var localAddress = new Uri(fileInfo.FullName);

                                context.Bus.Publish(new ImageRetrievedEvent(context.Message.CommandId,
                                    context.Message.SourceAddress, localAddress,
                                    response.Content.Headers.ContentType.ToString(), (int)fileInfo.Length));
                            }
                        }
                    }
                    else
                    {
                        string message = string.Format("Server returned a response status code: {0} ({1})",
                            (int)response.StatusCode, response.StatusCode);

                        context.Bus.Publish(new ImageNotFoundEvent(context.Message.CommandId,
                            context.Message.SourceAddress, message));
                    }
                }
            }
            catch (AggregateException exception)
            {
                context.Bus.Publish(new ImageRetrievalFailedEvent(context.Message.CommandId,
                    context.Message.SourceAddress, exception.InnerException.Message));
            }
            catch (Exception exception)
            {
                context.Bus.Publish(new ImageRetrievalFailedEvent(context.Message.CommandId,
                    context.Message.SourceAddress, exception.Message));
            }
        }


        class ImageNotFoundEvent :
            ImageNotFound
        {
            public ImageNotFoundEvent(Guid originatingCommandId, Uri sourceAddress, string reason)
            {
                EventId = NewId.NextGuid();
                Timestamp = DateTime.UtcNow;

                OriginatingCommandId = originatingCommandId;
                Reason = reason;
                SourceAddress = sourceAddress;
            }

            public Guid EventId { get; private set; }
            public DateTime Timestamp { get; private set; }
            public Guid OriginatingCommandId { get; private set; }
            public Uri SourceAddress { get; private set; }
            public string Reason { get; private set; }
        }

        class ImageRetrievalFailedEvent :
            ImageRetrievalFailed
        {
            public ImageRetrievalFailedEvent(Guid originatingCommandId, Uri sourceAddress, string reason)
            {
                EventId = NewId.NextGuid();
                Timestamp = DateTime.UtcNow;

                OriginatingCommandId = originatingCommandId;
                Reason = reason;
                SourceAddress = sourceAddress;
            }

            public Guid EventId { get; private set; }
            public DateTime Timestamp { get; private set; }
            public Guid OriginatingCommandId { get; private set; }
            public Uri SourceAddress { get; private set; }
            public string Reason { get; private set; }
        }


        class ImageRetrievedEvent :
            ImageRetrieved
        {
            public ImageRetrievedEvent(Guid originatingCommandId, Uri sourceAddress, Uri localAddress,
                string contentType, int contentLength)
            {
                EventId = NewId.NextGuid();
                Timestamp = DateTime.UtcNow;

                ContentLength = contentLength;
                ContentType = contentType;
                LocalAddress = localAddress;
                OriginatingCommandId = originatingCommandId;
                SourceAddress = sourceAddress;
            }

            public Guid EventId { get; private set; }
            public DateTime Timestamp { get; private set; }

            public Guid OriginatingCommandId { get; private set; }
            public Uri SourceAddress { get; private set; }
            public Uri LocalAddress { get; private set; }
            public string ContentType { get; private set; }
            public int ContentLength { get; private set; }
        }
    }
}