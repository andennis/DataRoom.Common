using Microsoft.EntityFrameworkCore;

namespace Common.Repository.EF
{
    public abstract class DbContextBase : DbContext
    {
        protected readonly string _connectionString;

        protected DbContextBase(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public abstract string DbScheme { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                //.UseLazyLoadingProxies()
                .UseSqlServer(_connectionString);
        }
    }
}
