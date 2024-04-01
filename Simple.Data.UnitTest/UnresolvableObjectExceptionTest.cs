namespace Simple.Data.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    public class UnresolvableObjectExceptionTest
    {
        [Test]
        public void EmptyConstructor()
        {
            var actual = new UnresolvableObjectException();
            ClassicAssert.IsNotNull(actual);
            Assert.That(actual.ObjectName, Is.Null.Or.Empty);
        }

        [Test]
        public void SingleStringSetsObjectName()
        {
            var actual = new UnresolvableObjectException("Foo");
            Assert.That("Foo", Is.EqualTo(actual.ObjectName));
        }

        [Test]
        public void TwoStringsSetsObjectNameAndMessage()
        {
            var actual = new UnresolvableObjectException("Foo", "Bar");
            Assert.That("Foo", Is.EqualTo(actual.ObjectName));
            Assert.That("Bar", Is.EqualTo(actual.Message));
        }

        [Test]
        public void TwoStringsAndAnExceptionSetsObjectNameAndMessageAndInnerException()
        {
            var inner = new Exception();
            var actual = new UnresolvableObjectException("Foo", "Bar", inner);
            Assert.That("Foo", Is.EqualTo(actual.ObjectName));
            Assert.That("Bar", Is.EqualTo(actual.Message));
            ClassicAssert.AreSame(inner, actual.InnerException);
        }
    }
}