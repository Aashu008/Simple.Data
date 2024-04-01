using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.InMemoryTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class CanAssignArray
    {
        [Test]
        public void InsertAndGetWithArrayPropertyShouldWork()
        {
            var adapter = new InMemoryAdapter();
            adapter.SetKeyColumn("Test", "Id");
            Database.UseMockAdapter(adapter);
            var db = Database.Open();
            db.Test.Insert(Id: 1, Names: new List<string> { "Alice", "Bob", "Charlie" });
            People record = db.Test.Get(1);
            ClassicAssert.IsNotNull(record);
            ClassicAssert.AreEqual(1, record.Id);
            ClassicAssert.AreEqual("Alice", record.Names[0]);
            ClassicAssert.AreEqual("Bob", record.Names[1]);
            ClassicAssert.AreEqual("Charlie", record.Names[2]);
        }
    }

    class People
    {
        public int Id { get; set; }
        public string[] Names { get; set; }
    }
}
