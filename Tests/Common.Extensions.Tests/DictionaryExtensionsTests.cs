using System.Collections.Generic;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void AddRangeWithSrcImportantTest()
        {
            var d1 = new Dictionary<string, string>()
                     {
                         {"K1", "V1"}
                     };
            var d2 = new Dictionary<string, string>()
                     {
                         {"K1", "V11"},
                         {"K2", "V2"}
                     };

            d1.AddRange(d2, isSrcImportant: true);

            Assert.AreEqual(2, d1.Count);
            Assert.AreEqual("V11", d1["K1"]);
            Assert.True(d1.ContainsKey("K2"));
            Assert.AreEqual("V2", d1["K2"]);
        }

        [Test]
        public void AddRangeWithDstImportantTest()
        {
            var d1 = new Dictionary<string, string>()
                     {
                         {"K1", "V1"}
                     };
            var d2 = new Dictionary<string, string>()
                     {
                         {"K1", "V11"},
                         {"K2", "V2"}
                     };

            d1.AddRange(d2, isSrcImportant: false);

            Assert.AreEqual(2, d1.Count);
            Assert.AreEqual("V1", d1["K1"]);
            Assert.True(d1.ContainsKey("K2"));
            Assert.AreEqual("V2", d1["K2"]);
        }

    }
}
