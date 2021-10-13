using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public class GatewayContext
    {
        //original configuration
        public GatewayConfiguration OriginalConfiguration { get; }
        
        //parsed routes
        public List<RouteContext> RouteContexts { get;  }

        //executiong http context
        public HttpContext HttpContext { get; }

        public GatewayContext(HttpContext httpContext, GatewayConfiguration gatewayConfiguration)
        {
            HttpContext = httpContext;
            this.OriginalConfiguration = gatewayConfiguration;
            this.RouteContexts = new List<RouteContext>();
        }

        public void AddRouteContext(RouteContext routeContext)
        {
            this.RouteContexts.Add(routeContext);
        }
    }
}
