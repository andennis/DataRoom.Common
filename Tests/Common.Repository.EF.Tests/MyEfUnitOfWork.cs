namespace Common.Repository.EF.Tests
{
    public class MyEfUnitOfWork : UnitOfWork
    {
        public MyEfUnitOfWork(DbContextBase dbContext)
            : base(dbContext)
        {
        }
    }
}
