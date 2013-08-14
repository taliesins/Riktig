namespace Riktig.Contracts.Services.Commands
{
    using System;


    /// <summary>
    /// Retrieve and image using the specified URI
    /// </summary>
    public interface RetrieveImage
    {
        /// <summary>
        /// Commands need a unique identifier for correlation of events
        /// </summary>
        Guid CommandId { get; }

        /// <summary>
        /// The time when the command was sent
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The source of the image
        /// </summary>
        Uri SourceAddress { get; }
    }
}