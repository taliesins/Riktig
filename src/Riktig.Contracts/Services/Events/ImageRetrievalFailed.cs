namespace Riktig.Contracts.Services.Events
{
    using System;


    public interface ImageRetrievalFailed
    {
        /// <summary>
        /// Every event published has a unique identifier to support idempotency at a message level
        /// if desired
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The timestamp that the event occurred
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The command id that initiated the image retrieval
        /// </summary>
        Guid OriginatingCommandId { get; }

        /// <summary>
        /// The source address from which the image was retrieved
        /// </summary>
        Uri SourceAddress { get; }

        /// <summary>
        /// The reason why the image was not found
        /// </summary>
        string Reason { get; }
    }
}