namespace Simple.Data.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class SimpleRecordAsDictionaryTest
    {
        private IDictionary<string, object> CreateTarget()
        {
            return new SimpleRecord();
        }

        private KeyValuePair<string, object> CreateEntry(int index)
        {
            return new KeyValuePair<string, object>(index.ToString(), index);
        }

        [Test]
        public void AddKeyValuePair()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            target.Add(entry);

            ClassicAssert.AreEqual(1, target.Count);
            ClassicAssert.IsTrue(target.ContainsKey(entry.Key));
            ClassicAssert.AreEqual(entry.Value, target[entry.Key]);
        }

        [Test]
        public void AddDuplicateKeyValuePairShouldThrow()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            Assert.Throws<ArgumentException>(() =>
            {
                target.Add(entry);
                target.Add(entry);
            });
        }

        [Test]
        public void AddKeyAndValue()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            target.Add(entry.Key, entry.Value);

            ClassicAssert.AreEqual(1, target.Count);
            ClassicAssert.IsTrue(target.ContainsKey(entry.Key));
            ClassicAssert.AreEqual(entry.Value, target[entry.Key]);
        }

        [Test]
        public void AddDuplicateKeyAndValueShouldThrow()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            Assert.Throws<ArgumentException>(() =>
            {
                target.Add(entry.Key, entry.Value);
                target.Add(entry.Key, entry.Value);
            });


        }

        [Test]
        public void AfterClearCountShouldBeZero()
        {
            var target = CreateTarget();
            target.Add(CreateEntry(0));
            target.Add(CreateEntry(1));
            target.Clear();

            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void ContainsShouldReturnTrueForValidEntry()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            ClassicAssert.IsTrue(target.Contains(entry));
        }

        [Test]
        public void ContainsShouldReturnFalseForInvalidEntry()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            ClassicAssert.IsFalse(target.Contains(entry));
        }

        [Test]
        public void ContainsKeyShouldReturnTrueForValidKey()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            ClassicAssert.IsTrue(target.ContainsKey(entry.Key));
        }

        [Test]
        public void ContainsKeyShouldReturnFalseForInvalidKey()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            ClassicAssert.IsFalse(target.ContainsKey(entry.Key));
        }

        [Test]
        public void CopyToZeroBasedShouldCopyAllElements()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);

            var array = new KeyValuePair<string, object>[2];
            target.CopyTo(array, 0);

            ClassicAssert.AreEqual(entry0, array[0]);
            ClassicAssert.AreEqual(entry1, array[1]);
        }

        [Test]
        public void CopyToNonZeroBasedShouldCopyElementsFromIndex()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);

            var array = new KeyValuePair<string, object>[3];
            target.CopyTo(array, 1);

            ClassicAssert.AreEqual(entry0, array[1]);
            ClassicAssert.AreEqual(entry1, array[2]);
        }

        [Test]
        public void CountOnNewInstanceShouldBeZero()
        {
            var target = CreateTarget();
            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void CountAfterAddShouldBeOne()
        {
            var target = CreateTarget();
            target.Add(CreateEntry(0));
            ClassicAssert.AreEqual(1, target.Count);
        }

        [Test]
        public void EnumeratorTest()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);

            var enumerator = target.GetEnumerator();
            ClassicAssert.IsTrue(enumerator.MoveNext());
            ClassicAssert.AreEqual(entry, enumerator.Current);
            ClassicAssert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void KeysShouldContainKey()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            ClassicAssert.AreEqual(1, target.Keys.Count);
            ClassicAssert.AreEqual(entry.Key, target.Keys.Single());
        }

        [Test]
        public void KeysShouldContainKeys()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);
            ClassicAssert.AreEqual(2, target.Keys.Count);
            ClassicAssert.AreEqual(entry0.Key, target.Keys.First());
            ClassicAssert.AreEqual(entry1.Key, target.Keys.Last());
        }

        [Test]
        public void RemoveKeyValuePairShouldRemoveIt()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            target.Remove(entry);
            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void RemoveKeyValuePairShouldOnlyRemoveIt()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);
            target.Remove(entry0);
            ClassicAssert.AreEqual(1, target.Count);
            ClassicAssert.IsTrue(target.Contains(entry1));
        }

        [Test]
        public void RemoveKeyShouldRemoveIt()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            target.Remove(entry.Key);
            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void RemoveKeyShouldOnlyRemoveIt()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);
            target.Remove(entry0.Key);
            ClassicAssert.AreEqual(1, target.Count);
            ClassicAssert.IsTrue(target.Contains(entry1));
        }

        [Test]
        public void TryGetValueShouldReturnTrueAndGetValue()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);

            object value;
            ClassicAssert.IsTrue(target.TryGetValue(entry.Key, out value));
            ClassicAssert.AreEqual(entry.Value, value);
        }

        [Test]
        public void TryGetValueShouldReturnFalseAndGetDefaultValue()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            object value;
            ClassicAssert.IsFalse(target.TryGetValue(entry.Key, out value));
            ClassicAssert.AreEqual(default(object), value);
        }

        [Test]
        public void IndexerShouldReturnCorrectValue()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);

            ClassicAssert.AreEqual(entry.Value, target[entry.Key]);
        }


        [Test]
        public void IndexerShouldThrowWithInvalidKey()
        {
            var target = CreateTarget();

            Assert.Throws<KeyNotFoundException>(() => { var x = target["INVALIDKEY"]; });

        }
    }
}
