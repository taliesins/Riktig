namespace Riktig.CoordinationService
{
    using System;
    using Coordination;
    using MassTransit;
    using MassTransit.Saga;
    using Topshelf;
    using Automatonymous;


    public class CoordinationService :
        ServiceControl
    {
        readonly Uri _address;
        readonly ImageRetrievalStateMachine _machine;
        ISagaRepository<ImageRetrievalState> _repository;
        IServiceBus _bus;

        public CoordinationService(Uri address, ImageRetrievalStateMachine machine)
        {
            _address = address;
            _machine = machine;
        }

        public bool Start(HostControl hostControl)
        {
            // use an in-memory saga repository to keep things simple, although SQLite would work
            // nicely here as well.
            _repository = new InMemorySagaRepository<ImageRetrievalState>();
            _bus = ServiceBusFactory.New(x =>
                {
                    x.ReceiveFrom(_address);
                    x.UseRabbitMq(r =>
                        {
                            r.ConfigureHost(_address, h =>
                                {
                                    // set username/password if required
                                    h.SetRequestedHeartbeat(1);
                                });
                        });

                    x.Subscribe(s =>
                        {
                            // subscribe the state machine to the bus, using the saga repository
                            s.StateMachineSaga(_machine, _repository, r =>
                                {
                                    // correlate the command to existing commands and use the 
                                    // requestId for the correlationId of the saga if it does not exist
                                    r.Correlate(_machine.Requested,
                                        (state, message) => state.SourceAddress == message.SourceAddress)
                                     .SelectCorrelationId(message => message.RequestId);

                                    // specify non-CorrelationId based correlation for the events
                                    r.Correlate(_machine.Retrieved,
                                        (state, message) => state.SourceAddress == message.SourceAddress);

                                    r.Correlate(_machine.NotFound,
                                        (state, message) => state.SourceAddress == message.SourceAddress);

                                    r.Correlate(_machine.RetrieveFailed,
                                        (state, message) => state.SourceAddress == message.SourceAddress);
                                });
                        });
                });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus.Dispose();

            return true;
        }
    }
}