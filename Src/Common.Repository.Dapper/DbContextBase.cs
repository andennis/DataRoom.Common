using System;
using System.Data;

namespace Common.Repository.Dapper
{
    public abstract class DbContextBase : IDisposable
    {
        private readonly string _connectionString;

        private IDbConnection _dbConnection;
        public IDbConnection DbConnection => _dbConnection ?? (_dbConnection = CreateDbConnection(_connectionString));

        protected DbContextBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected abstract IDbConnection CreateDbConnection(string connectionString);

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbConnection?.Close();
            }
            _disposed = true;
        }
        #endregion

    }
}
