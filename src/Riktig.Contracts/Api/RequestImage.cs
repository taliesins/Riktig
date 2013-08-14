namespace Riktig.Contracts.Api
{
    using System;

    /// <summary>
    /// Request an image from the image retrieval system
    /// </summary>
    public interface RequestImage
    {
        /// <summary>
        /// A unique request id sent by the originator of the request
        /// </summary>
        Guid RequestId { get; }

        /// <summary>
        /// The timestamp the request was sent
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The source address of the image
        /// </summary>
        Uri SourceAddress { get; }
    }
}