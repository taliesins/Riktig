namespace Riktig.ImageRetrievalService
{
    using RapidTransit.Core.Configuration;


    public interface RetrieveImageSettings :
        ISettings
    {
        string LocalImageCache { get; }
    }
}