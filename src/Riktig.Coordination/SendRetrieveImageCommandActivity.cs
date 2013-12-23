namespace Riktig.Coordination
{
    using System;
    using Automatonymous;
    using Contracts.Services.Commands;
    using MassTransit;
    using Taskell;


    public class SendRetrieveImageCommandActivity :
        Activity<ImageRetrievalState>
    {
        readonly SendRetrieveImageCommandSettings _settings;

        public SendRetrieveImageCommandActivity(SendRetrieveImageCommandSettings settings)
        {
            _settings = settings;
        }

        public void Accept(StateMachineInspector inspector)
        {
            inspector.Inspect(this, x => { });
        }

        public void Execute(Composer composer, ImageRetrievalState instance)
        {
            SendCommand(composer, instance);
        }

        public void Execute<T>(Composer composer, ImageRetrievalState instance, T value)
        {
            SendCommand(composer, instance);
        }

        void SendCommand(Composer composer, ImageRetrievalState instance)
        {
            composer.Execute(() =>
                {
                    Uri faultAddress = instance.Bus.Endpoint.Address.Uri;

                    IEndpoint endpoint = instance.Bus.GetEndpoint(_settings.ImageRetrievalServiceAddress);

                    endpoint.Send(new RetrieveImageCommand(instance.SourceAddress),
                        x => x.SetFaultAddress(faultAddress));
                });
        }


        class RetrieveImageCommand :
            RetrieveImage
        {
            public RetrieveImageCommand(Uri sourceAddress)
            {
                if (sourceAddress == null)
                    throw new ArgumentNullException("sourceAddress");

                CommandId = NewId.NextGuid();
                Timestamp = DateTime.UtcNow;

                SourceAddress = sourceAddress;
            }

            public Guid CommandId { get; private set; }
            public DateTime Timestamp { get; private set; }
            public Uri SourceAddress { get; private set; }
        }
    }
}