using Digitteck.Gateway.Service.JsonModelProvider;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Digitteck.Gateway.SourceModels
{
    public class JSGWConfiguration
    {
        public List<JSDirectiveCore> GlobalDirectives { get; }

        public List<JSRouteDefinition> RouteModels { get; }

        public JSGWConfiguration()
        {
            this.RouteModels = new List<JSRouteDefinition>();
            this.GlobalDirectives = new List<JSDirectiveCore>();
        }
    }
}
