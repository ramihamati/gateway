using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.SourceModels
{
    public class JSEndPointModel
    {
        [JsonProperty("Template")]
        public string EntryTemplate { get; set; }

        [JsonProperty("HttpMethod")]
        public JSHttpMethodType HttpMethod { get; set; }
    }
}
