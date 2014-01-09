namespace Riktig.CoordinationService
{
    using Autofac;
    using Coordination;
    using MassTransit.Saga;
    using RapidTransit.Core;
    using RapidTransit.Core.Services;
    using RapidTransit.Integration;
    using RapidTransit.Integration.Services;


    public class ImageRetrievalTrackingServiceBootstrapper :
        ServiceBusInstanceServiceBootstrapper
    {
        public ImageRetrievalTrackingServiceBootstrapper(ILifetimeScope lifetimeScope)
            : base(lifetimeScope, typeof(ImageRetrievalTrackingServiceBootstrapper))
        {
        }

        protected override void ConfigureLifetimeScope(ContainerBuilder builder)
        {
            builder.RegisterType<InMemorySagaRepository<ImageRetrievalState>>()
                   .As<ISagaRepository<ImageRetrievalState>>()
                   .SingleInstance();

            builder.RegisterType<AutofacStateMachineActivityFactory>()
                   .As<IStateMachineActivityFactory>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<SendRetrieveImageCommandConfigurationSettings>()
                   .As<SendRetrieveImageCommandSettings>()
                   .SingleInstance();

            builder.RegisterType<SendRetrieveImageCommandActivity>();

            builder.RegisterType<ImageRetrievalStateMachine>()
                   .SingleInstance();

            builder.RegisterType<ImageRetrievalStateBusInstance>()
                   .As<IServiceBusInstance>();

            base.ConfigureLifetimeScope(builder);
        }
    }
}