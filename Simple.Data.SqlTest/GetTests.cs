using NUnit.Framework;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework.Legacy;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void TestGet()
        {
            var db = DatabaseHelper.Open();
            var user = db.Users.Get(1);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void GetWithNonExistentPrimaryKeyShouldReturnNull()
        {
            var db = DatabaseHelper.Open();
            var user = db.Users.Get(1138);
            ClassicAssert.IsNull(user);
        }

        [Test]
        public void SelectClauseWithGetScalarShouldLimitQuery()
        {
            var db = DatabaseHelper.Open();
            string actual = db.Customers.Select(db.Customers.Name).GetScalar(1);
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual("Test", actual);
        }

        [Test]
        public void WithClauseShouldCastToStaticTypeWithComplexProperty()
        {
            var db = DatabaseHelper.Open();
            Order actual = db.Orders.WithCustomer().Get(1);
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.IsNotNull(actual.Customer);
            ClassicAssert.AreEqual("Test", actual.Customer.Name);
            ClassicAssert.AreEqual("100 Road", actual.Customer.Address);
        }

        [Test]
        public void WithClauseShouldCastToStaticTypeWithCollection()
        {
            var db = DatabaseHelper.Open();
            Customer actual = db.Customers.WithOrders().Get(1);
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual(1, actual.Orders.Single().OrderId);
            ClassicAssert.AreEqual(new DateTime(2010, 10, 10), actual.Orders.Single().OrderDate);
        }
    }
}