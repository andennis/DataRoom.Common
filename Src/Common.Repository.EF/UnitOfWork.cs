using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.Repository.EF
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContextBase _dbContext;
        protected readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();
        protected readonly IOperationContext _operationContext;

        protected UnitOfWork(DbContextBase dbContext)
            : this(dbContext, null)
        {
        }

        protected UnitOfWork(DbContextBase dbContext, IOperationContext operationContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
            _operationContext = operationContext;
        }

        protected void RegisterCustomRepository<TEntity>(Func<IRepository<TEntity>> fncRepository) where TEntity : class
        {
            Type entityType = typeof (TEntity);
            if (_repositories.ContainsKey(entityType))
                throw new Exception($"Repository has already been registered for the the entity type: {entityType.Name}");

            _repositories.Add(entityType, new Lazy<IRepository<TEntity>>(fncRepository));
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
                _dbContext.Dispose();
            }
            _disposed = true;
        }
        #endregion

        public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            Type entityType = typeof(TEntity);

            if (_repositories.TryGetValue(entityType, out object repository))
            {
                var lazyRep = repository as Lazy<IRepository<TEntity>>;
                if (lazyRep != null)
                    return lazyRep.Value;

                return (IRepository<TEntity>)repository;
            }

            IRepository<TEntity> rep = CreateDefaultRepository<TEntity>();
            _repositories.Add(entityType, rep);
            return rep;
        }

        protected virtual IRepository<TEntity> CreateDefaultRepository<TEntity>() where TEntity : class
        {
            Type repositoryType = typeof(Repository<>);
            return (IRepository<TEntity>)Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext, _operationContext);
        }

        public void Save()
        {
            foreach (EntityEntry dbEntityEntry in _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                //IEntityVersionable
                var entityVersionable = dbEntityEntry.Entity as IEntityVersionable;
                if (entityVersionable != null)
                {
                    entityVersionable.RowVersion = 1;
                    entityVersionable.CreatedDate = DateTime.Now;
                    entityVersionable.UpdatedDate = entityVersionable.CreatedDate;
                }
            }
            foreach (EntityEntry dbEntityEntry in _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                var entityVersionable = dbEntityEntry.Entity as IEntityVersionable;
                if (entityVersionable != null)
                {
                    entityVersionable.RowVersion++;
                    entityVersionable.UpdatedDate = DateTime.Now;
                }
            }

            _dbContext.SaveChanges();
            /*
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    //TODO Console message should be raplaced by logger
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            */
        }


        /*
        public IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            return _dbContext.GetValidationErrors();
        }
        */
    }
}