namespace Simple.Data.Ado.Test
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class AdoAdapterExceptionTest
    {
        [Test]
        public void EmptyConstructorShouldSetAdapterType()
        {
            var actual = new AdoAdapterException();
            Assert.That(typeof(AdoAdapter), Is.EqualTo(actual.AdapterType));
        }

        [Test]
        public void SingleStringConstructorShouldSetMessageAndAdapterType()
        {
            var actual = new AdoAdapterException("Foo");
            Assert.That(typeof(AdoAdapter), Is.EqualTo(actual.AdapterType));
            Assert.That("Foo", Is.EqualTo(actual.Message));
        }

        [Test]
        public void StringAndExceptionConstructorShouldSetMessageAndInnerExceptionAndAdapterType()
        {
            var inner = new Exception();
            var actual = new AdoAdapterException("Foo", inner);
            Assert.That(typeof(AdoAdapter), Is.EqualTo(actual.AdapterType));
            Assert.That("Foo", Is.EqualTo(actual.Message));
            ClassicAssert.AreSame(inner, actual.InnerException);
        }

        [Test]
        public void StringAndDbCommandConstructorShouldSetMessageAndCommandTextAndAdapterType()
        {
            var command = new SqlCommand("Bar");
            var actual = new AdoAdapterException("Foo", command);
            ClassicAssert.AreEqual(typeof(AdoAdapter), actual.AdapterType);
            ClassicAssert.AreEqual("Foo", actual.Message);
            ClassicAssert.AreEqual(command.CommandText, actual.CommandText);
        }

        [Test]
        public void StringAndDictionaryConstructorShouldSetCommandTextAndParametersAndAdapterType()
        {
            var param = new Dictionary<string, object> { { "P", "quux" } };
            var actual = new AdoAdapterException("Foo", param);
            ClassicAssert.AreEqual(typeof(AdoAdapter), actual.AdapterType);
            ClassicAssert.AreEqual("Foo", actual.CommandText);
            ClassicAssert.AreEqual("quux", actual.Parameters["P"]);
        }

        [Test]
        public void StringAndStringAndDictionaryConstructorShouldSetMessageAndCommandTextAndParametersAndAdapterType()
        {
            var param = new Dictionary<string, object> { { "P", "quux" } };
            var actual = new AdoAdapterException("Foo", "Bar", param);
            ClassicAssert.AreEqual(typeof(AdoAdapter), actual.AdapterType);
            ClassicAssert.AreEqual("Foo", actual.Message);
            ClassicAssert.AreEqual("Bar", actual.CommandText);
            ClassicAssert.AreEqual("quux", actual.Parameters["P"]);
        }

        [Test]
        public void SerializationTest()
        {
            var param = new Dictionary<string, object> { { "P", "quux" } };
            var source = new AdoAdapterException("Foo", param);

            var stream = new MemoryStream();
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(stream, source);

            stream.Position = 0;

            var actual = serializer.Deserialize(stream) as AdoAdapterException;

            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual(typeof(AdoAdapter), actual.AdapterType);
            ClassicAssert.AreEqual("Foo", actual.CommandText);
            ClassicAssert.AreEqual("quux", actual.Parameters["P"]);
        }
    }
}
