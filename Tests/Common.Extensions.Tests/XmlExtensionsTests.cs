using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class XmlExtensionsTests
    {

        [Test]
        public void SaveToXml_LoadFromXmlTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template.xml");
            if (File.Exists(path))
                File.Delete(path);

            var testObj = new TestClass();
            testObj.SaveToXml(path);
            var testResultObj = path.LoadFromXml<TestClass>();

            Assert.NotNull(testResultObj);
            Assert.AreEqual(testObj.Str, testResultObj.Str);
            Assert.AreEqual(testObj.Str2, testResultObj.Str2);
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "passTemplate", Namespace = "http://www.mobilewallet.com")]
    public class TestClass
    {
        [XmlElement(ElementName = "Str")]
        public string Str { get; set; }
         [XmlElement(ElementName = "Str2")]
        public string Str2 { get; set; }
    }
}
