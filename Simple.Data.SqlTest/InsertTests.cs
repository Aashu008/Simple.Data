using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;
using Simple.Data.Ado;
using Simple.Data.SqlTest.Resources;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework.Legacy;
    using System;

    [TestFixture]
    public class InsertTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void TestInsertWithNamedArguments()
        {
            var db = DatabaseHelper.Open();

            var user = db.Users.Insert(Name: "Ford", Password: "hoopy", Age: 29);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("Ford", user.Name);
            ClassicAssert.AreEqual("hoopy", user.Password);
            ClassicAssert.AreEqual(29, user.Age);
        }

        [Test]
        public void TestInsertWithIdentityInsertOn()
        {
            var db = DatabaseHelper.Open().WithOptions(new AdoOptions(identityInsert: true));
            var user = db.Users.Insert(Id: 42, Name: "Arthur", Password: "Tea", Age: 30);
            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(42, user.Id);
        }

        [Test]
        public void TestInsertWithIdentityInsertOnThenOffAgain()
        {
            var db = DatabaseHelper.Open().WithOptions(new AdoOptions(identityInsert: true));
            var user = db.Users.Insert(Id: 2267709, Name: "Douglas", Password: "dirk", Age: 49);
            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(2267709, user.Id);
            db.ClearOptions();
            user = db.Users.Insert(Name: "Frak", Password: "true", Age: 200);
            ClassicAssert.Less(2267709, user.Id);
        }

        [Test]
        public void TestInsertWithStaticTypeObject()
        {
            var db = DatabaseHelper.Open();

            var user = new User { Name = "Zaphod", Password = "zarquon", Age = 42 };

            var actual = db.Users.Insert(user);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("Zaphod", actual.Name);
            ClassicAssert.AreEqual("zarquon", actual.Password);
            ClassicAssert.AreEqual(42, actual.Age);
        }

        [Test]
        public void TestMultiInsertWithStaticTypeObjects()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            IList<User> actuals = db.Users.Insert(users).ToList<User>();

            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreNotEqual(0, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreNotEqual(0, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestMultiInsertWithStaticTypeObjectsAndNoReturn()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            db.Users.Insert(users);

            var slartibartfast = db.Users.FindByName("Slartibartfast");
            ClassicAssert.IsNotNull(slartibartfast);
            ClassicAssert.AreNotEqual(0, slartibartfast.Id);
            ClassicAssert.AreEqual("Slartibartfast", slartibartfast.Name);
            ClassicAssert.AreEqual("bistromathics", slartibartfast.Password);
            ClassicAssert.AreEqual(777, slartibartfast.Age);

            var wowbagger = db.Users.FindByName("Wowbagger");
            ClassicAssert.IsNotNull(wowbagger);

            ClassicAssert.AreNotEqual(0, wowbagger.Id);
            ClassicAssert.AreEqual("Wowbagger", wowbagger.Name);
            ClassicAssert.AreEqual("teatime", wowbagger.Password);
            ClassicAssert.AreEqual(int.MaxValue, wowbagger.Age);
        }

        [Test]
        public void TestInsertWithDynamicTypeObject()
        {
            var db = DatabaseHelper.Open();

            dynamic user = new ExpandoObject();
            user.Name = "Marvin";
            user.Password = "diodes";
            user.Age = 42000000;

            var actual = db.Users.Insert(user);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("Marvin", actual.Name);
            ClassicAssert.AreEqual("diodes", actual.Password);
            ClassicAssert.AreEqual(42000000, actual.Age);
        }

        [Test]
        public void TestMultiInsertWithDynamicTypeObjects()
        {
            var db = DatabaseHelper.Open();

            dynamic user1 = new ExpandoObject();
            user1.Name = "Slartibartfast";
            user1.Password = "bistromathics";
            user1.Age = 777;

            dynamic user2 = new ExpandoObject();
            user2.Name = "Wowbagger";
            user2.Password = "teatime";
            user2.Age = int.MaxValue;

            var users = new[] { user1, user2 };

            IList<dynamic> actuals = db.Users.Insert(users).ToList();

            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreNotEqual(0, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreNotEqual(0, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestMultiInsertWithErrorCallback()
        {
            var db = DatabaseHelper.Open();

            dynamic user1 = new ExpandoObject();
            user1.Name = "Slartibartfast";
            user1.Password = "bistromathics";
            user1.Age = 777;

            dynamic user2 = new ExpandoObject();
            user2.Name = null;
            user2.Password = null;
            user2.Age = null;

            dynamic user3 = new ExpandoObject();
            user3.Name = "Wowbagger";
            user3.Password = "teatime";
            user3.Age = int.MaxValue;

            var users = new[] { user1, user2, user3 };
            bool passed = false;

            ErrorCallback onError = (o, exception) => passed = true;

            IList<dynamic> actuals = db.Users.Insert(users, onError).ToList();

            ClassicAssert.IsTrue(passed, "Callback was not called.");
            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreNotEqual(0, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreNotEqual(0, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestTransactionMultiInsertWithErrorCallback()
        {
            var db = DatabaseHelper.Open();
            IList<dynamic> actuals;
            bool passed = false;
            using (var tx = db.BeginTransaction())
            {
                dynamic user1 = new ExpandoObject();
                user1.Name = "Slartibartfast";
                user1.Password = "bistromathics";
                user1.Age = 777;

                dynamic user2 = new ExpandoObject();
                user2.Name = null;
                user2.Password = null;
                user2.Age = null;

                dynamic user3 = new ExpandoObject();
                user3.Name = "Wowbagger";
                user3.Password = "teatime";
                user3.Age = int.MaxValue;

                var users = new[] { user1, user2, user3 };

                ErrorCallback onError = (o, exception) => passed = true;

                actuals = db.Users.Insert(users, onError).ToList();

                tx.Commit();
            }

            ClassicAssert.IsTrue(passed, "Callback was not called.");
            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreNotEqual(0, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreNotEqual(0, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestWithImageColumn()
        {
            var db = DatabaseHelper.Open();
            try
            {
                var image = GetImage.Image;
                db.Images.Insert(Id: 1, TheImage: image);
                var img = (DbImage)db.Images.FindById(1);
                ClassicAssert.IsTrue(image.SequenceEqual(img.TheImage));
            }
            finally
            {
                db.Images.DeleteById(1);
            }
        }

        [Test]
        public void TestInsertWithVarBinaryMaxColumn()
        {
            var db = DatabaseHelper.Open();
            var image = GetImage.Image;
            var blob = new Blob
            {
                Id = 1,
                Data = image
            };
            db.Blobs.Insert(blob);
            blob = db.Blobs.FindById(1);
            ClassicAssert.IsTrue(image.SequenceEqual(blob.Data));
        }

        [Test]
        public void TestInsertWithTimestampColumn()
        {
            var db = DatabaseHelper.Open();
            var row = db.TimestampTest.Insert(Description: "Foo");
            ClassicAssert.IsNotNull(row);
            ClassicAssert.IsInstanceOf<byte[]>(row.Version);
        }

        [Test]
        public void TestInsertWithDateTimeOffsetColumn()
        {
            var db = DatabaseHelper.Open();
            dynamic entry = new ExpandoObject();
            var time = DateTimeOffset.Now;
            entry.time = time;
            var inserted = db.DateTimeOffsetTest.Insert(entry);
            ClassicAssert.AreEqual(time, inserted.time);
        }
    }
}
