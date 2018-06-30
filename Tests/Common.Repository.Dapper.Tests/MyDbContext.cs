using System.Data;
using System.Data.SqlClient;

namespace Common.Repository.Dapper.Tests
{
    public class MyDbContext : DbContextBase
    {
        public MyDbContext(string dbConnectionString)
            :base(dbConnectionString)
        {
        }

        public override string DbScheme => "cmntst";

        protected override IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
