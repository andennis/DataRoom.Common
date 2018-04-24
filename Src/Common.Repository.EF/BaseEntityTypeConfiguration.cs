using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Repository.EF
{
    public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        protected string _dbScheme;

        protected BaseEntityTypeConfiguration(string dbScheme)
        {
            _dbScheme = dbScheme;
        }

        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
