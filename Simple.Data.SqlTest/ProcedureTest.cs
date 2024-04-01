using System.Diagnostics;
using NUnit.Framework;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework.Legacy;
    using System.Data;

    [TestFixture]
    public class ProcedureTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void GetCustomersTest()
        {
            var db = DatabaseHelper.Open();
            var results = db.GetCustomers();
            var actual = results.First();
            ClassicAssert.AreEqual(1, actual.CustomerId);
        }

        [Test]
        public void GetCustomerCountTest()
        {
            var db = DatabaseHelper.Open();
            var results = db.GetCustomerCount();
            ClassicAssert.AreEqual(1, results.ReturnValue);
        }

        [Test]
        public void FindGetCustomerCountAndInvokeTest()
        {
            var db = DatabaseHelper.Open();
            var getCustomerCount = db.GetCustomerCount;
            var results = getCustomerCount();
            ClassicAssert.AreEqual(1, results.ReturnValue);
        }

        [Test]
        public void FindGetCustomerCountUsingIndexerAndInvokeTest()
        {
            var db = DatabaseHelper.Open();
            var getCustomerCount = db["GetCustomerCount"];
            var results = getCustomerCount();
            ClassicAssert.AreEqual(1, results.ReturnValue);
        }

        [Test]
        public void SchemaUnqualifiedProcedureResolutionTest()
        {
            var db = DatabaseHelper.Open();
            var actual = db.SchemaProc().FirstOrDefault();
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual("dbo.SchemaProc", actual.Actual);
        }

        [Test]
        public void SchemaQualifiedProcedureResolutionTest()
        {
            var db = DatabaseHelper.Open();
            var actual = db.test.SchemaProc().FirstOrDefault();
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual("test.SchemaProc", actual.Actual);
        }

        [Test]
        public void GetCustomerCountAsOutputTest()
        {
            var db = DatabaseHelper.Open();
            var actual = db.GetCustomerCountAsOutput();
            ClassicAssert.AreEqual(42, actual.OutputValues["Count"]);
        }

#if DEBUG // Trace is only written for DEBUG build
        [Test]
        public void GetCustomerCountSecondCallExecutesNonQueryTest()
        {
            SimpleDataTraceSources.TraceSource.Switch.Level = SourceLevels.All;
            var listener = new TestTraceListener();
            SimpleDataTraceSources.TraceSource.Listeners.Add(listener);
            var db = DatabaseHelper.Open();
            db.GetCustomerCount();
            ClassicAssert.IsFalse(listener.Output.Contains("ExecuteNonQuery"));
            db.GetCustomerCount();
            ClassicAssert.IsTrue(listener.Output.Contains("ExecuteNonQuery"));
            SimpleDataTraceSources.TraceSource.Listeners.Remove(listener);
        }
#endif

        [Test]
        public void GetCustomerAndOrdersTest()
        {
            var db = DatabaseHelper.Open();
            var results = db.GetCustomerAndOrders(1);
            var customer = results.FirstOrDefault();
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual(1, customer.CustomerId);
            ClassicAssert.IsTrue(results.NextResult());
            var order = results.FirstOrDefault();
            ClassicAssert.IsNotNull(order);
            ClassicAssert.AreEqual(1, order.OrderId);
        }

        [Test]
        public void AddCustomerTest()
        {
            var db = DatabaseHelper.Open();
            Customer customer;
            customer = db.AddCustomer("Peter", "Address").FirstOrDefault();
            ClassicAssert.IsNotNull(customer);
            customer = db.Customers.FindByCustomerId(customer.CustomerId);
            ClassicAssert.IsNotNull(customer);
        }

        [Test]
        public void AddCustomerNullAddressTest()
        {
            var db = DatabaseHelper.Open();
            Customer customer;
            customer = db.AddCustomer("Peter", null).FirstOrDefault();
            ClassicAssert.IsNotNull(customer);
            customer = db.Customers.FindByCustomerId(customer.CustomerId);
            ClassicAssert.IsNotNull(customer);
        }

        [Test]
        public void GetCustomerAndOrdersStillWorksAfterZeroRecordCallTest()
        {
            var db = DatabaseHelper.Open();
            db.GetCustomerAndOrders(1000);
            var results = db.GetCustomerAndOrders(1);
            var customer = results.FirstOrDefault();
            ClassicAssert.IsNotNull(customer);
            ClassicAssert.AreEqual(1, customer.CustomerId);
            ClassicAssert.IsTrue(results.NextResult());
            var order = results.FirstOrDefault();
            ClassicAssert.IsNotNull(order);
            ClassicAssert.AreEqual(1, order.OrderId);
        }

        [Test]
        public void ScalarFunctionIsCalledCorrectly()
        {
            var db = DatabaseHelper.Open();
            var results = db.VarcharAndReturnInt("The answer to everything");
            ClassicAssert.AreEqual(42, results.ReturnValue);
        }

        [Test]
        public void CallProcedureWithDataTable()
        {
            var db = DatabaseHelper.Open();
            var dataTable = new DataTable();
            dataTable.Columns.Add("Value");
            dataTable.Rows.Add("One");
            dataTable.Rows.Add("Two");
            dataTable.Rows.Add("Three");

            var actual = db.ReturnStrings(dataTable).ToScalarList<string>();

            ClassicAssert.AreEqual(3, actual.Count);
            ClassicAssert.Contains("One", actual);
            ClassicAssert.Contains("Two", actual);
            ClassicAssert.Contains("Three", actual);
        }
    }
}
