using System;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void TruncateMilisecondsTest()
        {
            var dt1 = new DateTime(2014, 6, 2, 20, 33, 44, 55);
            DateTime dt2 = dt1.TruncateMiliseconds();
            Assert.AreEqual(dt1.AddMilliseconds(-55), dt2);
        }

        [Test]
        public void ToUnixTimeSecondsTest()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(0, dt.ToUnixTimeSeconds());
            Assert.AreEqual(DateTimeKind.Utc, dt.Kind);
        }

        [Test]
        public void UnixTimeSecondsToDateTimeTest()
        {
            const long seconds = 0;
            DateTime dt = seconds.UnixTimeSecondsToDateTime();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(unixEpoch, dt);
            Assert.AreEqual(DateTimeKind.Utc, dt.Kind);
        }
    }
}
