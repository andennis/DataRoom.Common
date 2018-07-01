using Common.Repository.Tests.Entities;
using NUnit.Framework;

namespace Common.Repository.Tests
{
    [TestFixture]
    public class UnitOfWorkBaseTests
    {
        [Test]
        public void GetRepository_Returns_Default_Repository()
        {
            IUnitOfWork uow = CreateUnitOfWork();
            var repo = uow.GetRepository<TestEntity1>();
            Assert.IsNotNull(repo);
            Assert.IsInstanceOf<IRepository<TestEntity1>>(repo);
        }

        [Test]
        public void GetRepository_Returns_Specific_Repository()
        {
            IUnitOfWork uow = CreateUnitOfWork();
            var repo = uow.GetRepository<TestEntity2>();
            Assert.IsNotNull(repo);
            Assert.IsInstanceOf<IMyRepositoryFake>(repo);
        }

        private IUnitOfWork CreateUnitOfWork()
        {
            return new MyUnitOfWorkFake();
        }
    }

}
