using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.Repository.Tests;
using Common.Repository.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Common.Repository.EF.Tests
{
    [TestFixture]
    public class EfRepositoryTests
    {
        [OneTimeSetUp]
        public void InitAllTests()
        {
            CleanUpDb();
        }

        [Test]
        public void CrudOperationsTest()
        {
            //Insert
            var entity1 = new TestEntity1() { Name = "N1", Value = "V1" };
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.Insert(entity1);
                uow.Save();
                Assert.Greater(entity1.MyId, 0);
            }
            long entityId = entity1.MyId;

            //Get
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                TestEntity1 entity = repo.Find(entityId);
                Assert.IsNotNull(entity);
                Assert.AreEqual(entityId, entity.MyId);
                Assert.AreEqual(entity1.Name, entity.Name);
                Assert.AreEqual(entity1.Value, entity.Value);
            }

            //Update
            var entity2 = new TestEntity1() { MyId = entityId, Name = "N2", Value = "V2" };
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.Update(entity2);
                uow.Save();
            }
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                TestEntity1 entity = repo.Find(entityId);
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity2.Name, entity.Name);
                Assert.AreEqual(entity2.Value, entity.Value);
            }

            //Delete
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.Delete(entityId);
                uow.Save();
                TestEntity1 entity = repo.Find(entityId);
                Assert.IsNull(entity);

                //delete nonexistent record
                repo.Delete(entityId);
            }

        }

        [Test]
        public void SqlQueryTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var entity1 = new TestEntity1() { Name = "NQuery1", Value = "VQuery1" };
                repo.Insert(entity1);
                uow.Save();

                IQueryable<TestEntity1> result = repo.SqlQuery("select * from cmntst.TestEntity1 where MyId=@id", new SqlParameter("Id", entity1.MyId));
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
                repo.Insert(entity1);
                uow.Save();

                IQueryable<TestEntity1> result = repo.SqlQuery<TestEntity1>("select * from cmntst.TestEntity1 where MyId=@id", new SqlParameter("Id", entity1.MyId));
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
                repo.Insert(entity1);
                uow.Save();

                var result = repo.SqlQueryScalar<long>("select cast(count(*) as bigint) from cmntst.TestEntity1 where MyId=@id", new SqlParameter("Id", entity1.MyId));
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
                repo.Insert(entity1);
                uow.Save();

                var result = repo.SqlQueryStoredProc("cmntst.Get_TestEntity1", new SqlParameter("Id", entity1.MyId));
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
                repo.Insert(entity1);
                uow.Save();

                var result = repo.SqlQueryStoredProc<TestEntity1>("cmntst.Get_TestEntity1", new SqlParameter("Id", entity1.MyId));
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
                repo.Insert(entity1);
                uow.Save();

                var result = repo.SqlQueryScalarStoredProc<long>("cmntst.GetTestEntity1Count", new SqlParameter("Id", entity1.MyId));
                Assert.AreEqual(1, result);
            }
        }

        [Test]
        public void ExecuteCommandTest()
        {
            var entity1 = new TestEntity1() { Name = "NExecuteCommand1", Value = "VExecuteCommand1" };
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.Insert(entity1);
                uow.Save();
            }
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();
                repo.ExecuteCommand("delete from cmntst.TestEntity1 where MyId=@id", new SqlParameter("id", entity1.MyId));

                TestEntity1 entity = repo.Find(entity1.MyId);
                Assert.IsNull(entity);
            }
        }

        [Test]
        public void ExecuteNonQueryStoredProcTest()
        {
            using (var uow = CreateUnitOfWork())
            {
                IRepository<TestEntity1> repo = uow.GetRepository<TestEntity1>();

                var prmId = new SqlParameter("Id", 0) {Direction = ParameterDirection.Output};
                var prmName = new SqlParameter("Name", "NExecuteNonQueryStoredProc1");
                var prmValue = new SqlParameter("Value", "VExecuteNonQueryStoredProc1") ;
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
                repo.Insert(entity1);
                uow.Save();

                var eview = repo.GetView<TestEntity1>(entity1.MyId);
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
                uow.Save();

                var prm = new QueryParameter("Name", entity1.Name);
                IEnumerable<TestEntity1> result = repo.Search<TestEntity1>(new List<QueryParameter>(){ prm }, out long totalRecords);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(entity1.Name, result.First().Name);
                Assert.AreEqual(entity1.Value, result.First().Value);
                Assert.AreEqual(1, totalRecords);
            }
        }

        private MyEfUnitOfWork CreateUnitOfWork()
        {
            var cfg = new TestAppConfig();
            var dbContext = new MyEfDbContext(cfg.GetConnectionString("CommonConnection"));
            return new MyEfUnitOfWork(dbContext);
        }

        private void CleanUpDb()
        {
            var cfg = new TestAppConfig();
            using (var dbContext = new MyEfDbContext(cfg.GetConnectionString("CommonConnection")))
            {
                dbContext.Database.ExecuteSqlCommand("delete from cmntst.TestEntity1");
            }
        }

    }

}
