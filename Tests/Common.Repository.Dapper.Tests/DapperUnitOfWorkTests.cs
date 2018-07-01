using System;
using NUnit.Framework;

namespace Common.Repository.Dapper.Tests
{
    [TestFixture]
    public class DapperUnitOfWorkTests
    {
        [Test]
        public void DbContext_cannot_be_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var v = new MyDapperUnitOfWork(null);
            });
        }
    }

}
