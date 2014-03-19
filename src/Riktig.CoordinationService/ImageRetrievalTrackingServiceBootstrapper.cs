namespace Riktig.CoordinationService
{
    using Autofac;
    using Automatonymous.NHibernateIntegration;
    using Coordination;
    using MassTransit.NHibernateIntegration.Saga;
    using MassTransit.Saga;
    using NHibernate;
    using RapidTransit.Core;
    using RapidTransit.Core.Configuration;
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
            builder.Register(CreateImageRetrievalSessionFactory)
                   .SingleInstance();

            builder.RegisterType<NHibernateSagaRepository<ImageRetrievalState>>()
                   .As<ISagaRepository<ImageRetrievalState>>()
                   .SingleInstance();

            builder.RegisterType<AutofacStateMachineActivityFactory>()
                   .As<IStateMachineActivityFactory>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<SendRetrieveImageCommandConfigurationSettings>()
                   .As<SendRetrieveImageCommandSettings>()
                   .SingleInstance();

            builder.RegisterType<SendRetrieveImageCommandActivity>();

            builder.Register(context =>
                {
                    var stateMachineActivityFactory = context.Resolve<IStateMachineActivityFactory>();
                    var machine = new ImageRetrievalStateMachine(stateMachineActivityFactory);

                    AutomatonymousStateUserType<ImageRetrievalStateMachine>.SaveAsString(machine);

                    return machine;
                }).SingleInstance();

            builder.RegisterType<ImageRetrievalStateBusInstance>()
                   .As<IServiceBusInstance>();

            base.ConfigureLifetimeScope(builder);
        }

        ISessionFactory CreateImageRetrievalSessionFactory(IComponentContext context)
        {
            var connectionStringProvider = context.Resolve<IConnectionStringProvider>();

            context.Resolve<ImageRetrievalStateMachine>();

            string connectionString;
            string providerName;
            if (connectionStringProvider.TryGetConnectionString("ImageRetrievalState",
                out connectionString, out providerName))
            {
                return new SQLiteSessionFactoryProvider(connectionString, typeof(ImageRetrievalStateMap))
                    .GetSessionFactory();
            }

            return new SQLiteSessionFactoryProvider(typeof(ImageRetrievalStateMap))
                .GetSessionFactory();
        }
    }
}