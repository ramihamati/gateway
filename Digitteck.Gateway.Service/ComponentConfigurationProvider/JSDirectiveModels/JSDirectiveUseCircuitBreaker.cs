using Newtonsoft.Json;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDirectiveUseCircuitBreaker : JSDirectiveCore
    {
        [JsonProperty("AttemptsToOpenCircuit")]
        public int? AttemptsToOpenCircuit { get; set; }

        [JsonProperty("TimeoutValue")]
        public int? TimeoutValue { get; set; }

        [JsonProperty("FailoutResponseExecutor")]
        public string FailoutResponseExecutor { get; set; }
    }
}
