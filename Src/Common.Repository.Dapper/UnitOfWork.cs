using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Repository.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        #region Dispose
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                //TODO dispose
            }
            _disposed = true;
        }
        #endregion
    }
}
