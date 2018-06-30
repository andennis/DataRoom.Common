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
    public class Repository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity> where TEntity : class
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

        protected override string DbScheme => _dbContext.DbScheme;

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
            DynamicParameters prms = CreateDapperParameters(parameters);
            return _dbContext.DbConnection.Query<T>(query, prms).AsQueryable();
        }

        public T SqlQueryScalar<T>(string query, params object[] parameters)
        {
            DynamicParameters prms = CreateDapperParameters(parameters);
            return _dbContext.DbConnection.ExecuteScalar<T>(query, prms);
        }

        public IQueryable<TEntity> SqlQueryStoredProc(string spName, params object[] parameters)
        {
            return SqlQueryStoredProc<TEntity>(spName, parameters);
        }

        public IQueryable<T> SqlQueryStoredProc<T>(string spName, params object[] parameters)
        {
            DynamicParameters dprPrms = CreateDapperParameters(parameters);
            IQueryable<T> result = _dbContext.DbConnection.Query<T>(spName, dprPrms, commandType: CommandType.StoredProcedure).AsQueryable();
            MapOutputParameters(dprPrms, parameters);
            return result;
        }

        public T SqlQueryScalarStoredProc<T>(string spName, params object[] parameters)
        {
            DynamicParameters prms = CreateDapperParameters(parameters);
            return _dbContext.DbConnection.ExecuteScalar<T>(spName, prms, commandType: CommandType.StoredProcedure);
        }

        public void ExecuteCommand(string commandText, params object[] parameters)
        {
            DynamicParameters prms = CreateDapperParameters(parameters);
            _dbContext.DbConnection.Execute(commandText, prms);
        }

        public void ExecuteNonQueryStoredProc(string spName, params object[] parameters)
        {
            DynamicParameters dprPrms = CreateDapperParameters(parameters);
            _dbContext.DbConnection.Execute(spName, dprPrms, commandType: CommandType.StoredProcedure);
            MapOutputParameters(dprPrms, parameters);
        }

        public TEntityView GetView<TEntityView>(object entityId) where TEntityView : class
        {
            return SqlQueryStoredProc<TEntityView>(SpNameGetView, new QueryParameter("ID", entityId)).FirstOrDefault();
        }

        public IEnumerable<TEntityView> Search<TEntityView>(IEnumerable<QueryParameter> searchParams, out long totalRecords)
        {
            IList<object> sqlPrms = searchParams?.Select(x => (object)x).ToList();

            var prm = new QueryParameter("TotalRecords", 0) { Direction = ParameterDirection.Output };
            if (sqlPrms != null)
                sqlPrms.Add(prm);
            else
                sqlPrms = new List<object>() { prm };

            IEnumerable<TEntityView> result = SqlQueryStoredProc<TEntityView>(SpNameSearch, sqlPrms.ToArray()).ToList();
            totalRecords = Convert.ToInt64(prm.Value);
            return result;
        }

        internal IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        private DynamicParameters CreateDapperParameters(object[] parameters)
        {
            var prms = new DynamicParameters();
            foreach (var prm in parameters.OfType<IDbDataParameter>())
                prms.Add(prm.ParameterName, prm.Value, direction: Enum.IsDefined(typeof(ParameterDirection), prm.Direction) ? prm.Direction : ParameterDirection.Input);
            foreach (var prm in parameters.OfType<QueryParameter>())
                prms.Add(prm.Name, prm.Value, direction:prm.Direction);

            return prms;
        }

        private void MapOutputParameters(DynamicParameters dprPrms, object[] parameters)
        {
            foreach (var queryPrm in parameters.OfType<IDbDataParameter>().Where(x => x.Direction == ParameterDirection.Output))
                queryPrm.Value = dprPrms.Get<object>(queryPrm.ParameterName);

            foreach (var queryPrm in parameters.OfType<QueryParameter>().Where(x => x.Direction == ParameterDirection.Output))
                queryPrm.Value = dprPrms.Get<object>(queryPrm.Name);
        }

    }
}
