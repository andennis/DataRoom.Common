using System;

namespace Common.Repository.Dapper
{
    public abstract class UnitOfWork : UnitOfWorkBase
    {
        private readonly DbContextBase _dbContext;

        protected UnitOfWork(DbContextBase dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
        }

        protected override IRepository<TEntity> CreateDefaultRepository<TEntity>()
        {
            Type repositoryType = typeof(Repository<>);
            return (IRepository<TEntity>)Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
        }

        #region Dispose
        private bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }
        #endregion
    }
}
