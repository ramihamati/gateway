using Digitteck.Gateway.Service.ComponentConfigurationProvider.Models;
using Digitteck.Gateway.SourceModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSOperationCall : JSOperationCore
    { 
        [JsonProperty("Directives")]
        public List<JSDirectiveCore> Directives { get; set; }

        [JsonProperty("Template")]
        public string Template { get; set; }

        [JsonProperty("Scheme")]
        public JSScheme Scheme { get; set; }

        [JsonProperty("ServerHost")]
        public string ServerHost { get; set; }

        [JsonProperty("ServerPort")]
        public int ServerPort { get; set; }

        [JsonProperty("HttpMethod")]
        public JSHttpMethodType HttpMethod { get; set; }

        /// <summary>
        /// This property will set the order of task execution if the downstream property RunAsync is true.
        /// It contains a list of operation tags, and this op will wait for other ops to be executed before executing.
        /// </summary>
        [JsonProperty("WaitFor")]
        public List<string> WaitFor { get; set; }

        public JSOperationCall()
        {
            this.Directives = new List<JSDirectiveCore>();
            this.WaitFor = new List<string>();
        }
    }
}
