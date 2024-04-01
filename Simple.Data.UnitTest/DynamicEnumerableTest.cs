using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.UnitTest
{
    [TestFixture]
    public class DynamicEnumerableTest
    {
        [Test]
        public void TestCast()
        {
            dynamic test = new SimpleResultSet(new[] { "Hello", "World" });
            IEnumerable<string> strings = test.Cast<string>();
            ClassicAssert.AreEqual(2, strings.Count());
        }

        //        [Test]
        //        public void TestOfType()
        //        {
        //            dynamic test = new SimpleResultSet(new dynamic[] { "Hello", 1 });
        //            IEnumerable<int> ints = test.OfType<int>();
        //             Assert.That(count, Is.EqualTo(3));(1, ints.Count());
        //             Assert.That(count, Is.EqualTo(3));(1, ints.Single());
        //        }

        [Test]
        public void TestCastWithClass()
        {
            var dict = new Dictionary<string, object>(HomogenizedEqualityComparer.DefaultInstance) { { "Name", "Bob" } };
            dynamic test = new SimpleResultSet(new[] { new SimpleRecord(dict) });
            IEnumerable<Foo> foos = test.Cast<Foo>();
            ClassicAssert.AreEqual(1, foos.Count());
        }

        [Test]
        public void TestCastWithForeach()
        {
            var dict = new Dictionary<string, object>(HomogenizedEqualityComparer.DefaultInstance) { { "Name", "Bob" } };
            dynamic test = new SimpleResultSet(new[] { new SimpleRecord(dict) });
            foreach (Foo foo in test)
            {
                ClassicAssert.AreEqual("Bob", foo.Name);
            }
        }

        class Foo
        {
            public string Name { get; set; }
        }
    }
}
