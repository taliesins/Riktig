namespace Riktig.Contracts.Api
{
    using System;


    public interface ImageRequestCompleted
    {
        /// <summary>
        /// The unique identifier for the event
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The timestamp when the event was produced
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The timestamp when the image was retrieved
        /// </summary>
        DateTime Retrieved { get; }

        /// <summary>
        /// The image source requested
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