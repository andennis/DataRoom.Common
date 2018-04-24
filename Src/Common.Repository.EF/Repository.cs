using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Repository.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly DbContextBase _dbContext;
        protected readonly IOperationContext _operationContext;

        public Repository(DbContextBase dbContext, IOperationContext operationContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
            _operationContext = operationContext;
        }

        protected string DbScheme => _dbContext.DbScheme;

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            UpdateNulltoDBNull(parameters);
            return _dbSet.FromSql(query, parameters);
        }
        public virtual IQueryable<T> SqlQuery<T>(string query, params object[] parameters)
        {
            UpdateNulltoDBNull(parameters);
            //RawSqlCommand cmd = _dbContext.Database.GetService<IRawSqlCommandBuilder>().Build(query, parameters);
            //RelationalDataReader dr = cmd.RelationalCommand.ExecuteReader(_dbContext.Database.GetService<IRelationalConnection>(),cmd.ParameterValues);
            throw new NotImplementedException();
        }

        private List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                _dbContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                        entities.Add(map(result));

                    return entities;
                }
            }
        }

        /*
        private IQueryable<T> SqlQuery(string query, params object[] parameters)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT * From Table1";
                _dbContext.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    return result.AsQueryable<T>();
                }
            }
        }
        */

        public virtual T SqlQueryScalar<T>(string query, params object[] parameters)
        {
            UpdateNulltoDBNull(parameters);
            RawSqlCommand cmd = _dbContext.Database.GetService<IRawSqlCommandBuilder>().Build(query, parameters);
            return (T)cmd.RelationalCommand.ExecuteScalar(_dbContext.Database.GetService<IRelationalConnection>(), cmd.ParameterValues);
        }

        public virtual IQueryable<TEntity> SqlQueryStoredProc(string spName, params object[] parameters)
        {
            string commandText = GetSqlCommandText(spName, parameters);
            return SqlQuery(commandText.TrimEnd(), parameters);
        }
        public virtual IQueryable<T> SqlQueryStoredProc<T>(string spName, params object[] parameters)
        {
            string commandText = GetSqlCommandText(spName, parameters);
            return SqlQuery<T>(commandText.TrimEnd(), parameters);
        }
        public virtual T SqlQueryScalarStoredProc<T>(string spName, params object[] parameters)
        {
            string commandText = GetSqlCommandText(spName, parameters);
            return SqlQueryScalar<T>(commandText.TrimEnd(), parameters);
        }

        public void ExecuteCommand(string commandText, params object[] parameters)
        {
            UpdateNulltoDBNull(parameters);
            _dbContext.Database.ExecuteSqlCommand(commandText, parameters);
        }

        public void ExecuteNonQueryStoredProc(string spName, params object[] parameters)
        {
            string commandText = GetSqlCommandText(spName, parameters);
            ExecuteCommand(commandText.TrimEnd(), parameters);
        }

        private void UpdateNulltoDBNull(IEnumerable<object> parameters)
        {
            foreach (IDbDataParameter prm in parameters.Cast<IDbDataParameter>())
            {
                if (prm.Value == null)
                    prm.Value = DBNull.Value;
            }
        }

        protected string GetSqlCommandText(string spName, params object[] parameters)
        {
            if (parameters == null || !parameters.Any())
                return spName;

            string prmNames = string.Join(",", parameters.Cast<IDbDataParameter>().Select(x => string.Format("@{0}=@{0}" + (x.Direction == ParameterDirection.Output ? " OUTPUT" : string.Empty), x.ParameterName)));
            return spName + (prmNames != string.Empty ? " " + prmNames : string.Empty);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
        }
        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbSet.Remove(entity);
        }

        public virtual IRepositoryQuery<TEntity> Query()
        {
            return new RepositoryQuery<TEntity>(this);
        }

        internal IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;
            
            includeProperties?.ForEach(i => query = query.Include(i));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
            {
                query = query
                    .Skip(page.Value)
                    .Take(pageSize.Value);
            }
            return query;
        }

        /// <summary>
        /// It executes the SP with default name pattern: 
        ///     [Db scheme].[TEntity type name]_GetView
        /// The SP should take the parameter: @ID INT
        /// </summary>
        /// <typeparam name="TEntityView"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual TEntityView GetView<TEntityView>(int entityId) where TEntityView : class
        {
            return SqlQueryStoredProc<TEntityView>(SpNameGetView, new SqlParameter("ID", entityId)).FirstOrDefault();
        }

        /// <summary>
        /// It executes the SP with default name pattern: 
        ///     [Db scheme].[TEntity type name]_Search
        /// 
        /// The minimal set of parameters:
        /// ------------------------------
        ///     @PageIndex INT,
        ///     @PageSize INT,
        ///     @SortBy VARCHAR(64),
        ///     @SortDirection INT,
        ///     @TotalRecords INT OUTPUT,
        ///     @SearchText NVARCHAR(MAX)
        /// ------------------------------
        /// </summary>
        /// <typeparam name="TEntityView"></typeparam>
        /// <param name="searchParams"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntityView> Search<TEntityView>(IEnumerable<QueryParameter> searchParams, out int totalRecords)
        {
            IList<object> sqlPrms = searchParams?.Select(x => (object)new SqlParameter(x.Name, x.Value)).ToList();

            var prm = new SqlParameter("TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };
            if (sqlPrms != null)
                sqlPrms.Add(prm);
            else
                sqlPrms = new List<object>() { prm };

            IEnumerable<TEntityView> result = SqlQueryStoredProc<TEntityView>(SpNameSearch, sqlPrms.ToArray()).ToList();
            totalRecords = Convert.ToInt32(prm.Value);
            return result;
        }

        protected object NullIf<T>(T val, T equalToVal) where T : struct
        {
            return !val.Equals(equalToVal) ? val : (object)DBNull.Value;
        }

        protected virtual string SpNameGetView => $"{DbScheme}.{typeof(TEntity).Name}_GetView";
        protected virtual string SpNameSearch => $"{DbScheme}.{typeof(TEntity).Name}_Search";
    }
}