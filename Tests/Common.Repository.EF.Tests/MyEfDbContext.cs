using Common.Repository.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.EF.Tests
{
    public class MyEfDbContext : DbContextBase
    {
        public MyEfDbContext(string dbConnectionString)
            :base(dbConnectionString)
        {
        }

        public override string DbScheme => "cmntst";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity1>().ToTable(nameof(TestEntity1), DbScheme);
            modelBuilder.Entity<TestEntity1>().HasKey(x => x.MyId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
