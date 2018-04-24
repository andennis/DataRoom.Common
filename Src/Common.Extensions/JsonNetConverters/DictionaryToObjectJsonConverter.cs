using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common.Extensions.JsonNetConverters
{
    public class DictionaryToObjectJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary<string,object>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dict = (IDictionary<string, object>) value;
            writer.WriteStartObject();
            foreach (var item in dict)
            {
                writer.WritePropertyName(item.Key);

                var val = item.Value as JsonValueWithoutQuotes;
                if (val != null)
                    writer.WriteRawValue(val.Value);
                else
                    serializer.Serialize(writer, item.Value);
            }
            writer.WriteEndObject();
        }
    }
}
