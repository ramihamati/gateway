using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class GWRuntime : IGWRuntime
    {
        private readonly ITemplateToRequestMatchService _requestMatcher;
        private readonly IOperationGroupDispatcher _operationGroupDispatcher;
        private readonly IObjectFactory _appbuilder;
        private readonly IConfigurationValidator _configurationValidator;

        public GWRuntime(
            ITemplateToRequestMatchService requestMatcher,
            IObjectFactory appBuilder,
            IOperationGroupDispatcher dispatcher,
            IConfigurationValidator configurationValidator)
        {
            _requestMatcher = requestMatcher;
            _operationGroupDispatcher = dispatcher;
            _appbuilder = appBuilder;
            _configurationValidator = configurationValidator;
        }

        public async Task<OperationResult> Execute(HttpContext httpContext, GatewayConfiguration configuration)
        {
            ValidationMessage validation = _configurationValidator.Validate(configuration);
            
            if (!validation.IsValid)
            {
                throw new GatewayException(ErrorCode.GWRuntime, validation.GetValidationMessage());
            }
            
            //create gateway context
            GatewayContext context = this._appbuilder.CreateGatewayContext(httpContext, configuration);
            //create route contexts
            foreach (RouteDefinition routeDef in context.OriginalConfiguration.RouteModels)
            {
                RouteContext routeContext = _appbuilder.CreateRouteContext(routeDef);
                context.AddRouteContext(routeContext);
            }

            //match route to execute
            RouteContext matchingContext = _requestMatcher.FindMatch(httpContext.Request, context.RouteContexts);

            //add operations to dispatcher
            foreach (OperationCore operation in matchingContext.Operations)
            {
                this._operationGroupDispatcher.AddOperationBlock(_appbuilder.CreateOperationDispatchUnit(operation, context));
            }

            return await _operationGroupDispatcher.DispatchOperationBlocks(matchingContext);
        }
    }
}
