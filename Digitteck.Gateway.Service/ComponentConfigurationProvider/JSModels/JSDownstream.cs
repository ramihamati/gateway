using Digitteck.Gateway.Service.JsonModelProvider;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.SourceModels
{
    public class JSDownstream
    {
        [JsonProperty("Operations")]
        public List<JSOperationCore> Operations { get; set; }

        /// <summary>
        /// If the flag is true, all operations defined downstream will run asynchronously, respeciting the order defined by the wait property
        /// defined in every operation.
        /// </summary>
        [JsonProperty("RunAsync")]
        public bool RunAsync { get; set; } = false;
    }
}
