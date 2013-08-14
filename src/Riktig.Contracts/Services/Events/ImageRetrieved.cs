namespace Riktig.Contracts.Services.Events
{
    using System;


    public interface ImageRetrieved
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
        /// The local address where the image was cached
        /// </summary>
        Uri LocalAddress { get; }

        /// <summary>
        /// The MIME content type of the image
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The length of the content, in bytes
        /// </summary>
        int ContentLength { get; }
    }
}