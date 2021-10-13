using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSOperationReturn : JSOperationCore
    {
        [JsonProperty("Directives")]
        public List<JSDirectiveCore> Directives { get; set; }

        [JsonProperty("ReturnTag")]
        public string ReturnTag { get; set; }

        public JSOperationReturn()
        {
            this.Directives = new List<JSDirectiveCore>();
        }
    }
}
