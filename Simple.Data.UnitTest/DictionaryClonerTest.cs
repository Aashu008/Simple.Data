namespace Simple.Data.UnitTest
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class DictionaryClonerTest
    {
        [Test]
        public void ClonedDictionaryShouldBeOfSameType()
        {
            var target = new DictionaryCloner();

            var original = new SortedDictionary<string, object>();
            var clone = target.CloneDictionary(original);
            ClassicAssert.AreNotSame(original, clone);
            ClassicAssert.IsInstanceOf<SortedDictionary<string, object>>(clone);
        }

        [Test]
        public void NestedDictionaryShouldBeCloned()
        {
            var target = new DictionaryCloner();
            var nested = new Dictionary<string, object> { { "Answer", 42 } };
            var original = new Dictionary<string, object> { { "Nested", nested } };

            var clone = target.CloneDictionary(original);

            ClassicAssert.IsTrue(clone.ContainsKey("Nested"));
            var nestedClone = clone["Nested"] as Dictionary<string, object>;
            ClassicAssert.IsNotNull(nestedClone);
            ClassicAssert.AreNotSame(nested, nestedClone);
            ClassicAssert.IsTrue(nestedClone.ContainsKey("Answer"));
            ClassicAssert.AreEqual(42, nestedClone["Answer"]);
        }

        [Test]
        public void NestedDictionariesShouldBeCloned()
        {
            var target = new DictionaryCloner();
            var nested = new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Answer", 42 } } };
            var original = new Dictionary<string, object> { { "Nested", nested } };

            var clone = target.CloneDictionary(original);

            ClassicAssert.IsTrue(clone.ContainsKey("Nested"));
            var nestedClone = clone["Nested"] as List<Dictionary<string, object>>;
            ClassicAssert.IsNotNull(nestedClone);
            ClassicAssert.AreNotSame(nested, nestedClone);
            ClassicAssert.IsTrue(nestedClone.Count == 1);
            ClassicAssert.AreEqual(42, nestedClone[0]["Answer"]);
        }
    }
}