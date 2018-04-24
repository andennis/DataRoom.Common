using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void ObjectPropertiesToDictionaryTest()
        {
            var obj1 = new {P1 = 10, P2 = "123"};
            IDictionary<string, object> dict = obj1.ObjectPropertiesToDictionary();
            Assert.NotNull(dict);
            Assert.AreEqual(2, dict.Count);
            Assert.True(dict.ContainsKey("P1"));
            Assert.True(dict.ContainsKey("P2"));
            Assert.AreEqual(10, dict["P1"]);
            Assert.AreEqual("123", dict["P2"]);

            obj1 = null;
            Assert.Throws<ArgumentNullException>(() => obj1.ObjectPropertiesToDictionary());
        }

        [Test]
        public void GetPropertyValueTest()
        {
            var obj1 = new { P1 = 10, P2 = "123" };
            object val =  obj1.GetPropertyValue("P1");
            Assert.AreEqual(10, val);
        }
    }
}
