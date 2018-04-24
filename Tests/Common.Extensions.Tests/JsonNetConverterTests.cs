using System;
using System.Collections.Generic;
using Common.Extensions.JsonNetConverters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class JsonNetConverterTests
    {
        private class MyClass
        {
            [JsonConverter(typeof(DateTimeToJavaScriptDateConverter))]
            public DateTime DateTime1 { get; set; }

            [JsonConverter(typeof(DateTimeToJavaScriptDateConverter))]
            public DateTime? DateTime2 { get; set; }

            [JsonConverter(typeof(DateTimeToJavaScriptDateConverter))]
            public DateTime? DateTime3 { get; set; }
        }

        [Test]
        public void DateTimeToJavaScriptDateConverterTest()
        {
            var dt1 = DateTime.Now.Date;
            var dt2 = dt1.AddDays(1);
            var obj = new MyClass()
                      {
                          DateTime1 = dt1,
                          DateTime2 = dt2
                      };

            string json = obj.ObjectToJson();
            Assert.True(json.Contains(string.Format("\"DateTime1\":new Date({0}, {1}, {2})", dt1.Year, dt1.Month, dt1.Day)));
            Assert.True(json.Contains(string.Format("\"DateTime2\":new Date({0}, {1}, {2})", dt2.Year, dt2.Month, dt2.Day)));
        }

        [Ignore("It should be implemented")]
        [Test]
        public void DictionaryToObjectJsonConverterTests()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DictionaryToJsonAsObjectTest()
        {
            var dict = new Dictionary<string, object>()
                       {
                           {"Name", "Name1"},
                           {"Value", 5},
                           {"P1", "123"},
                           {"Fnc", new JsonValueWithoutQuotes("Fnc1")}
                       };

            string json = dict.DictionaryToJsonAsObject();
            Assert.NotNull(json);
            Assert.IsNotEmpty(json);

            Assert.True(json.Contains("\"Name\":\"Name1\""));
            Assert.True(json.Contains("\"Value\":5"));
            Assert.True(json.Contains("\"P1\":\"123\""));
            Assert.True(json.Contains("\"Fnc\":Fnc1"));
        }

        [Ignore("It should be implemented")]
        [Test]
        public void JsonValueWithoutQuotesConverterTest()
        {
            throw new NotImplementedException();
        }

    }
}
