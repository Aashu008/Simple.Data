#if(!MONO)
namespace Simple.Data.UnitTest
{
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;
    using Extensions;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class PluralizationTest
    {
        [SetUp]
        public void FixtureSetup()
        {
            Database.SetPluralizer(new EntityPluralizer());
        }

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

        [Test]
        public void PluralizeCompanyShouldReturnCompanies()
        {
            ClassicAssert.AreEqual("Companies", "Company".Pluralize());
        }

        [Test]
        public void SingularizeCompaniesShouldReturnCompany()
        {
            ClassicAssert.AreEqual("Company", "Companies".Singularize());
        }
    }

    class EntityPluralizer : IPluralizer
    {
        private readonly PluralizationService _pluralizationService =
            PluralizationService.CreateService(new CultureInfo("en"));

        public bool IsPlural(string word)
        {
            return _pluralizationService.IsPlural(word);
        }

        public bool IsSingular(string word)
        {
            return _pluralizationService.IsSingular(word);
        }

        public string Pluralize(string word)
        {
            bool upper = (word.IsAllUpperCase());
            word = _pluralizationService.Pluralize(word);
            return upper ? word.ToUpper(_pluralizationService.Culture) : word;
        }

        public string Singularize(string word)
        {
            return _pluralizationService.Singularize(word);
        }
    }
}
#endif