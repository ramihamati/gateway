using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSOperationAggregateResponses : JSOperationCore
    {
        [JsonProperty("Directives")]
        public List<JSDirectiveCore> Directives { get; set; }

        /// <summary>
        /// This property will set the order of task execution if the downstream property RunAsync is true.
        /// It contains a list of operation tags, and this op will wait for other ops to be executed before executing.
        /// </summary>
        [JsonProperty("WaitFor")]
        public List<string> WaitFor { get; set; }

        [JsonProperty("Executor")]
        public string Executor { get; set; }

        public JSOperationAggregateResponses() 
        {
            WaitFor = new List<string>();
            this.Directives = new List<JSDirectiveCore>();
        }
    }

}
