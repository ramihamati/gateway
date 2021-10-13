using Digitteck.Gateway.Service.JsonModelProvider;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.SourceModels
{
    public class JSGWConfigurationRaw
    {
        [JsonProperty("Directives")]
        public List<JSDirectiveCore> GlobalDirectives { get; set; }
    }
}
