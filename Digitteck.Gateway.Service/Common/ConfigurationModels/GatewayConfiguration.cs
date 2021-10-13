using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public class GatewayConfiguration
    {
        public List<DirectiveCore> GlobalDirectives { get; set; }

        public List<RouteDefinition> RouteModels { get; set; }

        public GatewayConfiguration()
        {
            this.RouteModels = new List<RouteDefinition>();
            this.GlobalDirectives = new List<DirectiveCore>();
        }
    }
}
