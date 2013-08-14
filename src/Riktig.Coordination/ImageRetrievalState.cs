namespace Riktig.Coordination
{
    using System;
    using Automatonymous;
    using MassTransit;


    public class ImageRetrievalState :
        SagaStateMachineInstance
    {
        public ImageRetrievalState(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public ImageRetrievalState()
        {
        }

        /// <summary>
        /// When the state instance was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The timestamp when the image was first requested
        /// </summary>
        public DateTime FirstRequested { get; set; }

        /// <summary>
        /// The source address of the image
        /// </summary>
        public Uri SourceAddress { get; set; }

        public Guid CorrelationId { get; private set; }
        public IServiceBus Bus { get; set; }


        /// <summary>
        /// When the image was last retrieved
        /// </summary>
        public DateTime? LastRetrieved { get; set; }

        /// <summary>
        /// The local URI of the image
        /// </summary>
        public Uri LocalAddress { get; set; }

        /// <summary>
        /// The content type of the image
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The length in bytes of the image
        /// </summary>
        public int? ContentLength { get; set; }

        /// <summary>
        /// If faulted, the reason why it faulted
        /// </summary>
        public string Reason { get; set; }
    }
}