using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Digitteck.Gateway.Service
{
    public class ObjectFactory : IObjectFactory
    {
        //services from provider
        //private readonly ITemplateToRegexPatternService _templateToRegexService;
        private readonly IPlaceholderExtractor _placeholderExtractor;
        private readonly IOperationHandlersStore _operationHandlersStore;
        private readonly IUriHelperService _uriHelper;

        //object factories

        public ObjectFactory(IServiceProvider services)
        {
            try
            {
                _placeholderExtractor = services.GetRequiredService<IPlaceholderExtractor>();
                _operationHandlersStore = services.GetRequiredService<IOperationHandlersStore>();
                _uriHelper = services.GetRequiredService<IUriHelperService>();
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ObjectFactory, ex.Message, ex);
            }
        }

        public RouteContext CreateRouteContext(RouteDefinition routeDefinition)
        {
            try
            {
                string entryTemplate = _uriHelper.EnsureStartingPathDelimiter(routeDefinition.Upstream.EntryTemplate);

                TemplatePathAndQueryObject template = new TemplatePathAndQueryObject(entryTemplate);

                //validate downstream agains upstream. Downstream should contain placeholders that are defined in upstream

                return new RouteContext
                {
                    RunAsync = routeDefinition.Downstream.RunAsync,
                    Operations = routeDefinition.Downstream.Operations,
                    UpstreamTemplate = new UpstreamTemplate
                    {
                        EntryTemplate = template,
                        HttpMethodType = routeDefinition.Upstream.HttpMethod,
                        //RegexTemplate = _templateToRegexService.CreateRegexToMatchIncomeRequests(template),
                        TemplatePlaceholders = _placeholderExtractor.GetPlaceholdersFromTemplate(template)
                    }
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ObjectFactory, ex.Message, ex);
            }
        }

        public GatewayContext CreateGatewayContext(HttpContext httpContext, GatewayConfiguration configuration)
        {
            return new GatewayContext(httpContext, configuration);
        }

        public OperationDispatchUnit CreateOperationDispatchUnit(OperationCore operationCore, GatewayContext httpContext)
        {
            try
            {
                return new OperationDispatchUnit(operationCore, httpContext, _operationHandlersStore);
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ObjectFactory, ex.Message, ex);
            }
        }
    }
}
