using Moq;

namespace Common.Repository.Tests
{
    public class MyUnitOfWorkFake : UnitOfWorkBase
    {
        private IMyRepositoryFake _myRepository;
        public IMyRepositoryFake MyRepository => _myRepository ?? (_myRepository = new Mock<IMyRepositoryFake>().Object);

        public MyUnitOfWorkFake()
        {
            RegisterCustomRepository(() => MyRepository);
        }

        protected override IRepository<TEntity> CreateDefaultRepository<TEntity>()
        {
            return new Mock<IRepository<TEntity>>().Object;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
