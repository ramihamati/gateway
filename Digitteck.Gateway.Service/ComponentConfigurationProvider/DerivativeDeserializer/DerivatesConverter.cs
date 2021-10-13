using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    /// <summary>
    /// Converter to handle deserializing a list of json objects that map to a list of generic components
    /// </summary>
    public class DerivatesConverter : JsonConverter
    {
        private readonly Type _baseType;
        private readonly string _discriminatorName;

        public DerivatesConverter(List<JSDiscriminatorDef> discriminators, Type baseType, string discriminatorName)
        {
            this.Discriminators = discriminators;
            _baseType = baseType;
            _discriminatorName = discriminatorName;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(_baseType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            foreach (JSDiscriminatorDef disc in Discriminators)
            {
                if (jo[_discriminatorName].Value<string>() == null)
                {
                    throw new Exception($"The base type {_baseType.FullName} requires the discriminator {_discriminatorName}. Found null value for the discriminator");
                }

                if (jo[_discriminatorName].Value<string>() == disc.NodeValue)
                    return jo.ToObject(disc.ToType, serializer);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public List<JSDiscriminatorDef> Discriminators { get; }
    }
}
