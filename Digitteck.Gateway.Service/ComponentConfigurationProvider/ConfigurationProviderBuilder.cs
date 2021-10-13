using System;
using Digitteck.Gateway.Service.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Digitteck.Gateway.Service
{

    public class ConfigurationProviderBuilder
    {
        private ConfigurationProviderType configurationProviderType;

        public void UseJsonConfigurationProvider()
        {
            configurationProviderType = ConfigurationProviderType.JsonConfigurationProvider;
        }

        internal IConfigurationProvider Build(IServiceProvider serviceProvider)
        {
            if (configurationProviderType == ConfigurationProviderType.JsonConfigurationProvider)
            {
                return ActivatorUtilities.CreateInstance<JsonConfigurationProvider>(serviceProvider);
            }

            throw new GatewayException(ErrorCode.ConfigurationProvider, "The configuration provider is not set.");
        }
    }
}
