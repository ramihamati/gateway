using Newtonsoft.Json;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDirectiveUseRetryPolicy : JSDirectiveCore
    {
        [JsonProperty("RetryCount")]
        public int? RetryCount { get; set; }

        [JsonProperty("TimeBetweenRetries")]
        public int? TimeBetweenRetries { get; set; }
    }
}
