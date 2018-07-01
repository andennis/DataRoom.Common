using System;
using NUnit.Framework;

namespace Common.Repository.EF.Tests
{
    [TestFixture]
    public class EfUnitOfWorkTests
    {
        [Test]
        public void DbContext_cannot_be_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var v = new MyEfUnitOfWork(null);
            });
        }
    }

}
