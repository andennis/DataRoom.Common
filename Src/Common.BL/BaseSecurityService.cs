using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Repository;

namespace Common.BL
{
    public abstract class BaseSecurityService<TEntity, TSearchFilter, TUnitOfWork> : BaseService<TEntity, TSearchFilter, TUnitOfWork>
        where TEntity : class, IEntityId, new()
        where TSearchFilter : SearchFilterBase
        where TUnitOfWork : IUnitOfWork

    {
        protected readonly IUserAccessService _userAccess;

        protected BaseSecurityService(TUnitOfWork unitOfWork, IOperationContext operationContext, IUserAccessService userAccess)
            :base(unitOfWork, operationContext)
        {
            _userAccess = userAccess;
        }

        protected abstract int EntityTypeId { get; }

        public override void Create(TEntity entity)
        {
            CheckUserAccess(PermissionType.Create);
            base.Create(entity);
        }
        public override void Update(TEntity entity)
        {
            CheckUserAccess(PermissionType.Edit, entity.EntityId);
            base.Update(entity);
        }
        public override void Delete(int entityId)
        {
            CheckUserAccess(PermissionType.Read, entityId);
            base.Delete(entityId);
        }
        public override void Delete(TEntity entity)
        {
            CheckUserAccess(PermissionType.Delete, entity.EntityId);
            base.Delete(entity);
        }
        public override TEntity Get(int entityId)
        {
            CheckUserAccess(PermissionType.Read, entityId);
            return base.Get(entityId);
        }
        public override TEntityView GetView<TEntityView>(int entityId)
        {
            CheckUserAccess(PermissionType.Read,  entityId);
            return base.GetView<TEntityView>(entityId);
        }

        protected override IEnumerable<QueryParameter> GetSearchViewParameters(SearchContext searchContext, TSearchFilter searchFilter)
        {
            IEnumerable<QueryParameter> queryParams = base.GetSearchViewParameters(searchContext, searchFilter);
            var ocPrms = new List<QueryParameter>
            {
                new QueryParameter() {Name = "UserId", Value = _operationContext.UserId}
            };
            if (_operationContext.SiteId.HasValue)
                ocPrms.Add(new QueryParameter() { Name = "SiteId", Value = _operationContext.SiteId });

            return queryParams.Union(ocPrms);
        }

        protected virtual void CheckUserAccess(PermissionType permissionType, int? entityId = null)
        {
            _userAccess.CheckUserAccess(EntityTypeId, permissionType, entityId);
        }
    }
}
