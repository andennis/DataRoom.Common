using System;

namespace Common.Repository
{
    public abstract class RepositoryTestBase<TUnitOfWork>
        where TUnitOfWork : class, IUnitOfWork
    {
        protected abstract TUnitOfWork CreateUnitOfWork();

        protected TEntity CreateEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (TUnitOfWork unitOfWork = CreateUnitOfWork())
            {
                var entityRep = unitOfWork.GetRepository<TEntity>();
                entityRep.Insert(entity);
                unitOfWork.Save();
                return entity;
            }
        }

        protected void UpdateEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (TUnitOfWork unitOfWork = CreateUnitOfWork())
            {
                var entityRep = unitOfWork.GetRepository<TEntity>();
                entityRep.Update(entity);
                unitOfWork.Save();
            }
        }

        protected void DeleteEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (TUnitOfWork unitOfWork = CreateUnitOfWork())
            {
                var entityRep = unitOfWork.GetRepository<TEntity>();
                entityRep.Delete(entity);
                unitOfWork.Save();
            }
        }

        protected TEntity ReadEntity<TEntity>(params object[] keys) where TEntity : class, new()
        {
            using (TUnitOfWork unitOfWork = CreateUnitOfWork())
            {
                var entityRep = unitOfWork.GetRepository<TEntity>();
                return entityRep.Find(keys);
            }
        }

        protected string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
