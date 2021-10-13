using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Digitteck.Gateway.Service
{
    public static class GWServicesExtensions
    {
        public static IServiceCollection AddGW(this IServiceCollection services, string confFile)
        {
            services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();

            services.AddTransient<GatewayMiddleware>();
            services.TryAddSingleton<IGWRuntime, GWRuntime>();
            services.TryAddSingleton<IObjectFactory, ObjectFactory>();
            
            services.UseContentProvider(builder => builder.UseGlobalConfiguration(confFile).UseJsonContentProvider());
            services.UseConfigurationProvier(builder => builder.UseJsonConfigurationProvider());
            services.TryAddSingleton<IConfigurationValidator, ConfigurationValidator>();

            services.TryAddSingleton<ITemplateToRequestMatchService, TemplateToRequestMatchService>();
            services.TryAddSingleton<IOperationResponseConverter, OperationResponseConverter>();
            services.TryAddSingleton<IOperationResultStore, OperationResultStore>();
            services.TryAddSingleton<IOperationGroupDispatcher, OperationGroupDispatcher>();
            services.TryAddSingleton<IPlaceholderExtractor, PlaceholderExtractor>();
            services.TryAddSingleton<IUriHelperService, UriHelperService>();
            services.TryAddSingleton<IRequestMapper, RequestMapper>();
            services.TryAddSingleton<IAggregateBinder, AggregateBinder>();

            services.AddOperationHandler(builder =>
            {
                builder.AddHandlerForOperation<OperationCall, OperationCallHandler>();
                builder.AddHandlerForOperation<OperationReturn, OperationReturnHandler>();
                builder.AddHandlerForOperation<OperationAggregateResponse, OperationAggregateResponseHandler>();
            });

            return services;
        }
    }
}
