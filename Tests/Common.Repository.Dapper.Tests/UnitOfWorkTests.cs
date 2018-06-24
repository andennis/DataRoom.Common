using System;
using NUnit.Framework;

namespace Common.Repository.Dapper.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void DbContext_cannot_be_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var v = new MyUnitOfWork(null);
            });
        }

        [Test]
        public void GetRepository_Returns_Default_Repository()
        {
            IUnitOfWork uow = CreateUnitOfWork();
            var repo = uow.GetRepository<DprTestEntity1>();
            Assert.IsNotNull(repo);
            Assert.IsAssignableFrom<Repository<DprTestEntity1>>(repo);
        }

        [Test]
        public void GetRepository_Returns_Specific_Repository()
        {
            IUnitOfWork uow = CreateUnitOfWork();
            var repo = uow.GetRepository<DprTestEntity2>();
            Assert.IsNotNull(repo);
            Assert.IsAssignableFrom<MyRepository>(repo);
        }

        private IUnitOfWork CreateUnitOfWork()
        {
            return new MyUnitOfWork(new MyDbContext(""));
        }
    }

}
