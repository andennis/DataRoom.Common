using Common.Repository.Tests.Entities;

namespace Common.Repository.EF.Tests
{
    public class MyEfRepository : Repository<TestEntity2>
    {
        public MyEfRepository(DbContextBase dbContext) 
            : base(dbContext, null)
        {
        }
    }
}
