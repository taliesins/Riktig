namespace Riktig.CoordinationService
{
    using System;
    using System.Diagnostics;
    using Coordination;
    using MassTransit.Log4NetIntegration.Logging;
    using Topshelf;
    using Topshelf.Logging;


    class Program
    {
        static int Main()
        {
            // Topshelf uses it
            Log4NetLogWriterFactory.Use("log4net.config");

            // MassTransit uses it
            Log4NetLogger.Use();

            // simple but effective, this should be configuration settings of course
            var address = new Uri("rabbitmq://localhost/riktig-coordinationservice");
            var serviceAddress = new Uri("rabbitmq://localhost/riktig-imageretrievalservice");

            // create the state machine for the service
            var machine = new ImageRetrievalStateMachine(serviceAddress);

            return (int)HostFactory.Run(x =>
                    {
                        x.AfterInstall(() =>
                        {
                            VerifyEventLogSourceExists();

                            // this will force the performance counters to register during service installation
                            // making them created - of course using the InstallUtil stuff completely skips
                            // this part of the install :(
                            var counters = MassTransit.Monitoring.ServiceBusPerformanceCounters.Instance;
                        });


                        x.Service(settings => new CoordinationService(address, machine));
                    });
        }
        static void VerifyEventLogSourceExists()
        {
            if (!EventLog.SourceExists("Riktig Coordination Service"))
                EventLog.CreateEventSource("Riktig Coordination Service", "MassTransit");
        }


    }
}