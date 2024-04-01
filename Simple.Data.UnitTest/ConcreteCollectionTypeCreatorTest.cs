namespace Simple.Data.UnitTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ConcreteCollectionTypeCreatorTest
    {
        private IEnumerable<dynamic> Items()
        {
            yield return "Foo";
            yield return "Bar";
        }

        [Test]
        public void ListTest()
        {
            object result;
            ConcreteCollectionTypeCreator.TryCreate(typeof(List<string>), Items(), out result);
            ClassicAssert.IsNotNull(result);

            var list = result as List<string>;

            ClassicAssert.IsNotNull(list);
            ClassicAssert.AreEqual(2, list.Count);
            ClassicAssert.IsTrue(list.Contains("Foo"));
            ClassicAssert.IsTrue(list.Contains("Bar"));
        }

        [Test]
        public void SetTest()
        {
            object result;
            ConcreteCollectionTypeCreator.TryCreate(typeof(HashSet<string>), Items(), out result);
            ClassicAssert.IsNotNull(result);

            var @set = result as HashSet<string>;

            ClassicAssert.IsNotNull(@set);
            ClassicAssert.AreEqual(2, @set.Count);
            ClassicAssert.IsTrue(@set.Contains("Foo"));
            ClassicAssert.IsTrue(@set.Contains("Bar"));
        }

        [Test]
        public void ArrayListTest()
        {
            object result;
            ConcreteCollectionTypeCreator.TryCreate(typeof(ArrayList), Items(), out result);
            ClassicAssert.IsNotNull(result);

            var list = result as ArrayList;

            ClassicAssert.IsNotNull(list);
            ClassicAssert.AreEqual(2, list.Count);
            ClassicAssert.IsTrue(list.Contains("Foo"));
            ClassicAssert.IsTrue(list.Contains("Bar"));
        }

        [Test]
        public void TryConvertElementShouldConvertStringToEnum()
        {
            var testCreator = new TestCreator();
            object result;
            ClassicAssert.IsTrue(testCreator.TestTryConvertElement(typeof(TestEnum), "Value", out result));
            ClassicAssert.AreEqual(TestEnum.Value, result);
        }

        [Test]
        public void TryConvertElementShouldConvertIntToEnum()
        {
            var testCreator = new TestCreator();
            object result;
            ClassicAssert.IsTrue(testCreator.TestTryConvertElement(typeof(TestEnum), 1, out result));
            ClassicAssert.AreEqual(TestEnum.Value, result);
        }

        [Test]
        public void TryConvertElementShouldConvertIntToNullableInt()
        {
            var testCreator = new TestCreator();
            object result;
            ClassicAssert.IsTrue(testCreator.TestTryConvertElement(typeof(int?), 1, out result));
            ClassicAssert.AreEqual(1, result);
        }

        enum TestEnum
        {
            None = 0,
            Value = 1
        }
    }

    class TestCreator : ConcreteCollectionTypeCreator.Creator
    {
        public override bool IsCollectionType(Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type);
        }

        public override bool TryCreate(Type type, IEnumerable items, out object result)
        {
            throw new NotImplementedException();
        }

        public bool TestTryConvertElement(Type type, object value, out object result)
        {
            return TryConvertElement(type, value, out result);
        }
    }
}
