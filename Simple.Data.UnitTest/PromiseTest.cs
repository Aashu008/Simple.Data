using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.UnitTest
{
    using System.Threading;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class PromiseTest
    {
        [Test]
        public void PromiseShouldHaveHasValueFalseWhenCreated()
        {
            Action<int> setAction;
            var actual = Promise<int>.Create(out setAction);
            ClassicAssert.IsFalse(actual.HasValue);
        }

        [Test]
        public void PromiseShouldHaveValueAfterSetActionIsCalled()
        {
            Action<int> setAction;
            var actual = Promise<int>.Create(out setAction);
            setAction(42);
            ClassicAssert.IsTrue(actual.HasValue);
            ClassicAssert.AreEqual(42, actual.Value);
        }

        [Test]
        public void PromiseShouldImplicitlyCastToType()
        {
            Action<int> setAction;
            var actual = Promise<int>.Create(out setAction);
            setAction(42);
            Assert.That(42, Is.EqualTo(actual.Value));
        }

        [Test]
        public void PromiseShouldSpinWhenValueAccessedButNotSet()
        {
            Action<int> setAction;
            var actual = Promise<int>.Create(out setAction);
            using (new Timer(_ => setAction(42), null, 100, Timeout.Infinite))
            {
                ClassicAssert.AreEqual(42, actual.Value);
            }
        }
    }
}
