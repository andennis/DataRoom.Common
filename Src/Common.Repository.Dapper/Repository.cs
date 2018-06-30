using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using Dommel;

namespace Common.Repository.Dapper
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContextBase _dbContext;

        static Repository()
        {
            DommelMapper.AddSqlBuilder(typeof(SqlConnection), new DommelSqlServerSqlBuilder());
        }

        public Repository(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public object Insert(TEntity entity)
        {
            return _dbContext.DbConnection.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.DbConnection.Update(entity);
        }

        public void Delete(object id)
        {
            var entity = _dbContext.DbConnection.Get<TEntity>(id);
            if (entity != null)
                _dbContext.DbConnection.Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.DbConnection.Delete(entity);
        }

        public TEntity Find(params object[] keyValues)
        {
            if (keyValues.Length > 1)
                throw new CommonRepositoryException("Сomposite key is not supported");

            return _dbContext.DbConnection.Get<TEntity>(keyValues[0]);
        }

        public IRepositoryQuery<TEntity> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return SqlQuery<TEntity>(query, parameters);
        }

        public IQueryable<T> SqlQuery<T>(string query, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            return _dbContext.DbConnection.Query<T>(query, prms).AsQueryable();
        }

        public T SqlQueryScalar<T>(string query, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            return _dbContext.DbConnection.ExecuteScalar<T>(query, prms);
        }

        public IQueryable<TEntity> SqlQueryStoredProc(string spName, params object[] parameters)
        {
            return SqlQueryStoredProc<TEntity>(spName, parameters);
        }

        public IQueryable<T> SqlQueryStoredProc<T>(string spName, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            return _dbContext.DbConnection.Query<T>(spName, prms, commandType: CommandType.StoredProcedure).AsQueryable();
        }

        public T SqlQueryScalarStoredProc<T>(string spName, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            return _dbContext.DbConnection.ExecuteScalar<T>(spName, prms, commandType: CommandType.StoredProcedure);
        }

        public void ExecuteCommand(string commandText, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            _dbContext.DbConnection.Execute(commandText, prms);
        }

        public void ExecuteNonQueryStoredProc(string spName, params object[] parameters)
        {
            DynamicParameters prms = PrepareParameters(parameters);
            _dbContext.DbConnection.Execute(spName, prms, commandType: CommandType.StoredProcedure);
        }

        public TEntityView GetView<TEntityView>(int entityId) where TEntityView : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntityView> Search<TEntityView>(IEnumerable<QueryParameter> searchParams, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        internal IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        private DynamicParameters PrepareParameters(object[] parameters)
        {
            var prms = new DynamicParameters();
            foreach (var prm in parameters.Cast<IDbDataParameter>())
                prms.Add(prm.ParameterName, prm.Value);

            return prms;
        }

    }
}
