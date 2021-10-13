using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Digitteck.Gateway.Service
{
    internal sealed class ValueObjectSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ValueObject<string>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType?.BaseType?.GetGenericTypeDefinition()?.Equals(typeof(ValueObject<>)) ?? false)
            {
                if (reader.TokenType == JsonToken.String
                    || reader.TokenType == JsonToken.Float
                    || reader.TokenType == JsonToken.Date
                    || reader.TokenType == JsonToken.Boolean
                    || reader.TokenType == JsonToken.Bytes
                    || reader.TokenType == JsonToken.Integer)
                {
                    object instance = Activator.CreateInstance(objectType);

                    var propertyInfo = instance.GetType().GetProperty(nameof(ValueObject<string>.Value), BindingFlags.Public | BindingFlags.Instance);

                    Type targetType = objectType?.BaseType?.GenericTypeArguments?.FirstOrDefault();

                    //reader.Value is the text value of the token
                    object sourceValue = reader.Value;

                    if (sourceValue == null || targetType == null)
                    {
                        return null;
                    }

                    object convertervalue = JsonConvert.DeserializeObject(reader.Value.ToString(), targetType);

                    if (propertyInfo != null
                        && (convertervalue?.GetType().Equals(targetType) ?? false))
                    {
                        propertyInfo.SetValue(instance, convertervalue);
                    }

                    return instance;
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value?.GetType()?.BaseType?.GetGenericTypeDefinition()?.Equals(typeof(ValueObject<>)) ?? false)
            {
                var propertyInfo = value.GetType().GetProperty(nameof(ValueObject<string>.Value), BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    object propValue = propertyInfo.GetValue(value);
                    serializer.Serialize(writer, propValue);
                    return;
                }
            }

            serializer.Serialize(writer, null);
        }
    }
}
