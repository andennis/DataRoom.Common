using System.IO;
using System.Text;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class FileExtensionsTests
    {
        [Test]
        public void SaveToFileTest()
        {
            const string fileName = "SaveToFileTest.txt";
            if (File.Exists(fileName))
                File.Delete(fileName);

            const string strData = "Hello";
            byte[] byteData = Encoding.UTF8.GetBytes(strData);
            var ms = new MemoryStream(byteData);
            ms.SaveToFile(fileName);

            using (var sr = new StreamReader(fileName))
            {
                string strData2 = sr.ReadToEnd();
                Assert.AreEqual(strData, strData2);
            }
        }
    }
}
