using System.Security;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class SecureStringExtensionsTests
    {
        [Test]
        public void ConvertToSecureStringAndBackTest()
        {
            string str = null;
            Assert.IsNull(str.ConvertToSecureString());
            str = "123";
            SecureString secStr = str.ConvertToSecureString();
            Assert.IsNotNull(secStr);
            Assert.Greater(secStr.Length, 0);

            string str2 = secStr.ConvertToUnsecureString();
            Assert.AreEqual(str, str2);

            secStr = null;
            Assert.IsNull(secStr.ConvertToUnsecureString());
        }
    }
}
