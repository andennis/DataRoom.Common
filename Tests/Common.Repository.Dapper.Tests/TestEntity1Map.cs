using Common.Repository.Tests.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Common.Repository.Dapper.Tests
{
    public class TestEntity1Map : DommelEntityMap<TestEntity1>
    {
        public TestEntity1Map()
        {
            ToTable(nameof(TestEntity1), "cmntst");
            Map(x => x.MyId).IsKey().IsIdentity();
        }
    }
}
