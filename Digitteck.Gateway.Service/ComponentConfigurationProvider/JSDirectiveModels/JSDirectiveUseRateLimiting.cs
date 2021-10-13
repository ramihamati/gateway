using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDirectiveUseRateLimiting : JSDirectiveCore
    {
        [JsonProperty("ClientWhitelist")]
        public List<string> ClientWhitelist { get; set; }

        [JsonProperty("Period")]
        public string Period { get; set; }

        [JsonProperty("PeriodTimespan")]
        public int PeriodTimespan { get; set; }

        [JsonProperty("Limit")]
        public int Limit { get; set; }

        public JSDirectiveUseRateLimiting()
        {
            this.ClientWhitelist = new List<string>();
        }
    }

}
