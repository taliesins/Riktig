namespace Riktig.ImageRetrievalService
{
    using System.Configuration;
    using Autofac;
    using RapidTransit.Core.Configuration;
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

            builder.Register(GetRetrieveImageSettings)
                   .As<RetrieveImageSettings>()
                   .SingleInstance();

            builder.RegisterType<RetrieveImageConsumer>()
                   .AsSelf();

            builder.RegisterType<ImageRetrievalConsumerBusInstance>()
                   .As<IServiceBusInstance>();

            base.ConfigureLifetimeScope(builder);
        }

        static RetrieveImageSettings GetRetrieveImageSettings(IComponentContext context)
        {
            RetrieveImageSettings settings;
            if (context.Resolve<ISettingsProvider>().TryGetSettings(out settings))
                return settings;

            throw new ConfigurationErrorsException("Unable to resolve RetrieveImageSettings from configuration");
        }
    }
}