namespace Riktig.CoordinationService
{
    using System;
    using Autofac;
    using Coordination;
    using Topshelf;
    using Topshelf.Logging;
    using Topshelf.Runtime;


    public class CoordinationServiceBootstrapper :
        IDisposable
    {
        readonly IContainer _container;
        readonly LogWriter _log = HostLogger.Get<CoordinationServiceBootstrapper>();

        public CoordinationServiceBootstrapper(HostSettings hostSettings, Uri serviceAddress, Uri address)
        {
            _log.InfoFormat("Configuring Service Container");

            var builder = new ContainerBuilder();

            builder.RegisterInstance(hostSettings);

            builder.RegisterType<SendRetrieveImageCommandActivity>()
                   .WithParameter(new TypedParameter(typeof(Uri), serviceAddress));

            builder.RegisterGeneric(typeof(AutofacStateMachineActivityFactory<>))
                   .As(typeof(IStateMachineActivityFactory<>));

            builder.RegisterType<ImageRetrievalStateMachine>()
                   .SingleInstance();

            builder.RegisterType<CoordinationService>()
                .WithParameter(new TypedParameter(typeof(Uri), address));

            _container = builder.Build();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public T GetService<T>()
            where T : ServiceControl
        {
            return _container.Resolve<T>();
        }
    }
}