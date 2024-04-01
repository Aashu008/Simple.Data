namespace Simple.Data.UnitTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class SimpleListAsListTest
    {
        private IList<object> CreateTarget()
        {
            return new SimpleList(Enumerable.Empty<string>());
        }

        private object CreateEntry(int seed)
        {
            return seed.ToString();
        }

        [Test]
        public void AddShouldAddItem()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);
            Assert.That(1, Is.EqualTo(target.Count));
            Assert.That(entry, Is.EqualTo(target[0]));
        }

        [Test]
        public void ClearShouldRemoveAllEntries()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            target.Add(entry);

            target.Clear();
            Assert.That(0, Is.EqualTo(target.Count));
            ClassicAssert.IsFalse(target.Contains(entry));
        }

        [Test]
        public void WhenListContainsItemContainsShouldBeTrue()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            target.Add(entry);

            ClassicAssert.IsTrue(target.Contains(entry));
        }

        [Test]
        public void WhenListDoesNotContainItemContainsShouldBeFalse()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);

            ClassicAssert.IsFalse(target.Contains(entry));
        }

        [Test]
        public void NewListShouldHaveCountEqualToZero()
        {
            var target = CreateTarget();

            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void ListWithOneItemShouldHaveCountEqualToOne()
        {
            var target = CreateTarget();
            target.Add(CreateEntry(0));

            ClassicAssert.AreEqual(1, target.Count);
        }

        [Test]
        public void CopyToZeroBasedShouldCopyAllElements()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target.Add(entry1);

            var array = new object[2];
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

            var array = new object[3];
            target.CopyTo(array, 1);

            ClassicAssert.AreEqual(entry0, array[1]);
            ClassicAssert.AreEqual(entry1, array[2]);
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
        public void NonGenericEnumeratorTest()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);

            var enumerator = ((IEnumerable)target).GetEnumerator();
            ClassicAssert.IsTrue(enumerator.MoveNext());
            ClassicAssert.AreEqual(entry, enumerator.Current);
            ClassicAssert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void IndexOfShouldReturnCorrectValues()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);

            target.Add(entry0);
            target.Add(entry1);

            ClassicAssert.AreEqual(0, target.IndexOf(entry0));
            ClassicAssert.AreEqual(1, target.IndexOf(entry1));
        }

        [Test]
        public void InsertShouldPutItemAtCorrectIndex()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);

            target.Add(entry1);
            ClassicAssert.AreEqual(0, target.IndexOf(entry1));

            target.Insert(0, entry0);

            ClassicAssert.AreEqual(0, target.IndexOf(entry0));
            ClassicAssert.AreEqual(1, target.IndexOf(entry1));
        }

        [Test]
        public void RemoveShouldRemoveEntry()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);

            target.Add(entry0);
            target.Remove(entry0);
            ClassicAssert.IsFalse(target.Contains(entry0));
            ClassicAssert.AreEqual(0, target.Count);
        }

        [Test]
        public void RemoveAtShouldRemoveEntryAtCorrectIndex()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);

            target.Add(entry0);
            target.Add(entry1);

            target.RemoveAt(0);
            Assert.That(1, Is.EqualTo(target.Count));
            Assert.That(target[0], Is.EqualTo(entry1));
        }

        [Test]
        public void GetIndexerTest()
        {
            var target = CreateTarget();
            var entry = CreateEntry(0);
            target.Add(entry);

            Assert.That(entry, Is.EqualTo(target[0]));
        }

        [Test]
        public void SetIndexerTest()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);
            var entry1 = CreateEntry(1);
            target.Add(entry0);
            target[0] = entry1;

            Assert.That(entry1, Is.EqualTo(target[0]));
        }

        [Test]
        public void SetIndexerBeyondSizeOfListShouldThrowException()
        {
            var target = CreateTarget();
            var entry0 = CreateEntry(0);

            Assert.Throws<ArgumentOutOfRangeException>(() => target[0] = entry0);


        }
    }
}
