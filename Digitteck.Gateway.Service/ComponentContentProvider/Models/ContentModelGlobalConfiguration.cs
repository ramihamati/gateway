using Newtonsoft.Json;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.ComponentContentProvider.Models
{
    public class ContentModelGlobalConfiguration
    {
        [JsonProperty("RouteSources")]
        public List<string> RouteSources { get; set; }
    }
}
