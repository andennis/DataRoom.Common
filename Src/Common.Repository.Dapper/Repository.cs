using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Repository.Dapper
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IRepositoryQuery<TEntity> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SqlQuery<T>(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public T SqlQueryScalar<T>(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> SqlQueryStoredProc(string spName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SqlQueryStoredProc<T>(string spName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public T SqlQueryScalarStoredProc<T>(string spName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCommand(string commandText, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNonQueryStoredProc(string spName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public TEntityView GetView<TEntityView>(int entityId) where TEntityView : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntityView> Search<TEntityView>(IEnumerable<QueryParameter> searchParams, out int totalRecords)
        {
            throw new NotImplementedException();
        }
    }
}
