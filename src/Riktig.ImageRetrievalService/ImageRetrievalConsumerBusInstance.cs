namespace Riktig.ImageRetrievalService
{
    using MassTransit;
    using RapidTransit.Core.Configuration;
    using RapidTransit.Core.Services;


    public class ImageRetrievalConsumerBusInstance :
        ServiceBusInstance
    {
        public ImageRetrievalConsumerBusInstance(IConfigurationProvider configuration,
            IConsumerFactory<RetrieveImageConsumer> consumerFactory)
            : base(configuration, "ImageRetrievalServiceQueueName", "ImageRetrievalServiceConsumerLimit", 1)
        {
            this.Consumer(consumerFactory);
        }
    }
}