namespace Riktig.Web.Controllers
{
    using System;
    using RapidTransit.Core.Configuration;


    public interface ImageServiceSettings :
        ISettings
    {
        Uri ImageTrackingServiceAddress { get; }
    }
}