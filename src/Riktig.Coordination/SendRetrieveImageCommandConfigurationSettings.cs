namespace Riktig.Coordination
{
    using System;
    using RapidTransit.Core;
    using RapidTransit.Core.Configuration;


    public class SendRetrieveImageCommandConfigurationSettings :
        SendRetrieveImageCommandSettings
    {
        readonly Uri _imageRetrievalServiceAddress;

        public SendRetrieveImageCommandConfigurationSettings(IConfigurationProvider configurationProvider,
            ITransportConfigurator transportConfigurator)
        {
            string queueName;
            if (!configurationProvider.TryGetSetting("ImageRetrievalServiceQueueName", out queueName))
                throw new ArgumentException("The configuration setting ImageRetrievalServiceQueueName was not found");

            _imageRetrievalServiceAddress = transportConfigurator.GetQueueAddress(queueName);
        }

        public Uri ImageRetrievalServiceAddress
        {
            get { return _imageRetrievalServiceAddress; }
        }
    }
}