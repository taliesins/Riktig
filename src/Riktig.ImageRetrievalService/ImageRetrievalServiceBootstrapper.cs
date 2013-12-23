namespace Riktig.ImageRetrievalService
{
    using Autofac;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Services;
    using Topshelf.Runtime;


    public class ImageRetrievalServiceBootstrapper :
        TopshelfServiceBootstrapper<ImageRetrievalServiceBootstrapper>
    {
        public ImageRetrievalServiceBootstrapper(HostSettings hostSettings)
            : base(hostSettings)
        {
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceConfigurationProviderModule>();
            builder.RegisterModule<RabbitMqConfigurationModule>();
            builder.RegisterModule<HostServiceBusModule>();

            builder.RegisterType<ImageRetrievalConsumerServiceBootstrapper>()
                   .As<IServiceBootstrapper>();
        }
    }
}