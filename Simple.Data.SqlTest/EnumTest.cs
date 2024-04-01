using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class EnumTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void ConvertsBetweenEnumAndInt()
        {
            var db = DatabaseHelper.Open();
            EnumTestClass actual = db.EnumTest.Insert(Flag: TestFlag.One);
            ClassicAssert.AreEqual(TestFlag.One, actual.Flag);

            actual.Flag = TestFlag.Three;

            db.EnumTest.Update(actual);

            actual = db.EnumTest.FindById(actual.Id);
            ClassicAssert.AreEqual(TestFlag.Three, actual.Flag);
        }
    }

    class EnumTestClass
    {
        public int Id { get; set; }
        public TestFlag Flag { get; set; }
    }

    enum TestFlag
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3
    }
}
