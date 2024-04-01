using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Simple.Data.Extensions;

namespace Simple.Data.UnitTest
{


    /// <summary>
    ///This is a test class for StringExtensionsTest and is intended
    ///to contain all StringExtensionsTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class StringExtensionsTest
    {
        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralLowercaseUsersShouldReturnTrue()
        {
            ClassicAssert.IsTrue("Users".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralUppercaseUsersShouldReturnTrue()
        {
            ClassicAssert.IsTrue("USERS".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralLowercaseUserShouldReturnFalse()
        {
            ClassicAssert.IsFalse("User".IsPlural());
        }

        /// <summary>
        ///A test for IsPlural
        ///</summary>
        [Test]
        public void IsPluralUppercaseUserShouldReturnFalse()
        {
            ClassicAssert.IsFalse("USER".IsPlural());
        }

        /// <summary>
        ///A test for Pluralize
        ///</summary>
        [Test()]
        public void PluralizeUserShouldReturnUsers()
        {
            ClassicAssert.AreEqual("Users", "User".Pluralize());
        }

        /// <summary>
        ///A test for Pluralize
        ///</summary>
        [Test()]
        // ReSharper disable InconsistentNaming
        public void PluralizeUSERShouldReturnUSERS()
        // ReSharper restore InconsistentNaming
        {
            ClassicAssert.AreEqual("USERS", "USER".Pluralize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        public void SingularizeUsersShouldReturnUser()
        {
            ClassicAssert.AreEqual("User", "Users".Singularize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        public void SingularizeUserShouldReturnUser()
        {
            ClassicAssert.AreEqual("User", "User".Singularize());
        }

        /// <summary>
        ///A test for Singularize
        ///</summary>
        [Test()]
        // ReSharper disable InconsistentNaming
        public void SingularizeUSERSShouldReturnUSER()
        // ReSharper restore InconsistentNaming
        {
            ClassicAssert.AreEqual("USER", "USERS".Singularize());
        }

        /// <summary>
        ///A test for IsAllUpperCase
        ///</summary>
        [Test()]
        public void IsAllUpperCaseTrueTest()
        {
            ClassicAssert.IsTrue("USERS".IsAllUpperCase());
        }

        /// <summary>
        ///A test for IsAllUpperCase
        ///</summary>
        [Test()]
        public void IsAllUpperCaseProperFalseTest()
        {
            ClassicAssert.IsFalse("Users".IsAllUpperCase());
        }

        /// <summary>
        ///A test for IsAllUpperCase
        ///</summary>
        [Test()]
        public void IsAllUpperCasePascalFalseTest()
        {
            ClassicAssert.IsFalse("MoreUsers".IsAllUpperCase());
        }

        /// <summary>
        ///A test for IsAllUpperCase
        ///</summary>
        [Test()]
        public void IsAllUpperCaseLowerFalseTest()
        {
            ClassicAssert.IsFalse("users".IsAllUpperCase());
        }

        /// <summary>
        ///A test for IsAllUpperCase
        ///</summary>
        [Test()]
        public void IsAllUpperCaseMixedFalseTest()
        {
            ClassicAssert.IsFalse("CUSTOMEr".IsAllUpperCase());
        }
    }
}
