namespace Riktig.CoordinationService
{
    using System;
    using System.IO;
    using MassTransit.Log4NetIntegration.Logging;
    using RapidTransit.Integration.Services;
    using Topshelf;
    using Topshelf.Logging;
    using log4net.Config;


    class Program
    {
        static int Main()
        {
            var configurator = new RapidTransitHostConfigurator<CoordinationServiceBootstrapper>();

            configurator.OnStarting += settings => ConfigureLog4Net();

            return (int)HostFactory.Run(configurator.Configure);
        }

        static void ConfigureLog4Net()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            var configFile = new FileInfo(file);
            if (configFile.Exists)
                XmlConfigurator.ConfigureAndWatch(configFile);
            else
                BasicConfigurator.Configure();

            Log4NetLogWriterFactory.Use();
            Log4NetLogger.Use();
        }


//        static CoordinationService CreateCoordinationService(HostSettings hostSettings)
//        {
//            Log4NetLogger.Use();
//
//            // simple but effective, this should be configuration settings of course
//            var address = new Uri("rabbitmq://localhost/riktig-coordinationservice");
//            var serviceAddress = new Uri("rabbitmq://localhost/riktig-imageretrievalservice");
//
//
//            _bootstrapper = new ImageRetrievalTrackingServiceBootstrapper(hostSettings, serviceAddress, address);
//
//            return _bootstrapper.GetService<CoordinationService>();
//        }
//
    }
}