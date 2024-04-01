namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class SimpleRecordCloneTest
    {
        [Test]
        public void CloneShouldNotBeSameObject()
        {
            dynamic target = new SimpleRecord();
            var actual = target.Clone();
            ClassicAssert.AreNotSame(target, actual);
        }

        [Test]
        public void CloneShouldContainSameValues()
        {
            dynamic target = new SimpleRecord();
            target.Name = "Foo";
            var actual = target.Clone();
            ClassicAssert.AreNotSame(target, actual);
            ClassicAssert.AreEqual(target.Name, actual.Name);
        }

        [Test]
        public void CloneShouldNotChangeWhenOriginalChanges()
        {
            dynamic target = new SimpleRecord();
            target.Name = "Foo";
            var actual = target.Clone();
            target.Name = "Bar";
            ClassicAssert.AreEqual("Foo", actual.Name);
        }
    }
}