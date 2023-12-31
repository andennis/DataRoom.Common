﻿using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Common.Repository.Dapper.Tests
{
    [TestFixture]
    public class DapperDbContextTests
    {
        [Test]
        public void Dispose_Test()
        {
            var context = new MyDapperDbContext("");
            Assert.IsInstanceOf<IDisposable>(context);
            var conn = context.DbConnection;
            context.Dispose();
            context.Dispose();
        }

        [Test]
        public void DbConnection_Is_SqlConnection()
        {
            var context = new MyDapperDbContext("");
            IDbConnection conn = context.DbConnection;
            Assert.IsNotNull(conn);
            Assert.IsAssignableFrom<SqlConnection>(conn);
        }
    }
}
