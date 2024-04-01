using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    class SpecialReferenceTests
    {
        [Test]
        public void TestExistsReference()
        {
            var actual = new ExistsSpecialReference();
            ClassicAssert.AreEqual("EXISTS", actual.Name);
        }

        [Test]
        public void TestCountReference()
        {
            var actual = new CountSpecialReference();
            ClassicAssert.AreEqual("COUNT", actual.Name);
        }

        [Test]
        public void TestSimpleEmptyExpression()
        {
            var actual = new SimpleEmptyExpression();
            ClassicAssert.IsNull(actual.LeftOperand);
            ClassicAssert.AreEqual(SimpleExpressionType.Empty, actual.Type);
            ClassicAssert.IsNull(actual.RightOperand);
        }
    }
}
