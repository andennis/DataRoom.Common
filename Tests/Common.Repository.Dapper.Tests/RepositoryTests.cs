using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using NUnit.Framework;

namespace Common.Repository.Dapper.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [OneTimeSetUp]
        public void InitAllTests()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new DprTestEntity1Map());
                config.ForDommel();
            });
        }

        [Test]
        public void CrudOperationsTest()
        {
            //Insert
            long entityId;
            var entity1 = new DprTestEntity1() { Name = "N1", Value = "V1" };
            using (var uow = CreateUnitOfWork())
            {
                IRepository<DprTestEntity1> repo = uow.GetRepository<DprTestEntity1>();
                object id = repo.Insert(entity1);
                Assert.IsNotNull(id);
                Assert.IsAssignableFrom<long>(id);
                entityId = (long) id;
                Assert.Greater(entityId, 0);
            }

            //Get
            using (var uow = CreateUnitOfWork())
            {
                IRepository<DprTestEntity1> repo = uow.GetRepository<DprTestEntity1>();
                DprTestEntity1 entity2 = repo.Find(entityId);
                Assert.IsNotNull(entity2);
                Assert.AreEqual(entityId, entity2.MyId);
                Assert.AreEqual(entity1.Name, entity2.Name);
                Assert.AreEqual(entity1.Value, entity2.Value);
            }

            //Update
            using (var uow = CreateUnitOfWork())
            {
                IRepository<DprTestEntity1> repo = uow.GetRepository<DprTestEntity1>();
                var entity2 = new DprTestEntity1(){ MyId = entityId, Name = "N2", Value = "V2" };
                repo.Update(entity2);

                DprTestEntity1 entity3 = repo.Find(entityId);
                Assert.IsNotNull(entity3);
                Assert.AreEqual(entity2.Name, entity3.Name);
                Assert.AreEqual(entity2.Value, entity3.Value);
            }

            //Delete
            using (var uow = CreateUnitOfWork())
            {
                IRepository<DprTestEntity1> repo = uow.GetRepository<DprTestEntity1>();
                repo.Delete(entityId);
                DprTestEntity1 entity2 = repo.Find(entityId);
                Assert.IsNull(entity2);

                //delete nonexistent record
                repo.Delete(entityId);
            }

        }


        private MyUnitOfWork CreateUnitOfWork()
        {
            var cfg = new TestAppConfig();
            var dbContext = new MyDbContext(cfg.GetConnectionString("CommonConnection"));
            return new MyUnitOfWork(dbContext);
        }

    }

}
