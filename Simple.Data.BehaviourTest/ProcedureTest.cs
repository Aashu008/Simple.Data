using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Simple.Data.Ado;
using Simple.Data.Mocking.Ado;

// ReSharper disable InconsistentNaming

namespace Simple.Data.IntegrationTest
{
    [TestFixture]
    public class ProcedureTest
    {
        static dynamic CreateDatabase(MockDatabase mockDatabase)
        {
            var mockSchemaProvider = new MockSchemaProvider();
            mockSchemaProvider.SetProcedures(new[] { "dbo", "ProcedureWithParameters" },
                new[] { "dbo", "ProcedureWithoutParameters" }, new[] { "foo", "ProcedureInAnotherSchema" });
            mockSchemaProvider.SetParameters(new[] { "dbo", "ProcedureWithParameters", "@One" },
                                          new[] { "dbo", "ProcedureWithParameters", "@Two" });
            return new Database(new AdoAdapter(new MockConnectionProvider(new MockDbConnection(mockDatabase), mockSchemaProvider)));
        }

        [Test]
        public void CheckMockWorking()
        {
            var mockDatabase = new MockDatabase();
            var db = CreateDatabase(mockDatabase);
            ClassicAssert.NotNull(db);
        }

        [Test]
        public void CallingMethodOnDatabase_Should_CallProcedure()
        {
            var mockDatabase = new MockDatabase();
            var db = CreateDatabase(mockDatabase);
            db.ProcedureWithoutParameters();
            ClassicAssert.IsNotNull(mockDatabase.Sql);
            ClassicAssert.AreEqual("[dbo].[ProcedureWithoutParameters]".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(0, mockDatabase.Parameters.Count());
        }

        [Test]
        public void CallingMethodOnDatabase_WithNamedParameters_Should_CallProcedure()
        {
            var mockDatabase = new MockDatabase();
            var db = CreateDatabase(mockDatabase);
            db.ProcedureWithParameters(One: 1, Two: 2);
            ClassicAssert.AreEqual(1, mockDatabase.CommandTexts.Count);
            ClassicAssert.AreEqual("[dbo].[ProcedureWithParameters]".ToLowerInvariant(), mockDatabase.CommandTexts[0].ToLowerInvariant());
            ClassicAssert.AreEqual(2, mockDatabase.CommandParameters[0].Count);
            ClassicAssert.IsTrue(mockDatabase.CommandParameters[0].ContainsKey("@One"));
            ClassicAssert.AreEqual(1, mockDatabase.CommandParameters[0]["@One"]);
            ClassicAssert.IsTrue(mockDatabase.CommandParameters[0].ContainsKey("@Two"));
            ClassicAssert.AreEqual(2, mockDatabase.CommandParameters[0]["@Two"]);
        }

        [Test]
        public void CallingMethodOnDatabase_WithPositionalParameters_Should_CallProcedure()
        {
            var mockDatabase = new MockDatabase();
            var db = CreateDatabase(mockDatabase);
            db.ProcedureWithParameters(1, 2);
            ClassicAssert.AreEqual(1, mockDatabase.CommandTexts.Count);
            ClassicAssert.AreEqual("[dbo].[ProcedureWithParameters]".ToLowerInvariant(), mockDatabase.CommandTexts[0].ToLowerInvariant());
            ClassicAssert.AreEqual(2, mockDatabase.CommandParameters[0].Count);
            ClassicAssert.IsTrue(mockDatabase.CommandParameters[0].ContainsKey("@One"));
            ClassicAssert.AreEqual(1, mockDatabase.CommandParameters[0]["@One"]);
            ClassicAssert.IsTrue(mockDatabase.CommandParameters[0].ContainsKey("@Two"));
            ClassicAssert.AreEqual(2, mockDatabase.CommandParameters[0]["@Two"]);
        }

        [Test]
        public void CallingMethodOnObjectInDatabase_Should_CallProcedure()
        {
            var mockDatabase = new MockDatabase();
            var db = CreateDatabase(mockDatabase);
            db.foo.ProcedureInAnotherSchema();
            ClassicAssert.IsNotNull(mockDatabase.Sql);
            ClassicAssert.AreEqual("[foo].[ProcedureInAnotherSchema]".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(0, mockDatabase.Parameters.Count());
        }
    }
}
