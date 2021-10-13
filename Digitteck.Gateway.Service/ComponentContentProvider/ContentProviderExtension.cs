using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Digitteck.Gateway.Service
{
    public static class ContentProviderExtension
    {
        public static IServiceCollection UseContentProvider(this IServiceCollection services, Action<ContentProviderBuilder> builder)
        {
            ContentProviderBuilder contentBuilder = new ContentProviderBuilder();

            builder(contentBuilder);

            services.TryAddSingleton<IContentProvider>(contentBuilder.GetContentProvider());

            return services;
        }
    }
}
