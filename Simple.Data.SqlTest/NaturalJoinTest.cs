using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.SqlTest
{
    [TestFixture]
    public class NaturalJoinTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void CustomerDotOrdersDotOrderDateShouldReturnOneRow()
        {
            var db = DatabaseHelper.Open();
            var row = db.Customers.Find(db.Customers.Orders.OrderDate == new DateTime(2010, 10, 10));
            ClassicAssert.IsNotNull(row);
            ClassicAssert.AreEqual("Test", row.Name);
        }

        [Test]
        public void CustomerDotOrdersDotOrderItemsDotItemDotNameShouldReturnOneRow()
        {
            var db = DatabaseHelper.Open();
            var customer = db.Customers.Find(db.Customers.Orders.OrderItems.Item.Name == "Widget");
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual("Test", customer.Name);
            foreach (var order in customer.Orders)
            {
                ClassicAssert.AreEqual(1, order.OrderId);
            }
        }

        [Test]
        public void CustomerDotNameAndCustomerDotOrdersDotOrderItemsDotItemDotNameShouldReturnOneRow()
        {
            var db = DatabaseHelper.Open();
            var customer = db.Customers.Find(db.Customers.Name == "Test" &&
                                             db.Customers.Orders.OrderItems.Item.Name == "Widget");
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual("Test", customer.Name);
            foreach (var order in customer.Orders)
            {
                ClassicAssert.AreEqual(1, order.OrderId);
            }
        }

        [Test]
        public void CustomerDotNameAndCustomerDotOrdersDotOrderDateAndCustomerDotOrdersDotOrderItemsDotItemDotNameShouldReturnOneRow()
        {
            var db = DatabaseHelper.Open();
            var customer = db.Customers.Find(db.Customers.Name == "Test" &&
                                             db.Customers.Orders.OrderDate == new DateTime(2010, 10, 10) &&
                                             db.Customers.Orders.OrderItems.Item.Name == "Widget");
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual("Test", customer.Name);
            foreach (var order in customer.Orders)
            {
                ClassicAssert.AreEqual(1, order.OrderId);
            }
        }
    }
}
