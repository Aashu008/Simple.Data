using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Simple.Data.Ado;
using Simple.Data.Mocking.Ado;

namespace Simple.Data.IntegrationTest
{
    [TestFixture]
    public class SchemaQualifiedTableTest
    {
        static Database CreateDatabase(MockDatabase mockDatabase)
        {
            var mockSchemaProvider = new MockSchemaProvider();
            mockSchemaProvider.SetTables(new[] { "foo", "Users", "BASE TABLE" },
                                         new[] { "foo.bar", "Test", "BASE TABLE" });
            mockSchemaProvider.SetColumns(new[] { "foo", "Users", "Id" },
                                          new[] { "foo", "Users", "Name" },
                                          new[] { "foo", "Users", "Password" },
                                          new[] { "foo", "Users", "Age" },
                                          new[] { "foo.bar", "Test", "Id" },
                                          new[] { "foo.bar", "Test", "Value" });
            mockSchemaProvider.SetPrimaryKeys(new object[] { "foo", "Users", "Id", 0 });
            return new Database(new AdoAdapter(new MockConnectionProvider(new MockDbConnection(mockDatabase), mockSchemaProvider)));
        }

        private const string usersColumns = "[foo].[users].[id], [foo].[users].[name], [foo].[users].[password], [foo].[users].[age]";

        [Test]
        public void TestFindEqualWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id == 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[users] where [foo].[users].[id] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindNotEqualWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id != 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[Id] != @p1".ToLowerInvariant().ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindGreaterThanWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id > 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[Id] > @p1".ToLowerInvariant().ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindGreaterThanOrEqualWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id >= 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[Id] >= @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindLessThanWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id < 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[Id] < @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindLessThanOrEqualWithInt32()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Find(database.foo.Users.Id <= 1);
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[Id] <= @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindByDynamicSingleColumn()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.FindByName("Foo");
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where [foo].[Users].[name] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Foo", mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestFindByDynamicTwoColumns()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.FindByNameAndPassword("Foo", "secret");
            ClassicAssert.AreEqual("select " + usersColumns + " from [foo].[Users] where ([foo].[Users].[name] = @p1 and [foo].[Users].[password] = @p2)".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Foo", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual("secret", mockDatabase.Parameters[1]);
        }

        [Test]
        public void TestFindAllByDynamic()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.FindAllByName("Foo").ToList();
            ClassicAssert.AreEqual("select [foo].[Users].[Id],[foo].[Users].[Name],[foo].[Users].[Password],[foo].[Users].[Age] from [foo].[Users] where [foo].[Users].[name] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Foo", mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestInsertWithNamedArguments()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Insert(Name: "Steve", Age: 50);
            ClassicAssert.AreEqual("insert into [foo].[Users] ([Name],[Age]) values (@p0,@p1)".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[1]);
        }

        [Test]
        public void TestUpdateWithNamedArguments()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.UpdateById(Id: 1, Name: "Steve", Age: 50);
            ClassicAssert.AreEqual("update [foo].[Users] set [Name] = @p1, [Age] = @p2 where [foo].[Users].[Id] = @p3".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[1]);
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[2]);
        }

        [Test]
        public void TestUpdateWithDynamicObject()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            dynamic record = new SimpleRecord();
            record.Id = 1;
            record.Name = "Steve";
            record.Age = 50;
            database.foo.Users.Update(record);
            ClassicAssert.AreEqual("update [foo].[Users] set [Name] = @p1, [Age] = @p2 where [foo].[Users].[Id] = @p3".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[1]);
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[2]);
        }

        [Test]
        public void TestUpdateByWithDynamicObject()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            dynamic record = new SimpleRecord();
            record.Id = 1;
            record.Name = "Steve";
            record.Age = 50;
            database.foo.Users.UpdateById(record);
            ClassicAssert.AreEqual("update [foo].[Users] set [Name] = @p1, [Age] = @p2 where [foo].[Users].[Id] = @p3".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[1]);
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[2]);
        }

        [Test]
        public void TestUpdateWithStaticObject()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            var user = new User
            {
                Id = 1,
                Name = "Steve",
                Age = 50
            };
            database.foo.Users.Update(user);
            ClassicAssert.AreEqual("update [foo].[Users] set [Name] = @p1, [Password] = @p2, [Age] = @p3 where [foo].[Users].[Id] = @p4".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(DBNull.Value, mockDatabase.Parameters[1]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[2]);
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[3]);
        }

        [Test]
        public void TestUpdateByWithStaticObject()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            var user = new User
            {
                Id = 1,
                Name = "Steve",
                Age = 50
            };
            database.foo.Users.UpdateById(user);
            ClassicAssert.AreEqual("update [foo].[Users] set [Name] = @p1, [Password] = @p2, [Age] = @p3 where [foo].[Users].[Id] = @p4".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Steve", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(DBNull.Value, mockDatabase.Parameters[1]);
            ClassicAssert.AreEqual(50, mockDatabase.Parameters[2]);
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[3]);
        }

        [Test]
        public void TestDeleteWithNamedArguments()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.Delete(Id: 1);
            ClassicAssert.AreEqual("delete from [foo].[Users] where [foo].[Users].[Id] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestDeleteBy()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foo.Users.DeleteById(1);
            ClassicAssert.AreEqual("delete from [foo].[Users] where [foo].[Users].[Id] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }

        [Test]
        public void TestInsertOnTable()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            dynamic person = new ExpandoObject();
            person.Name = "Phil";
            person.Age = 42;
            database.foo.Users.Insert(person);
            ClassicAssert.AreEqual("insert into [foo].[Users] ([Name],[Age]) values (@p0,@p1)".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual("Phil", mockDatabase.Parameters[0]);
            ClassicAssert.AreEqual(42, mockDatabase.Parameters[1]);
        }

        [Test]
        public void TestFindWithDotInSchemaName()
        {
            var mockDatabase = new MockDatabase();
            dynamic database = CreateDatabase(mockDatabase);
            database.foobar.Test.Find(database.foobar.Test.Id == 1);
            ClassicAssert.AreEqual("select [foo.bar].[Test].[Id], [foo.bar].[Test].[Value] from [foo.bar].[Test] where [foo.bar].[Test].[id] = @p1".ToLowerInvariant(), mockDatabase.Sql.ToLowerInvariant());
            ClassicAssert.AreEqual(1, mockDatabase.Parameters[0]);
        }
    }
}
