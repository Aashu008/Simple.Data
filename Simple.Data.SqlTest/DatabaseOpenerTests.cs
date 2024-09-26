using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Simple.Data.SqlTest
{
    using Ado;
    using NUnit.Framework.Legacy;
    using SqlServer;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    public class DatabaseOpenerTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var testDllName = Assembly.GetAssembly(GetType())
                                      .GetName()
                                      .Name;
            var configName = testDllName + ".dll.config";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configName);
            DatabaseHelper.Reset();
        }


        [Test]
        public void OpenNamedConnectionTest()
        {
            if (Environment.GetEnvironmentVariable("SIMPLETESTDB") != null)
            {
                Assert.Ignore();
                return;
            }
            var db = Database.OpenNamedConnection("Test");
            ClassicAssert.IsNotNull(db);
            var user = db.Users.FindById(1);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void TestProviderIsSqlProvider()
        {
            var provider = new ProviderHelper().GetProviderByConnectionString(DatabaseHelper.ConnectionString);
            ClassicAssert.IsInstanceOf(typeof(SqlConnectionProvider), provider);
        }

        [Test]
        public void TestProviderIsSqlProviderFromOpen()
        {
            Database db = DatabaseHelper.Open();
            ClassicAssert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
            ClassicAssert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
        }

        [Test]
        public void TestProviderIsSqlProviderFromOpenConnection()
        {
            Database db = Database.OpenConnection(DatabaseHelper.ConnectionString);
            ClassicAssert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
            ClassicAssert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
        }
    }
}
