using NUnit.Framework;

namespace Common.Utils.Tests
{
    [TestFixture]
    public class CryptoTests
    {
        [Test]
        public void EncryptDecryptStringTest()
        {
            string key = "1234567812345678";
            string iv = "8765432187654321";
            const string textToEncrypt = "Hello Aes!!!123678";

            string encrypted = Crypto.EncryptString(textToEncrypt, key, iv);
            Assert.NotNull(encrypted);
            Assert.IsNotEmpty(encrypted);
            Assert.AreNotEqual(textToEncrypt, encrypted);

            string decrypted = Crypto.DecryptString(encrypted, key, iv);
            Assert.NotNull(decrypted);
            Assert.IsNotEmpty(decrypted);
            Assert.AreEqual(textToEncrypt, decrypted);

            key = "1111111111111111"; //wrong key
            decrypted = Crypto.DecryptString(encrypted, key, iv);
            Assert.IsNull(decrypted);

            key = "1234567812345678";
            iv = "2222222222222222"; //wrong vector
            decrypted = Crypto.DecryptString(encrypted, key, iv);
            Assert.AreNotEqual(textToEncrypt, decrypted);
        }

        [Test]
        public void CalculateHashTest()
        {
            var data = new byte[]{1,2,3,4,5};
            string hashCode = Crypto.CalculateHash(data);
            Assert.AreEqual("11966ab9c099f8fabefac54c08d5be2bd8c903af", hashCode);
        }
    }
}
