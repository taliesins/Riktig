namespace Riktig.Web.Controllers
{
    using System;
    using RapidTransit.Core;
    using RapidTransit.Core.Configuration;


    public class ImageServiceConfigurationSettings :
        ImageServiceSettings
    {
        readonly Uri _imageTrackingServiceAddress;

        public ImageServiceConfigurationSettings(IConfigurationProvider configurationProvider,
            ITransportConfigurator transportConfigurator)
        {
            string queueName;
            if (!configurationProvider.TryGetSetting("ImageRetrievalTrackingQueueName", out queueName))
                throw new ArgumentException("The configuration setting ImageRetrievalTrackingQueueName was not found");

            _imageTrackingServiceAddress = transportConfigurator.GetQueueAddress(queueName);
        }

        public Uri ImageTrackingServiceAddress
        {
            get { return _imageTrackingServiceAddress; }
        }
    }
}