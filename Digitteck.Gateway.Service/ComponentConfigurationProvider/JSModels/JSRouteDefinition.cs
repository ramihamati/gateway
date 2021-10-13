using Newtonsoft.Json;

namespace Digitteck.Gateway.SourceModels
{
    public class JSRouteDefinition
    {
        [JsonProperty("Endpoint")]
        public JSEndPointModel Endpoint { get; set; }

        [JsonProperty("Downstream")]
        public JSDownstream Downstream { get; set; }
    }

}
