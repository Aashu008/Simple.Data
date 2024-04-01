using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.SqlTest.SchemaTests
{
    [TestFixture]
    public class DataTypeTests
    {
        [Test]
        public void TestDecimalCanBeRetrievedCorrectly()
        {
            var db = DatabaseHelper.Open();
            var value = db.DecimalTest.FindById(1).Value;
            ClassicAssert.AreEqual(typeof(Decimal), value.GetType());
            ClassicAssert.AreEqual(1.234567, value);
        }

        [Test]
        public void TestDecimalCanBeInsertedCorrectly()
        {
            var db = DatabaseHelper.Open();
            var decimalTest = new { Value = 12.345678 };
            var value = db.DecimalTest.Insert(decimalTest).Value;
            ClassicAssert.AreEqual(typeof(Decimal), value.GetType());
            ClassicAssert.AreEqual(decimalTest.Value, value);
        }
    }
}
