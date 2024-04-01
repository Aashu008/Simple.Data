namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class MathReferenceTest
    {
        static readonly dynamic Db = new Database(null);

        [Test]
        public void MathReferenceWithEqualsMakesExpression()
        {
            AssertHelper(Db.foo.id + 1 == 1, SimpleExpressionType.Equal, 1);
        }

        [Test]
        public void MathReferenceWithNotEqualsMakesExpression()
        {
            AssertHelper(Db.foo.id - 1 != 1, SimpleExpressionType.NotEqual, 1);
        }

        [Test]
        public void MathReferenceWithGreaterThanMakesExpression()
        {
            AssertHelper(Db.foo.id * 1 > 1, SimpleExpressionType.GreaterThan, 1);
        }

        [Test]
        public void MathReferenceWithLessThanMakesExpression()
        {
            AssertHelper(Db.foo.id / 2 < 1, SimpleExpressionType.LessThan, 1);
        }

        [Test]
        public void MathReferenceWithGreaterThanOrEqualMakesExpression()
        {
            AssertHelper(Db.foo.id % 1 >= 1, SimpleExpressionType.GreaterThanOrEqual, 1);
        }

        [Test]
        public void MathReferenceWithLessThanOrEqualMakesExpression()
        {
            AssertHelper(Db.foo.id % 2 <= 1, SimpleExpressionType.LessThanOrEqual, 1);
        }

        [Test]
        public void EqualsShouldPass()
        {
            MathReference first = Db.foo.id % 1;
            MathReference second = Db.foo.id % 1;
            ClassicAssert.IsTrue(first.Equals(second));
        }

        private static void AssertHelper<T>(SimpleExpression actual, SimpleExpressionType expectedType, T expectedRightOperand)
        {
            ClassicAssert.AreEqual(Db.foo.id, ((MathReference)actual.LeftOperand).LeftOperand);
            ClassicAssert.AreEqual(expectedType, actual.Type);
            ClassicAssert.AreEqual(expectedRightOperand, actual.RightOperand);
        }
    }
}