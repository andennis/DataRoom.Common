namespace Common.Repository
{
    public interface IOperationContext
    {
        int? UserId { get; }
        int? SiteId { get; }
    }
}
