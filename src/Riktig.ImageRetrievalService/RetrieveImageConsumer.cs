namespace Riktig.ImageRetrievalService
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Contracts.Services.Commands;
    using Contracts.Services.Events;
    using Magnum.Extensions;
    using MassTransit;
    using Topshelf.Logging;


    public class RetrieveImageConsumer :
        Consumes<RetrieveImage>.Context
    {
        static readonly LogWriter _log = HostLogger.Get<RetrieveImageConsumer>();
        readonly RetrieveImageSettings _settings;

        public RetrieveImageConsumer(RetrieveImageSettings settings)
        {
            _settings = settings;
        }

        public void Consume(IConsumeContext<RetrieveImage> context)
        {
            Uri sourceAddress = context.Message.SourceAddress;

            _log.DebugFormat("Retrieve Image: {0}", sourceAddress);

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(sourceAddress).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string localFileName =
                            Path.GetFullPath(Path.Combine(_settings.LocalImageCache, NewId.NextGuid().ToString()));
                        Uri contentLocation = response.Content.Headers.ContentLocation ?? sourceAddress;
                        if (response.Content.Headers.ContentDisposition != null && 
                            Path.HasExtension(response.Content.Headers.ContentDisposition.FileName))
                            localFileName += Path.GetExtension(response.Content.Headers.ContentDisposition.FileName);
                        else if (Path.HasExtension(contentLocation.AbsoluteUri))
                            localFileName += Path.GetExtension(contentLocation.AbsoluteUri);

                        _log.DebugFormat("Success, copying to local file: {0}", localFileName);

                        using (FileStream stream = File.Create(localFileName))
                        {
                            bool completed = response.Content.CopyToAsync(stream).Wait(30.Seconds());
                            if (completed)
                            {
                                stream.Close();

                                var fileInfo = new FileInfo(localFileName);
                                var localAddress = new Uri(fileInfo.FullName);

                                _log.DebugFormat("Completed, length = {0}", fileInfo.Length);

                                context.Bus.Publish(new ImageRetrievedEvent(context.Message.CommandId,
                                    sourceAddress, localAddress,
                                    response.Content.Headers.ContentType.ToString(), (int)fileInfo.Length));
                            }
                        }
                    }
                    else
                    {
                        string message = string.Format("Server returned a response status code: {0} ({1})",
                            (int)response.StatusCode, response.StatusCode);

                        _log.ErrorFormat("Failed to retrieve image: {0}", message);

                        context.Bus.Publish(new ImageNotFoundEvent(context.Message.CommandId,
                            sourceAddress, message));
                    }
                }
            }
            catch (AggregateException exception)
            {
                _log.Error("Exception from HttpClient", exception.InnerException);

                context.Bus.Publish(new ImageRetrievalFailedEvent(context.Message.CommandId,
                    sourceAddress, exception.InnerException.Message));
            }
            catch (Exception exception)
            {
                _log.Error("Exception from HttpClient", exception);

                context.Bus.Publish(new ImageRetrievalFailedEvent(context.Message.CommandId,
                    sourceAddress, exception.Message));
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