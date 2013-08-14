namespace Riktig.ImageRetrievalService
{
    using System;
    using MassTransit;
    using Topshelf;


    public class ImageRetrievalService :
        ServiceControl
    {
        readonly Uri _address;
        IServiceBus _bus;

        public ImageRetrievalService(Uri address)
        {
            _address = address;
        }

        public bool Start(HostControl hostControl)
        {
            _bus = ServiceBusFactory.New(x =>
            {
                x.ReceiveFrom(_address);
                x.UseRabbitMq(r =>
                {
                    r.ConfigureHost(_address, h =>
                    {
                        // set username/password if required
                        h.SetRequestedHeartbeat(1);
                    });
                });

                x.Subscribe(s =>
                    {
                        s.Consumer(() => new RetrieveImageConsumer());
                    });
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}