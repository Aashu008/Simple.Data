using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    class SchemaQualifiedTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void TestFindAllByIdWithSchemaQualification()
        {
            var db = DatabaseHelper.Open();
            var dboCount = db.dbo.SchemaTable.FindAllById(1).ToList().Count;
            var testCount = db.test.SchemaTable.FindAllById(1).ToList().Count;
            ClassicAssert.AreEqual(1, dboCount);
            ClassicAssert.AreEqual(0, testCount);
        }

        [Test]
        public void TestFindWithSchemaQualification()
        {
            var db = DatabaseHelper.Open();

            var dboActual = db.dbo.SchemaTable.FindById(1);
            var testActual = db.test.SchemaTable.FindById(1);

            ClassicAssert.IsNotNull(dboActual);
            ClassicAssert.AreEqual("Pass", dboActual.Description);
            ClassicAssert.IsNull(testActual);
        }

        [Test]
        public void QueryWithSchemaQualifiedTableName()
        {
            var db = DatabaseHelper.Open();
            var result = db.test.SchemaTable.QueryById(2)
                           .Select(db.test.SchemaTable.Id,
                                   db.test.SchemaTable.Description)
                           .Single();
            ClassicAssert.AreEqual(2, result.Id);
            ClassicAssert.AreEqual("Pass", result.Description);
        }

        [Test]
        public void QueryWithSchemaQualifiedTableNameAndAliases()
        {
            var db = DatabaseHelper.Open();
            var result = db.test.SchemaTable.QueryById(2)
                           .Select(db.test.SchemaTable.Id.As("This"),
                                   db.test.SchemaTable.Description.As("That"))
                           .Single();
            ClassicAssert.AreEqual(2, result.This);
            ClassicAssert.AreEqual("Pass", result.That);
        }

    }
}
