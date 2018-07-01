using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.Repository.Tests;
using Common.Repository.Tests.Entities;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using NUnit.Framework;

namespace Common.Repository.Dapper.Tests
{
    [TestFixture]
    public class DapperRepositoryTests
    {
        [OneTimeSetUp]
        public void InitAllTests()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TestEntity1Map());
                config.ForDommel();
            });

            CleanUpDb();
        }

        [Test]
        public void CrudOperationsTest()
        {
            //Insert
            long entityId;
            var entity1 = new TestEntity1() { Name = "N1", Value = "V1" };
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                object id = repo.Insert(entity1);
                Assert.IsNotNull(id);
                Assert.IsAssignableFrom<long>(id);
                entityId = (long) id;
                Assert.Greater(entityId, 0);
            }

            //Get
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                TestEntity1 entity2 = repo.Find(entityId);
                Assert.IsNotNull(entity2);
                Assert.AreEqual(entityId, entity2.MyId);
                Assert.AreEqual(entity1.Name, entity2.Name);
                Assert.AreEqual(entity1.Value, entity2.Value);
            }

            //Update
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                var entity2 = new TestEntity1(){ MyId = entityId, Name = "N2", Value = "V2" };
                repo.Update(entity2);

                TestEntity1 entity3 = repo.Find(entityId);
                Assert.IsNotNull(entity3);
                Assert.AreEqual(entity2.Name, entity3.Name);
                Assert.AreEqual(entity2.Value, entity3.Value);
            }

            //Delete
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.Delete(entityId);
                TestEntity1 entity2 = repo.Find(entityId);
                Assert.IsNull(entity2);

                //delete nonexistent record
                repo.Delete(entityId);
            }

        }

        [Test]
        public void Сomposite_Key_Not_Supported()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                Assert.Throws<CommonRepositoryException>(() => repo.Find(1, 2));
            }
        }

        [Test]
        public void SqlQueryTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NQuery1", Value = "VQuery1" };
                object id = repo.Insert(entity1);

                IQueryable<TestEntity1> result = repo.SqlQuery("select * from cmntst.TestEntity1 where MyId=@id", new QueryParameter("Id", id));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
            }
        }

        [Test]
        public void SqlQuery_Genegic_Test()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NQuery_Gnr_1", Value = "VQuery_Gnr_1" };
                object id = repo.Insert(entity1);

                IQueryable<TestEntity1> result = repo.SqlQuery<TestEntity1>("select * from cmntst.TestEntity1 where MyId=@id", new QueryParameter("Id", id));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
            }
        }

        [Test]
        public void SqlQueryScalarTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NQueryScalar1", Value = "VQueryScalar1" };
                object id = repo.Insert(entity1);

                var result = repo.SqlQueryScalar<long>("select count(*) from cmntst.TestEntity1 where MyId=@id", new QueryParameter("Id", id));
                Assert.AreEqual(1, result);
            }
        }

        [Test]
        public void SqlQueryStoredProcTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NSqlQueryStoredProc1", Value = "VSqlQueryStoredProc1" };
                object id = repo.Insert(entity1);

                var result = repo.SqlQueryStoredProc("cmntst.Get_TestEntity1", new QueryParameter("Id", id));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
            }
        }

        [Test]
        public void SqlQueryStoredProc_Generic_Test()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NSqlQueryStoredProc_Gnr_1", Value = "VSqlQueryStoredProc1_Gnr_" };
                object id = repo.Insert(entity1);

                var result = repo.SqlQueryStoredProc<TestEntity1>("cmntst.Get_TestEntity1", new QueryParameter("Id", id));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
            }
        }

        [Test]
        public void SqlQueryScalarStoredProcTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NSqlQueryScalarStoredProc1", Value = "VSqlQueryScalarStoredProc1" };
                object id = repo.Insert(entity1);

                var result = repo.SqlQueryScalarStoredProc<long>("cmntst.GetTestEntity1Count", new QueryParameter("Id", id));
                Assert.AreEqual(1, result);
            }
        }

        [Test]
        public void ExecuteCommandTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NExecuteCommand1", Value = "VExecuteCommand1" };
                object id = repo.Insert(entity1);

                repo.ExecuteCommand("delete from cmntst.TestEntity1 where MyId=@id", new QueryParameter("id", id));
                TestEntity1 entity = repo.Find(id);
                Assert.IsNull(entity);
            }
        }

        [Test]
        public void ExecuteNonQueryStoredProcTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var prmId = new QueryParameter("Id", 0) {Direction = ParameterDirection.Output};
                var prmName = new QueryParameter("Name", "NExecuteNonQueryStoredProc1");
                var prmValue = new QueryParameter("Value", "VExecuteNonQueryStoredProc1") ;
                repo.ExecuteNonQueryStoredProc("cmntst.Insert_TestEntity1", prmName, prmValue, prmId);

                Assert.IsNotNull(prmId.Value);
                TestEntity1 entity = repo.Find(prmId.Value);
                Assert.IsNotNull(entity);
                Assert.AreEqual(prmName.Value, entity.Name);
                Assert.AreEqual(prmValue.Value, entity.Value);
            }
        }

        [Test]
        public void GetViewTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NGetView1", Value = "VGetView1" };
                object id = repo.Insert(entity1);

                var eview = repo.GetView<TestEntity1>(id);
                Assert.IsNotNull(eview);
                Assert.AreEqual(entity1.Name, eview.Name);
                Assert.AreEqual(entity1.Value, eview.Value);
            }
        }

        [Test]
        public void SearchTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NSearch1", Value = "VSearch1" };
                repo.Insert(entity1);

                var prm = new QueryParameter("Name", entity1.Name);
                IEnumerable<TestEntity1> result = repo.Search<TestEntity1>(new List<QueryParameter>(){ prm }, out long totalRecords);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
                Assert.AreEqual(entity1.Value, result.First().Value);
                Assert.AreEqual(1, totalRecords);
            }
        }

        private MyDapperUnitOfWork CreateUnitOfWork()
        {
            var cfg = new TestAppConfig();
            var dbContext = new MyDapperDbContext(cfg.GetConnectionString("CommonConnection"));
            return new MyDapperUnitOfWork(dbContext);
        }

        private void CleanUpDb()
        {
            var cfg = new TestAppConfig();
            using (var cnn = new SqlConnection(cfg.GetConnectionString("CommonConnection")))
            {
                cnn.Execute("delete from cmntst.TestEntity1");
            }
        }

    }

}
