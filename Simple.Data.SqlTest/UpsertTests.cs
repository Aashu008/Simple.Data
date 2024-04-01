namespace Simple.Data.SqlTest
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Resources;

    [TestFixture]
    public class UpsertTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void TestUpsertWithNamedArgumentsAndExistingObject()
        {
            var db = DatabaseHelper.Open();

            db.Users.UpsertById(Id: 1, Name: "Ford Prefect");
            var user = db.Users.Get(1);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(1, user.Id);
            ClassicAssert.AreEqual("Ford Prefect", user.Name);
        }

        [Test]
        public void TestUpsertWithNamedArgumentsAndExistingObjectUsingTransaction()
        {
            using (var tx = DatabaseHelper.Open().BeginTransaction())
            {

                tx.Users.UpsertById(Id: 1, Name: "Ford Prefect");
                var user = tx.Users.Get(1);
                tx.Commit();

                ClassicAssert.IsNotNull(user);
                ClassicAssert.AreEqual(1, user.Id);
                ClassicAssert.AreEqual("Ford Prefect", user.Name);
            }
        }

        [Test]
        public void TestUpsertWithNamedArgumentsAndNewObject()
        {
            var db = DatabaseHelper.Open();

            var user = db.Users.UpsertById(Id: 0, Name: "Ford Prefect", Password: "Foo", Age: 42);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreNotEqual(0, user.Id);
            ClassicAssert.AreEqual("Ford Prefect", user.Name);
            ClassicAssert.AreEqual("Foo", user.Password);
            ClassicAssert.AreEqual(42, user.Age);
        }

        [Test]
        public void TestUpsertWithStaticTypeObject()
        {
            var db = DatabaseHelper.Open();

            var user = new User { Id = 2, Name = "Charlie", Password = "foobar", Age = 42 };

            var actual = db.Users.Upsert(user);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(2, actual.Id);
            ClassicAssert.AreEqual("Charlie", actual.Name);
            ClassicAssert.AreEqual("foobar", actual.Password);
            ClassicAssert.AreEqual(42, actual.Age);
        }

        [Test]
        public void TestUpsertByWithStaticTypeObject()
        {
            var db = DatabaseHelper.Open();

            var user = new User { Id = 2, Name = "Charlie", Password = "foobar", Age = 42 };

            var actual = db.Users.UpsertById(user);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual(2, actual.Id);
            ClassicAssert.AreEqual("Charlie", actual.Name);
            ClassicAssert.AreEqual("foobar", actual.Password);
            ClassicAssert.AreEqual(42, actual.Age);
        }

        [Test]
        public void TestMultiUpsertWithStaticTypeObjectsForExistingRecords()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Id = 1, Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Id = 2, Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            IList<User> actuals = db.Users.Upsert(users).ToList<User>();

            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreEqual(1, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreEqual(2, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestMultiUpsertWithStaticTypeObjectsForNewRecords()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            IList<User> actuals = db.Users.Upsert(users).ToList<User>();

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
        public void TestMultiUpsertWithStaticTypeObjectsForMixedRecords()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Id = 1, Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            IList<User> actuals = db.Users.Upsert(users).ToList<User>();

            ClassicAssert.AreEqual(2, actuals.Count);
            ClassicAssert.AreEqual(1, actuals[0].Id);
            ClassicAssert.AreEqual("Slartibartfast", actuals[0].Name);
            ClassicAssert.AreEqual("bistromathics", actuals[0].Password);
            ClassicAssert.AreEqual(777, actuals[0].Age);

            ClassicAssert.AreNotEqual(0, actuals[1].Id);
            ClassicAssert.AreEqual("Wowbagger", actuals[1].Name);
            ClassicAssert.AreEqual("teatime", actuals[1].Password);
            ClassicAssert.AreEqual(int.MaxValue, actuals[1].Age);
        }

        [Test]
        public void TestMultiUpsertWithStaticTypeObjectsAndNoReturn()
        {
            var db = DatabaseHelper.Open();

            var users = new[]
                            {
                                new User { Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

            //IList<User> actuals = db.Users.Upsert(users).ToList<User>();
            db.Users.Upsert(users);

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
        public void TestUpsertWithDynamicTypeObject()
        {
            var db = DatabaseHelper.Open();

            dynamic user = new ExpandoObject();
            user.Name = "Marvin";
            user.Password = "diodes";
            user.Age = 42000000;

            var actual = db.Users.Upsert(user);

            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("Marvin", actual.Name);
            ClassicAssert.AreEqual("diodes", actual.Password);
            ClassicAssert.AreEqual(42000000, actual.Age);
        }

        [Test]
        public void TestMultiUpsertWithDynamicTypeObjects()
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

            IList<dynamic> actuals = db.Users.Upsert(users).ToList();

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
        public void TestMultiUpsertWithErrorCallback()
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

            IList<dynamic> actuals = db.Users.Upsert(users, onError).ToList();

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
        public void TestMultiUpsertWithErrorCallbackUsingTransaction()
        {
            IList<dynamic> actuals;
            bool passed = false;
            using (var tx = DatabaseHelper.Open().BeginTransaction())
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

                actuals = tx.Users.Upsert(users, onError).ToList();
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
        public void TestTransactionMultiUpsertWithErrorCallback()
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

                actuals = db.Users.Upsert(users, onError).ToList();

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
                db.Images.Upsert(Id: 1, TheImage: image);
                var img = (DbImage)db.Images.FindById(1);
                ClassicAssert.IsTrue(image.SequenceEqual(img.TheImage));
            }
            finally
            {
                db.Images.DeleteById(1);
            }
        }

        [Test]
        public void TestUpsertWithVarBinaryMaxColumn()
        {
            var db = DatabaseHelper.Open();
            var image = GetImage.Image;
            var blob = new Blob
            {
                Id = 1,
                Data = image
            };
            db.Blobs.Upsert(blob);
            blob = db.Blobs.FindById(1);
            ClassicAssert.IsTrue(image.SequenceEqual(blob.Data));
        }

        [Test]
        public void TestUpsertWithSingleArgumentAndExistingObject()
        {
            var db = DatabaseHelper.Open();

            var actual = db.Users.UpsertById(Id: 1);

            ClassicAssert.IsNotNull(actual);
            ClassicAssert.AreEqual(1, actual.Id);
            ClassicAssert.IsNotNull(actual.Name);
        }

        [Test]
        public void TestUpsertUserBySecondaryField()
        {
            var db = DatabaseHelper.Open();

            var id = db.Users.UpsertByName(new User() { Age = 20, Name = "Black sheep", Password = "Bah" }).Id;
            User actual = db.Users.FindById(id);

            ClassicAssert.AreEqual(id, actual.Id);
            ClassicAssert.AreEqual("Black sheep", actual.Name);
            ClassicAssert.AreEqual("Bah", actual.Password);
            ClassicAssert.AreEqual(20, actual.Age);
        }

        [Test]
        public void TestUpsertUserByTwoSecondaryFields()
        {
            var db = DatabaseHelper.Open();

            var id = db.Users.UpsertByNameAndPassword(new User() { Age = 20, Name = "Black sheep", Password = "Bah" }).Id;
            User actual = db.Users.FindById(id);

            ClassicAssert.AreEqual(id, actual.Id);
            ClassicAssert.AreEqual("Black sheep", actual.Name);
            ClassicAssert.AreEqual("Bah", actual.Password);
            ClassicAssert.AreEqual(20, actual.Age);
        }

        [Test]
        public void TestUpsertExisting()
        {
            var db = DatabaseHelper.Open();

            var id = db.Users.UpsertByNameAndPassword(new User() { Age = 20, Name = "Black sheep", Password = "Bah" }).Id;
            db.Users.UpsertById(new User() { Id = id, Age = 12, Name = "Dog", Password = "Bark" });

            User actual = db.Users.FindById(id);

            ClassicAssert.AreEqual(id, actual.Id);
            ClassicAssert.AreEqual("Dog", actual.Name);
            ClassicAssert.AreEqual("Bark", actual.Password);
            ClassicAssert.AreEqual(12, actual.Age);
        }
    }
}