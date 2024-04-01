using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.TestHelper;

namespace Simple.Data.SqlCe40Test.SchemaTests
{
    [TestFixture]
    public class DatabaseSchemaTests : DatabaseSchemaTestsBase
    {
        private static readonly string DatabasePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8)),
            "TestDatabase.sdf");

        protected override Database GetDatabase()
        {
            return Database.OpenFile(DatabasePath);
        }

        [Test]
        public void TestTables()
        {
            ClassicAssert.AreEqual(1, Schema.Tables.Count(t => t.ActualName == "Users"));
        }

        [Test]
        public void TestColumns()
        {
            var table = Schema.FindTable("Users");
            ClassicAssert.AreEqual(1, table.Columns.Count(c => c.ActualName == "Id"));
        }

        [Test]
        public void TestPrimaryKey()
        {
            ClassicAssert.AreEqual("CustomerId", Schema.FindTable("Customers").PrimaryKey[0]);
        }

        [Test]
        public void TestForeignKey()
        {
            var foreignKey = Schema.FindTable("Orders").ForeignKeys.Single();
            ClassicAssert.AreEqual("Customers", foreignKey.MasterTable.Name);
            ClassicAssert.AreEqual("Orders", foreignKey.DetailTable.Name);
            ClassicAssert.AreEqual("CustomerId", foreignKey.Columns[0]);
            ClassicAssert.AreEqual("CustomerId", foreignKey.UniqueColumns[0]);
        }

        [Test]
        public void TestSingularResolution()
        {
            ClassicAssert.AreEqual("OrderItems",
                Schema.FindTable("OrderItem").ActualName);
        }

        [Test]
        public void TestShoutySingularResolution()
        {
            ClassicAssert.AreEqual("OrderItems",
                Schema.FindTable("ORDER_ITEM").ActualName);
        }

        [Test]
        public void TestShoutyPluralResolution()
        {
            ClassicAssert.AreEqual("OrderItems",
                Schema.FindTable("ORDER_ITEM").ActualName);
        }
    }
}
