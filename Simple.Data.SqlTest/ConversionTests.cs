using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.SqlTest
{
    [TestFixture]
    public class ConversionTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void WeirdTypeGetsConvertedToInt()
        {
            var weirdValue = new WeirdType(1);
            var db = DatabaseHelper.Open();
            var user = db.Users.FindById(weirdValue);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void WeirdTypeUsedInQueryGetsConvertedToInt()
        {
            var weirdValue = new WeirdType(1);
            var db = DatabaseHelper.Open();
            var user = db.Users.QueryById(weirdValue).FirstOrDefault();
            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void InsertingWeirdTypesFromExpando()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = new WeirdType("Oddball");
            expando.Password = new WeirdType("Fish");
            expando.Age = new WeirdType(3);
            expando.ThisIsNotAColumn = new WeirdType("Submit");

            var db = DatabaseHelper.Open();
            var user = db.Users.Insert(expando);
            ClassicAssert.IsInstanceOf<int>(user.Id);
            ClassicAssert.AreEqual("Oddball", user.Name);
            ClassicAssert.AreEqual("Fish", user.Password);
            ClassicAssert.AreEqual(3, user.Age);
        }
    }

    class WeirdType : DynamicObject
    {
        private readonly object _value;

        public WeirdType(object value)
        {
            _value = value;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Convert.ChangeType(_value, binder.Type);
            return true;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
