namespace Common.Repository.Dapper.Tests
{
    public class MyRepository : Repository<DprTestEntity2>
    {
        public MyRepository(DbContextBase dbContext) : base(dbContext)
        {
        }
    }
}
