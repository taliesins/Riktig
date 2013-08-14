namespace Riktig.Contracts.Api
{
    using System;


    public interface ImageRequested
    {
        Guid EventId { get; }
        DateTime Timestamp { get; }

        Guid OriginatingCommandId { get; }
        Uri SourceAddress { get; }
    }
}