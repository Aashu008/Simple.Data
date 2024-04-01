using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.InMemoryTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    class NameResolutionTest
    {
        [Test]
        public void InsertAndFindByTableNameResolvesCorrectlyWithHomogenisedStringComparer()
        {
            var inMemoryAdapter = new InMemoryAdapter(new AdoCompatibleComparer());

            Database.UseMockAdapter(inMemoryAdapter);
            var db = Database.Open();

            db.CUSTOMER.Insert(ID: 1, NAME: "ACME");

            var actual = db.Customers.FindById(1);
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual("ACME", actual.Name);
        }

        [Test]
        public void UpdateTableNameResolvesCorrectlyWithHomogenisedStringComparer()
        {
            var inMemoryAdapter = new InMemoryAdapter(new AdoCompatibleComparer());

            Database.UseMockAdapter(inMemoryAdapter);
            var db = Database.Open();

            db.CUSTOMER.Insert(ID: 1, NAME: "ACME");

            db.Customers.UpdateById(Id: 1, Name: "ACME Inc.");
            var actual = db.Customers.FindById(1);
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual("ACME Inc.", actual.Name);
        }

        [Test]
        public void DeleteTableNameResolvesCorrectlyWithHomogenisedStringComparer()
        {
            var inMemoryAdapter = new InMemoryAdapter(new AdoCompatibleComparer());

            Database.UseMockAdapter(inMemoryAdapter);
            var db = Database.Open();

            db.CUSTOMER.Insert(ID: 1, NAME: "ACME");

            var actual = db.Customers.FindById(1);
            ClassicAssert.IsNotNull(actual);

            db.Customers.DeleteById(1);
            actual = db.Customers.FindById(1);
            ClassicAssert.IsNull(actual);
        }
    }
}
