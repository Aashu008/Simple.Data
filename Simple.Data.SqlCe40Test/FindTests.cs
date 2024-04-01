using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using Simple.Data.Ado;
using Simple.Data.SqlCe40;
using Simple.Data.SqlCeTest;
using NUnit.Framework.Legacy;

namespace Simple.Data.SqlCe40Test
{
    /// <summary>
    /// Summary description for FindTests
    /// </summary>
    [TestFixture]
    public class FindTests
    {
        private static readonly string DatabasePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8)),
            "TestDatabase.sdf");

        [OneTimeSetUp]
        public void DeleteAlice()
        {
            var db = Database.Opener.OpenFile(DatabasePath);
            db.Users.DeleteByName("Alice");
        }

        [Test]
        public void TestProviderWithFileName()
        {
            var provider = new ProviderHelper().GetProviderByFilename(DatabasePath);
            ClassicAssert.IsInstanceOf(typeof(SqlCe40ConnectionProvider), provider);
        }

        [Test]
        public void TestProviderWithConnectionString()
        {
            var provider = new ProviderHelper().GetProviderByConnectionString(string.Format("data source={0}", DatabasePath));
            ClassicAssert.IsInstanceOf(typeof(SqlCe40ConnectionProvider), provider);
        }

        [Test]
        public void TestFindById()
        {
            var db = Database.Opener.OpenFile(DatabasePath);
            var user = db.Users.FindById(1);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void TestAll()
        {
            var db = Database.OpenFile(DatabasePath);
            var all = new List<object>(db.Users.All().Cast<dynamic>());
            ClassicAssert.IsNotEmpty(all);
        }

        [Test]
        public void TestImplicitCast()
        {
            var db = Database.OpenFile(DatabasePath);
            User user = db.Users.FindById(1);
            ClassicAssert.AreEqual(1, user.Id);
        }

        [Test]
        public void TestImplicitEnumerableCast()
        {
            var db = Database.OpenFile(DatabasePath);
            foreach (User user in db.Users.All())
            {
                ClassicAssert.IsNotNull(user);
            }
        }

        [Test]
        public void TestInsert()
        {
            var db = Database.OpenFile(DatabasePath);

            var user = db.Users.Insert(Name: "Alice", Password: "foo", Age: 29);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("Alice", user.Name);
            ClassicAssert.AreEqual("foo", user.Password);
            ClassicAssert.AreEqual(29, user.Age);
        }
    }
}
