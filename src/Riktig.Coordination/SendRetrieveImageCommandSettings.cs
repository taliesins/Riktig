namespace Riktig.Coordination
{
    using System;
    using RapidTransit.Core.Configuration;


    public interface SendRetrieveImageCommandSettings :
        ISettings
    {
        Uri ImageRetrievalServiceAddress { get; }
    }
}