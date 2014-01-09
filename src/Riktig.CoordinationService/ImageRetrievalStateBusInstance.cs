namespace Riktig.CoordinationService
{
    using Automatonymous;
    using Coordination;
    using MassTransit.Saga;
    using RapidTransit.Core.Configuration;
    using RapidTransit.Core.Services;


    public class ImageRetrievalStateBusInstance :
        ServiceBusInstance
    {
        public ImageRetrievalStateBusInstance(IConfigurationProvider configuration, ImageRetrievalStateMachine machine,
            ISagaRepository<ImageRetrievalState> sagaRepository)
            : base(configuration, "ImageRetrievalTrackingQueueName", "ImageRetrievalTrackingConsumerLimit", 1)
        {
            this.StateMachineSaga(machine, sagaRepository, machine.ConfigureStateMachineCorrelations);
        }
    }
}