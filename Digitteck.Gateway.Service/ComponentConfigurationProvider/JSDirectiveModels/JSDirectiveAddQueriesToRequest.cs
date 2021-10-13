using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDirectiveAddQueriesToRequest :JSDirectiveCore
    {
        [JsonProperty("Executor")]
        public string Executor { get; set; }

        [JsonProperty("Arguments")]
        public IList<object> Arguments { get; }

        public JSDirectiveAddQueriesToRequest()
        {
            this.Arguments = new List<object>();
        }
    }

}
