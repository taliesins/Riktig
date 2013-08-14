namespace Riktig.Web
{
    using System;
    using MassTransit;


    public static class ServiceBusConfig
    {
        public static readonly Uri WebAddress = new Uri("rabbitmq://localhost/riktig-web");
        public static readonly Uri ImageServiceAddress = new Uri("rabbitmq://localhost/riktig-coordinationservice");

        public static void RegisterServiceBus()
        {
            Bus.Initialize(x =>
            {
                x.ReceiveFrom(WebAddress);
                x.UseRabbitMq(r =>
                {
                    r.ConfigureHost(WebAddress, h =>
                    {
                        // set username/password if required
                        h.SetRequestedHeartbeat(1);
                    });
                });
                x.ReceiveFrom(WebAddress);
            });
        }

        public static void StopServiceBus()
        {
            Bus.Shutdown();
        }
    }
}