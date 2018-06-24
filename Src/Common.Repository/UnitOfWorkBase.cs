using System;
using System.Collections.Generic;

namespace Common.Repository
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        protected readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();

        protected void RegisterCustomRepository<TEntity>(Func<IRepository<TEntity>> fncRepository) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (_repositories.ContainsKey(entityType))
                throw new Exception($"Repository has already been registered for the the entity type: {entityType.Name}");

            _repositories.Add(entityType, new Lazy<IRepository<TEntity>>(fncRepository));
        }

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

        protected abstract IRepository<TEntity> CreateDefaultRepository<TEntity>() where TEntity : class;

        public virtual void Save()
        {
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected abstract void Dispose(bool disposing);
        #endregion

    }
}
