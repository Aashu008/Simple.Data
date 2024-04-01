using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ExpressionHelperTest
    {
        [Test]
        public void DictionaryToExpressionTest()
        {
            var dict = new Dictionary<string, object>
                           {
                               { "foo", 1 },
                               { "bar", 2 }
                           };

            var actual = ExpressionHelper.CriteriaDictionaryToExpression("quux", dict);

            ClassicAssert.AreEqual(SimpleExpressionType.And, actual.Type);

            var actualFirst = (SimpleExpression)actual.LeftOperand;
            var actualSecond = (SimpleExpression)actual.RightOperand;

            ClassicAssert.AreEqual("foo", ((ObjectReference)actualFirst.LeftOperand).GetName());
            ClassicAssert.AreEqual(SimpleExpressionType.Equal, actualFirst.Type);
            ClassicAssert.AreEqual(1, actualFirst.RightOperand);

            ClassicAssert.AreEqual("bar", ((ObjectReference)actualSecond.LeftOperand).GetName());
            ClassicAssert.AreEqual(SimpleExpressionType.Equal, actualSecond.Type);
            ClassicAssert.AreEqual(2, actualSecond.RightOperand);
        }
    }
}
