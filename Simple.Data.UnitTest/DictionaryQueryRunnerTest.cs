using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using QueryPolyfills;

    [TestFixture]
    public class DictionaryQueryRunnerTest
    {
        [Test]
        public void DistinctShouldRemoveDuplicateRows()
        {
            var runner = new DictionaryQueryRunner("FooTable", DuplicatingSource(), new DistinctClause());
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(1, actual.Count(d => d.ContainsKey("Foo") && (string)d["Foo"] == "bar"));
            ClassicAssert.AreEqual(1, actual.Count(d => d.ContainsKey("Quux") && (string)d["Quux"] == "baz"));
        }

        [Test]
        public void ShouldNotRemoveDistinctRows()
        {
            var runner = new DictionaryQueryRunner("FooTable", NonDuplicatingSource(), new DistinctClause());
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(1, actual.Count(d => d.ContainsKey("Foo") && (string)d["Foo"] == "bar"));
            ClassicAssert.AreEqual(1, actual.Count(d => d.ContainsKey("Quux") && (string)d["Quux"] == "baz"));
        }

        [Test]
        public void SkipShouldSkip()
        {
            var runner = new DictionaryQueryRunner("FooTable", SkipTakeSource(), new SkipClause(1));
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(1, actual[0]["Row"]);
            ClassicAssert.AreEqual(2, actual[1]["Row"]);
        }

        [Test]
        public void TakeShouldTake()
        {
            var runner = new DictionaryQueryRunner("FooTable", SkipTakeSource(), new TakeClause(2));
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(0, actual[0]["Row"]);
            ClassicAssert.AreEqual(1, actual[1]["Row"]);
        }

        [Test]
        public void SkipAndTakeShouldSkipAndTake()
        {
            var runner = new DictionaryQueryRunner("FooTable", SkipTakeSource(), new SkipClause(1), new TakeClause(1));
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual(1, actual[0]["Row"]);
        }

        [Test]
        public void SkipAndTakeWithCountShouldSkipAndTakeAndGiveCount()
        {
            int count = 0;
            var runner = new DictionaryQueryRunner("FooTable", SkipTakeSource(), new WithCountClause(n => count = n), new SkipClause(1), new TakeClause(1));
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(3, count);
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual(1, actual[0]["Row"]);
        }

        [Test]
        public void SelectShouldRestrictColumnList()
        {
            var tableRef = new ObjectReference("FooTable");
            var selectClause = new SelectClause(new SimpleReference[] { new ObjectReference("Id", tableRef), new ObjectReference("Name", tableRef) });
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), selectClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(4, actual.Count);
            ClassicAssert.AreEqual(2, actual[0].Count);
            ClassicAssert.AreEqual(1, actual[0]["Id"]);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
            ClassicAssert.AreEqual(2, actual[1].Count);
            ClassicAssert.AreEqual(2, actual[1]["Id"]);
            ClassicAssert.AreEqual("Bob", actual[1]["Name"]);
            ClassicAssert.AreEqual(2, actual[2].Count);
            ClassicAssert.AreEqual(3, actual[2]["Id"]);
            ClassicAssert.AreEqual("Charlie", actual[2]["Name"]);
            ClassicAssert.AreEqual(2, actual[3].Count);
            ClassicAssert.AreEqual(4, actual[3]["Id"]);
            ClassicAssert.AreEqual("David", actual[3]["Name"]);
        }

        [Test]
        public void SelectLengthShouldUseLengthFunction()
        {
            var tableRef = new ObjectReference("FooTable");
            var function = new FunctionReference("Length", new ObjectReference("Name", tableRef)).As("NameLength");
            var selectClause = new SelectClause(new SimpleReference[] { new ObjectReference("Name", tableRef), function });
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), selectClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(4, actual.Count);
            ClassicAssert.AreEqual(2, actual[0].Count);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
            ClassicAssert.AreEqual(5, actual[0]["NameLength"]);
            ClassicAssert.AreEqual(2, actual[1].Count);
            ClassicAssert.AreEqual("Bob", actual[1]["Name"]);
            ClassicAssert.AreEqual(3, actual[1]["NameLength"]);
            ClassicAssert.AreEqual(2, actual[2].Count);
            ClassicAssert.AreEqual("Charlie", actual[2]["Name"]);
            ClassicAssert.AreEqual(7, actual[2]["NameLength"]);
            ClassicAssert.AreEqual(2, actual[3].Count);
            ClassicAssert.AreEqual("David", actual[3]["Name"]);
            ClassicAssert.AreEqual(5, actual[3]["NameLength"]);
        }

        [Test]
        public void BasicWhereEqualShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Name", tableRef) == "Alice");
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
        }

        [Test]
        public void WhereNullShouldWorkWhenValueExistsAndIsNull()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Value", tableRef) == null);
            var data = new List<IDictionary<string, object>>
                           {
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Steve"}, { "Value", null }
                                   },
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Dave"}, { "Value", 42 }
                                   },
                           };
            var runner = new DictionaryQueryRunner("FooTable", data, whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Steve", actual[0]["Name"]);
        }

        [Test]
        public void WhereNullShouldWorkWhenValueDoesNotExist()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Value", tableRef) == null);
            var data = new List<IDictionary<string, object>>
                           {
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Steve"}
                                   },
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Dave"}, { "Value", 42 }
                                   },
                           };
            var runner = new DictionaryQueryRunner("FooTable", data, whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Steve", actual[0]["Name"]);
        }

        [Test]
        public void WhereEqualWithByteArrayShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Array", tableRef) == new byte[] { 1, 2, 3, 4 });
            var data = new List<IDictionary<string, object>>
                           {
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Steve"}, { "Array", new byte[] { 1, 2, 3, 4}}
                                   },
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Dave"}, { "Array", new byte[] { 2, 3, 4}}
                                   },
                           };
            var runner = new DictionaryQueryRunner("FooTable", data, whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Steve", actual[0]["Name"]);
        }

        [Test]
        public void BasicWhereNotEqualShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Name", tableRef) != "Alice");
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(3, actual.Count);
            ClassicAssert.False(actual.Any(a => (string)a["Name"] == "Alice"));
        }

        [Test]
        public void WhereNotNullShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Value", tableRef) != null);
            var data = new List<IDictionary<string, object>>
                           {
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Steve"}, { "Value", null }
                                   },
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Dave"}, { "Value", 42 }
                                   },
                           };
            var runner = new DictionaryQueryRunner("FooTable", data, whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Dave", actual[0]["Name"]);
        }

        [Test]
        public void WhereNotEqualWithByteArrayShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Array", tableRef) != new byte[] { 1, 2, 3, 4 });
            var data = new List<IDictionary<string, object>>
                           {
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Steve"}, { "Array", new byte[] { 1, 2, 3, 4}}
                                   },
                               new Dictionary<string, object>
                                   {
                                       {"Name", "Dave"}, { "Array", new byte[] { 2, 3, 4}}
                                   },
                           };
            var runner = new DictionaryQueryRunner("FooTable", data, whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Dave", actual[0]["Name"]);
        }

        [Test]
        public void BasicWhereGreaterThanShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Weight", tableRef) > 200M);
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("David", actual[0]["Name"]);
        }

        [Test]
        public void BasicWhereLessThanShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Weight", tableRef) < 150M);
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
        }

        [Test]
        public void BasicWhereGreaterThanOrEqualShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Weight", tableRef) >= 250M);
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("David", actual[0]["Name"]);
        }

        [Test]
        public void BasicWhereLessThanOrEqualShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            var whereClause = new WhereClause(new ObjectReference("Weight", tableRef) <= 100M);
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
        }

        [Test]
        public void BasicLikeShouldWork()
        {
            var tableRef = new ObjectReference("FooTable");
            dynamic objRef = new ObjectReference("Name", tableRef);
            var expression = new SimpleExpression(objRef, new SimpleFunction("like", new[] { "A%" }), SimpleExpressionType.Function);
            var whereClause = new WhereClause(expression);
            var runner = new DictionaryQueryRunner("FooTable", SelectSource(), whereClause);
            var actual = runner.Run().ToList();
            ClassicAssert.AreEqual(1, actual.Count);
            ClassicAssert.AreEqual("Alice", actual[0]["Name"]);
        }

        #region Distinct sources

        private static IEnumerable<IDictionary<string, object>> DuplicatingSource()
        {
            yield return new Dictionary<string, object>
                             {
                                 { "Foo", "bar" },
                             };
            yield return new Dictionary<string, object>
                             {
                                 { "Quux", "baz" },
                             };
            yield return new Dictionary<string, object>
                             {
                                 { "Quux", "baz" },
                             };
        }

        private static IEnumerable<IDictionary<string, object>> NonDuplicatingSource()
        {
            yield return new Dictionary<string, object>
                             {
                                 { "Foo", "bar" },
                             };
            yield return new Dictionary<string, object>
                             {
                                 { "Quux", "baz" },
                             };
        }

        #endregion

        #region Skip/Take/WithCount sources

        private static IEnumerable<IDictionary<string, object>> SkipTakeSource()
        {
            yield return new Dictionary<string, object>
                             {
                                 { "Row", 0 },
                             };
            yield return new Dictionary<string, object>
                             {
                                 { "Row", 1 },
                             };
            yield return new Dictionary<string, object>
                             {
                                 { "Row", 2 }
                             };
        }

        #endregion

        private static IEnumerable<IDictionary<string, object>> SelectSource()
        {
            yield return new Dictionary<string, object>
                             {
                                {"Id", 1}, { "Type", "A"}, {"Name","Alice"}, {"Weight", 100M}
                             };
            yield return new Dictionary<string, object>
                             {
                                {"Id", 2}, { "Type", "A"}, {"Name","Bob"}, {"Weight", 150M}
                             };
            yield return new Dictionary<string, object>
                             {
                                {"Id", 3}, { "Type", "B"}, {"Name","Charlie"}, {"Weight", 200M}
                             };
            yield return new Dictionary<string, object>
                             {
                                {"Id", 4}, { "Type", "B"}, {"Name","David"}, {"Weight", 250M}
                             };
        }
    }
}
