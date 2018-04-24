using System.Collections.Generic;
using System.Linq;

namespace Common.Repository
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);

        TEntity Find(params object[] keyValues);
        IRepositoryQuery<TEntity> Query();
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
        IQueryable<T> SqlQuery<T>(string query, params object[] parameters);
        T SqlQueryScalar<T>(string query, params object[] parameters);

        IQueryable<TEntity> SqlQueryStoredProc(string spName, params object[] parameters);
        IQueryable<T> SqlQueryStoredProc<T>(string spName, params object[] parameters);
        T SqlQueryScalarStoredProc<T>(string spName, params object[] parameters);

        void ExecuteCommand(string commandText, params object[] parameters);
        void ExecuteNonQueryStoredProc(string spName, params object[] parameters);

        TEntityView GetView<TEntityView>(int entityId) where TEntityView : class;
        IEnumerable<TEntityView> Search<TEntityView>(IEnumerable<QueryParameter> searchParams, out int totalRecords);

    }
}