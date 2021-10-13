using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Digitteck.Gateway.Service
{
    public static class ConfigurationProviderExtension
    {
        public static IServiceCollection UseConfigurationProvier(this IServiceCollection services, Action<ConfigurationProviderBuilder> builder)
        {
            ConfigurationProviderBuilder configurationProviderBuilder = new ConfigurationProviderBuilder();

            builder(configurationProviderBuilder);

            services.TryAddSingleton<IConfigurationProvider>((sp) =>
            {
                return configurationProviderBuilder.Build(sp);
            });

            return services;
        }
    }
}
