using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Simple.Data;

namespace Simple.Data.UnitTest
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        public void IntRangeTest()
        {
            var range = 1.to(10);
            ClassicAssert.AreEqual(1, range.Start);
            ClassicAssert.AreEqual(10, range.End);
        }

        [Test]
        public void StringToDateRangeTest()
        {
            var range = "2011-01-01".to("2011-01-31");
            ClassicAssert.AreEqual(new DateTime(2011, 1, 1), range.Start);
            ClassicAssert.AreEqual(new DateTime(2011, 1, 31), range.End);
        }

        [Test]
        public void RangeToStringTest()
        {
            var range = 1.to(10);
            ClassicAssert.AreEqual("(1..10)", range.ToString());
        }

        [Test]
        public void RangeEqualityTest()
        {
            var range1 = 1.to(10);
            var range2 = 1.to(10);
            ClassicAssert.IsTrue(range1.Equals(range2));
        }

        [Test]
        public void RangeAsObjectEqualityTest()
        {
            object range1 = 1.to(10);
            object range2 = 1.to(10);
            ClassicAssert.IsTrue(range1.Equals(range2));
        }

        [Test]
        public void RangeEqualityOperatorTestTrue()
        {
            var range1 = 1.to(10);
            var range2 = 1.to(10);
            ClassicAssert.IsTrue(range1 == range2);
        }

        [Test]
        public void RangeEqualityOperatorTestFalse()
        {
            var range1 = 1.to(10);
            var range2 = 1.to(9);
            ClassicAssert.IsFalse(range1 == range2);
        }

        [Test]
        public void RangeInequalityOperatorTestFalse()
        {
            var range1 = 1.to(10);
            var range2 = 1.to(10);
            ClassicAssert.IsFalse(range1 != range2);
        }

        [Test]
        public void RangeInequalityOperatorTestTrue()
        {
            var range1 = 1.to(10);
            var range2 = 1.to(9);
            ClassicAssert.IsTrue(range1 != range2);
        }

        [Test]
        public void AsEnumerableFromIRange()
        {
            IRange range = 1.to(10);
            var enumerator = range.AsEnumerable().GetEnumerator();
            enumerator.MoveNext();
            ClassicAssert.AreEqual(1, enumerator.Current);
            enumerator.MoveNext();
            ClassicAssert.AreEqual(10, enumerator.Current);
        }

        [Test]
        public void AsEnumerable()
        {
            var range = 1.to(10);
            var enumerator = range.AsEnumerable().GetEnumerator();
            enumerator.MoveNext();
            ClassicAssert.AreEqual(1, enumerator.Current);
            enumerator.MoveNext();
            ClassicAssert.AreEqual(10, enumerator.Current);
        }

        [Test]
        public void AsEnumerableWithStep()
        {
            var range = 1.to(10);
            var enumerator = range.AsEnumerable(n => n + 1).GetEnumerator();
            for (int i = 1; i < 11; i++)
            {
                ClassicAssert.IsTrue(enumerator.MoveNext());
                ClassicAssert.AreEqual(i, enumerator.Current);
            }
        }
    }
}
