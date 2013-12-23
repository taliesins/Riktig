namespace Riktig.ImageRetrievalService
{
    using Autofac;
    using RapidTransit.Core.Services;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Services;


    public class ImageRetrievalConsumerServiceBootstrapper :
        ServiceBusHostServiceBootstrapper
    {
        public ImageRetrievalConsumerServiceBootstrapper(ILifetimeScope lifetimeScope)
            : base(lifetimeScope, typeof(ImageRetrievalConsumerServiceBootstrapper))
        {
        }

        protected override void ConfigureLifetimeScope(ContainerBuilder builder)
        {
            builder.RegisterAutofacConsumerFactory();

            builder.RegisterType<RetrieveImageConsumer>()
                   .AsSelf();

            builder.RegisterType<ImageRetrievalConsumerBusHost>()
                   .As<IServiceBusHost>();

            base.ConfigureLifetimeScope(builder);
        }
    }
}