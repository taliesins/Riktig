namespace Riktig.CoordinationService
{
    using System;
    using System.Diagnostics;
    using MassTransit.Log4NetIntegration.Logging;
    using MassTransit.Monitoring;
    using Topshelf;
    using Topshelf.Logging;
    using Topshelf.Runtime;


    class Program
    {
        static CoordinationServiceBootstrapper _bootstrapper;

        static int Main()
        {
            // Topshelf uses it
            Log4NetLogWriterFactory.Use("log4net.config");

            // MassTransit uses it
            Log4NetLogger.Use();


            return (int)HostFactory.Run(x =>
                {
                    x.AfterInstall(() =>
                        {
                            VerifyEventLogSourceExists();

                            // this will force the performance counters to register during service installation
                            // making them created - of course using the InstallUtil stuff completely skips
                            // this part of the install :(
                            ServiceBusPerformanceCounters counters = ServiceBusPerformanceCounters.Instance;
                        });


                    x.Service(CreateCoordinationService,
                        s => s.AfterStoppingService(() =>
                            {
                                if (_bootstrapper != null)
                                    _bootstrapper.Dispose();
                            }));
                });
        }

        static CoordinationService CreateCoordinationService(HostSettings hostSettings)
        {
            Log4NetLogger.Use();

            // simple but effective, this should be configuration settings of course
            var address = new Uri("rabbitmq://localhost/riktig-coordinationservice");
            var serviceAddress = new Uri("rabbitmq://localhost/riktig-imageretrievalservice");


            _bootstrapper = new CoordinationServiceBootstrapper(hostSettings, serviceAddress, address);

            return _bootstrapper.GetService<CoordinationService>();
        }

        static void VerifyEventLogSourceExists()
        {
            if (!EventLog.SourceExists("Riktig Coordination Service"))
                EventLog.CreateEventSource("Riktig Coordination Service", "MassTransit");
        }
    }
}