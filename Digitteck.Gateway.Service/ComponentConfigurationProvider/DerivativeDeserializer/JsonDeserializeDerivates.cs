using Digitteck.Gateway.SourceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JsonDeserializeDerivates
    {
        private JSConverterBuilder converterBuilder;

        private List<JsonConverter> converters;

        public JsonDeserializeDerivates()
        {
            this.converters = new List<JsonConverter>();
        }

        public JsonDeserializeDerivates CreateConverter(Action<JSConverterBuilder> builder)
        {
            this.converterBuilder = new JSConverterBuilder();

            builder(this.converterBuilder);

            converters.Add(this.converterBuilder.Build());

            return this;
        }

        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
            {
                Converters = this.converters
            });
        }
    }
}
