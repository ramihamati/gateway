using Microsoft.AspNetCore.Http;

namespace Digitteck.Gateway.Service
{
    public interface IObjectFactory
    {
        RouteContext CreateRouteContext(RouteDefinition routeDefinition);
        GatewayContext CreateGatewayContext(HttpContext httpContext, GatewayConfiguration configuration);
        OperationDispatchUnit CreateOperationDispatchUnit(OperationCore operationCore, GatewayContext httpContext);
    }
}