namespace Common.Repository.Dapper.Tests
{
    public class MyDapperUnitOfWork : UnitOfWork
    {
        private MyDapperRepository _myDapperRepository;
        public MyDapperRepository MyDapperRepository => _myDapperRepository ?? (_myDapperRepository = new MyDapperRepository(_dbContext));

        public MyDapperUnitOfWork(DbContextBase dbContext)
            : base(dbContext)
        {
            RegisterCustomRepository(() => MyDapperRepository);
        }
    }
}
