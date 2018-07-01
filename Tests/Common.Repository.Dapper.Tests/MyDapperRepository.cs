using Common.Repository.Tests.Entities;

namespace Common.Repository.Dapper.Tests
{
    public class MyDapperRepository : Repository<TestEntity2>
    {
        public MyDapperRepository(DbContextBase dbContext) : base(dbContext)
        {
        }
    }
}
