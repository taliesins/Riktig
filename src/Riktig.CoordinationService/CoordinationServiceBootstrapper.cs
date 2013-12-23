namespace Riktig.CoordinationService
{
    using Autofac;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Services;
    using RapidTransit.Integration.Services.QuartzIntegration;
    using Topshelf.Runtime;


    public class CoordinationServiceBootstrapper :
        TopshelfServiceBootstrapper<CoordinationServiceBootstrapper>
    {
        public CoordinationServiceBootstrapper(HostSettings hostSettings)
            : base(hostSettings)
        {
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceConfigurationProviderModule>();
            builder.RegisterModule<RabbitMqConfigurationModule>();
            builder.RegisterModule<HostServiceBusModule>();

            builder.RegisterType<ImageRetrievalTrackingServiceBootstrapper>()
                   .As<IServiceBootstrapper>();

            builder.RegisterType<MessageSchedulingServiceBootstrapper>()
                   .As<IServiceBootstrapper>();
        }
    }
}