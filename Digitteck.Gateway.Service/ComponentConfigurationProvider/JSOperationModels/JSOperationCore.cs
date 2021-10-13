using Newtonsoft.Json;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    /// <summary>
    /// Do not make class abstract. It will cause problems with the deserialzier s(for some reasons...)
    /// </summary>
    public class JSOperationCore
    {
        [JsonProperty("OperationTag")]
        public string OperationTag { get; set; }

        //important for grouping
        public JSOperationCore Clone() 
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this), this.GetType()) as JSOperationCore;
        }
    }
}
