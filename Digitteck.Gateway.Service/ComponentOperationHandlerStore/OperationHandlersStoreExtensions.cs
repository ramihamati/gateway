using Microsoft.Extensions.DependencyInjection;
using System;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// Adds the operation handler to the services using a builder
    /// </summary>
    public static class OperationHandlersStoreExtensions
    {
        public static IServiceCollection AddOperationHandler(this IServiceCollection services, Action<OperationHandlingBuilder> builder)
        {
            services.AddSingleton<IOperationHandlersStore, OperationHandlersStore>(sp =>
            {
                OperationHandlingBuilder operationHandlingBuilder = new OperationHandlingBuilder(sp);
                builder(operationHandlingBuilder);
                return operationHandlingBuilder.Build();
            });

            return services;
        }
    }
}
