using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDirectiveAddHeadersToRequest : JSDirectiveCore
    {
        [JsonProperty("Executor")]
        public string Executor { get; set; }

        [JsonProperty("Arguments")]
        public IList<object> Arguments { get; set; }

        public JSDirectiveAddHeadersToRequest()
        {
            this.Arguments = new List<object>();
        }
    }
}
