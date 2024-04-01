namespace Simple.Data.UnitTest
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class SimpleDataExceptionTest
    {
        [Test]
        public void EmptyConstructor()
        {
            var actual = new SimpleDataException();
            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual(actual.Message, "Exception of type 'Simple.Data.SimpleDataException' was thrown.");
        }

        [Test]
        public void StringConstructor()
        {
            var actual = new SimpleDataException("Foo");
            ClassicAssert.AreEqual("Foo", actual.Message);
        }

        [Test]
        public void StringAndExceptionConstructor()
        {
            var inner = new Exception();
            var actual = new SimpleDataException("Foo", inner);
            ClassicAssert.AreEqual("Foo", actual.Message);
            ClassicAssert.AreSame(inner, actual.InnerException);
        }
    }
}
