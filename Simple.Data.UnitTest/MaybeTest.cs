namespace Simple.Data.UnitTest
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class MaybeTest
    {

        [Test]
        public void MaybeNoneShouldBeFalse()
        {
            ClassicAssert.False(Maybe<int>.None.HasValue);
        }

        [Test]
        public void MaybeSomeShouldBeTrue()
        {
            ClassicAssert.True(Maybe<int>.Some(1).HasValue);
        }

        [Test]
        public void IteratorShouldRun()
        {
            int n = 0;
            Func<Maybe<int>> iterator = () => ++n < 10 ? n : Maybe<int>.None;
            Maybe<int> maybe;
            while ((maybe = iterator()).HasValue)
            {
                ClassicAssert.AreEqual(n, maybe.Value);
            }
            ClassicAssert.False(maybe.HasValue);
        }

        [Test]
        public void NoneOfSameTypeShouldBeEqual()
        {
            ClassicAssert.AreEqual(Maybe<int>.None, Maybe<int>.None);
        }

        [Test]
        public void NoneOfDifferentTypeShouldNotBeEqual()
        {
            ClassicAssert.AreNotEqual(Maybe<int>.None, Maybe<long>.None);
        }

        [Test]
        public void NoneValueIsDefault()
        {
            ClassicAssert.AreEqual(default(int), Maybe<int>.None.Value);
            ClassicAssert.IsNull(Maybe<object>.None.Value);
        }

        [Test]
        public void NoneToStringIsEmptyString()
        {
            ClassicAssert.AreEqual(string.Empty, Maybe<int>.None.ToString());
        }

        [Test]
        public void SomeEqualityTrueTest()
        {
            var first = Maybe.Some(42);
            var second = Maybe.Some(42);
            ClassicAssert.IsTrue(first == second);
        }

        [Test]
        public void SomeEqualityFalseTest()
        {
            var first = Maybe.Some(42);
            var second = Maybe.Some(43);
            ClassicAssert.IsFalse(first == second);
        }

        [Test]
        public void SomeInequalityFalseTest()
        {
            var first = Maybe.Some(42);
            var second = Maybe.Some(42);
            ClassicAssert.IsFalse(first != second);
        }

        [Test]
        public void SomeInequalityTrueTest()
        {
            var first = Maybe.Some(42);
            var second = Maybe.Some(43);
            ClassicAssert.IsTrue(first != second);
        }
    }
}