namespace Common.BL
{
    public interface IBaseService<TEntity, TSearchFilter> 
        where TEntity : class
        where TSearchFilter : SearchFilterBase
    {
        void Create(TEntity entity);
        void Update(TEntity entity);

        void Delete(int entityId);
        void Delete(TEntity entity);

        TEntity Get(int entityId);
        SearchResult<TEntity> Search(SearchContext searchContext, TSearchFilter searchFilter);

        TEntityView GetView<TEntityView>(int entityId) where TEntityView : class;
        SearchResult<TEntityView> SearchView<TEntityView>(SearchContext searchContext, TSearchFilter searchFilter) where TEntityView : class;
    }
}
