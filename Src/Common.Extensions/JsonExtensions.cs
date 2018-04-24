using System.Collections.Generic;
using System.Linq;
using Common.Extensions.JsonNetConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Extensions
{
    public static class JsonExtensions
    {
        private const Formatting JsonFormatting = Formatting.None;

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
                                                                             {
                                                                                 NullValueHandling = NullValueHandling.Ignore, 
                                                                                 Formatting = JsonFormatting
                                                                             };
        private static readonly JsonSerializerSettings _dictionaryToObjectJsonSettings = new JsonSerializerSettings()
                                                                            {
                                                                                NullValueHandling = NullValueHandling.Ignore,
                                                                                Formatting = JsonFormatting,
                                                                                Converters = new JsonConverter[] { new DictionaryToObjectJsonConverter() }
                                                                            };


        public static string ObjectToJson(this object obj)
        {
            if (obj == null)
                return "{}";

            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }

        public static string MergeJson(this string json1, string json2)
        {
            var jobj1 = JObject.Parse(json1);
            var jobj2 = JObject.Parse(json2);
            jobj1.Merge(jobj2);
            return jobj1.ToString();
        }
        public static string ObjectToJsonMerge(this object obj1, params object[] objs)
        {
            var jobj1 = JObject.FromObject(obj1);
            foreach (object obj in objs)
            {
                if (obj == null)
                    continue;

                var jobj2 = JObject.FromObject(obj);
                jobj1.Merge(jobj2);
            }
            
            return jobj1.ToString();
        }

        public static TResult JsonToObject<TResult>(this string json)
        {
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        public static string DictionaryToJsonAsObject(this IDictionary<string, object> dict)
        {
            return JsonConvert.SerializeObject(dict, _dictionaryToObjectJsonSettings);
        }

        public static string RemoveJsonProperty(this string json, string propertyName)
        {
            return json.RemoveJsonProperties(new [] {propertyName});
        }
        public static string RemoveJsonProperties(this string json, IEnumerable<string> propertyNames)
        {
            if (propertyNames == null || !propertyNames.Any())
                return json;

            var jobj = JObject.Parse(json);
            foreach (string propertyName in propertyNames)
                jobj.Remove(propertyName);
            
            return jobj.ToString(JsonFormatting);
        }

    }
}
