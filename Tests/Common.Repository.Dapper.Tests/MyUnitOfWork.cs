namespace Common.Repository.Dapper.Tests
{
    public class MyUnitOfWork : UnitOfWork
    {
        private MyRepository _myRepository;
        public MyRepository MyRepository => _myRepository ?? (_myRepository = new MyRepository(new MyDbContext("")));

        public MyUnitOfWork(DbContextBase dbContext) 
            : base(dbContext)
        {
            RegisterCustomRepository(() => MyRepository);
        }
    }
}
