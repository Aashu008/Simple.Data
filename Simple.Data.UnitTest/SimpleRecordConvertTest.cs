namespace Simple.Data.UnitTest
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class SimpleRecordConvertTest
    {
        [Test]
        public void CanConvertToFoo()
        {
            dynamic source = new SimpleRecord(new Dictionary<string, object> { { "X", "Bar" } });
            Foo actual = source;
            ClassicAssert.AreEqual("Bar", actual.X);
        }

        [Test]
        public void CanConvertWithSubItemToFoo()
        {
            dynamic source = new SimpleRecord(new Dictionary<string, object> { { "X", "Bar" }, { "Y", new Dictionary<string, object> { { "X", "Quux" } } } });
            Foo actual = source;
            ClassicAssert.AreEqual("Bar", actual.X);
            ClassicAssert.IsNotNull(actual.Y);
            ClassicAssert.AreEqual("Quux", actual.Y.X);
        }

        [Test]
        public void CanConvertWithSubItemAndCollectionToFoo()
        {
            dynamic source =
                new SimpleRecord(new Dictionary<string, object>
                                     {{"X", "Bar"},
                                     {"Y", new Dictionary<string, object> {{"X", "Quux"}}},
                                     {"Z", new[] { new Dictionary<string, object> {{"X", "Wibble"}}}}
                                     });
            Foo actual = source;
            ClassicAssert.AreEqual("Bar", actual.X);
            ClassicAssert.IsNotNull(actual.Y);
            ClassicAssert.AreEqual("Quux", actual.Y.X);
            ClassicAssert.IsNotNull(actual.Z);
            ClassicAssert.AreEqual(1, actual.Z.Count);
            ClassicAssert.AreEqual("Wibble", actual.Z.Single().X);
        }

        public class Foo
        {
            public string X { get; set; }
            public Foo Y { get; set; }
            public ICollection<Foo> Z { get; set; }
        }
    }
}
