namespace Riktig.Contracts.Api
{
    using System;


    public interface ImageRequestFaulted
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
        /// The image source requested
        /// </summary>
        Uri SourceAddress { get; }

        /// <summary>
        /// The reason the request failed
        /// </summary>
        string Reason { get; }
    }
}