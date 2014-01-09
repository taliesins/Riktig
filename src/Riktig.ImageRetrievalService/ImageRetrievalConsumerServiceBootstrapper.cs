namespace Riktig.ImageRetrievalService
{
    using Autofac;
    using RapidTransit.Core.Services;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Services;


    public class ImageRetrievalConsumerServiceBootstrapper :
        ServiceBusInstanceServiceBootstrapper
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

            builder.RegisterType<ImageRetrievalConsumerBusInstance>()
                   .As<IServiceBusInstance>();

            base.ConfigureLifetimeScope(builder);
        }
    }
}