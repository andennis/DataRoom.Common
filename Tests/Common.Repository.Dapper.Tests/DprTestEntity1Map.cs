using Dapper.FluentMap.Dommel.Mapping;

namespace Common.Repository.Dapper.Tests
{
    public class DprTestEntity1Map : DommelEntityMap<DprTestEntity1>
    {
        public DprTestEntity1Map()
        {
            ToTable("DprTestEntity1", "cmntst");
            Map(x => x.MyId).IsKey().IsIdentity();
        }
    }
}
