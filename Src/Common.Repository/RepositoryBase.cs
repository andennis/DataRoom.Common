namespace Common.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity : class
    {
        protected abstract string DbScheme { get; }

        protected virtual string SpNameGetView => $"{DbScheme}.{typeof(TEntity).Name}_GetView";
        protected virtual string SpNameSearch => $"{DbScheme}.{typeof(TEntity).Name}_Search";
    }
}
