namespace Simple.Data.UnitTest
{
    using System.Collections.Generic;
    using System.Linq;
    using Ado;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class EagerLoadingEnumerableTest
    {
        [Test]
        public void GroupsObjectAsChildItem()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"foo", "Foo1"},
                               {"__with1__bar__quux", "Quux1"}
                           };

            var test = new EagerLoadingEnumerable(new[] { dict }).ToList();
            ClassicAssert.AreEqual(1, test.Count);
            var actual = test[0];

            ClassicAssert.AreEqual("Foo1", actual["foo"]);
            var bar = actual["bar"] as IDictionary<string, object>;
            ClassicAssert.NotNull(bar);
            ClassicAssert.AreEqual("Quux1", bar["quux"]);
        }

        [Test]
        public void GroupsMultipleObjectsAsChildItems()
        {
            var dicts = new[]
                            {
                                new Dictionary<string, object>
                                    {
                                        {"foo", "Foo1"},
                                        {"__with1__bar__quux", "Quux1"},
                                        {"__with1__wibble__wobble", "Wobble1"}
                                    },
                            };

            var test = new EagerLoadingEnumerable(dicts).ToList();
            ClassicAssert.AreEqual(1, test.Count);
            var actual = test[0];

            ClassicAssert.AreEqual("Foo1", actual["foo"]);
            var bar = actual["bar"] as IDictionary<string, object>;
            ClassicAssert.NotNull(bar);
            ClassicAssert.AreEqual("Quux1", bar["quux"]);
            var wibble = actual["wibble"] as IDictionary<string, object>;
            ClassicAssert.NotNull(wibble);
            ClassicAssert.AreEqual("Wobble1", wibble["wobble"]);
        }

        [Test]
        public void GroupsMultipleObjectsAsChildItemLists()
        {
            var dicts = new[]
                            {
                                new Dictionary<string, object>
                                    {
                                        {"foo", "Foo1"},
                                        {"__withn__bar__quux", "Quux1"},
                                        {"__withn__wibble__wobble", "Wobble1"}
                                    },
                                new Dictionary<string, object>
                                    {
                                        {"foo", "Foo1"},
                                        {"__withn__bar__quux", "Quux1"},
                                        {"__withn__wibble__wobble", "Wobble2"}
                                    },
                                new Dictionary<string, object>
                                    {
                                        {"foo", "Foo1"},
                                        {"__withn__bar__quux", "Quux2"},
                                        {"__withn__wibble__wobble", "Wobble1"}
                                    },
                                new Dictionary<string, object>
                                    {
                                        {"foo", "Foo1"},
                                        {"__withn__bar__quux", "Quux2"},
                                        {"__withn__wibble__wobble", "Wobble2"}
                                    },
                            };

            var test = new EagerLoadingEnumerable(dicts).ToList();
            ClassicAssert.AreEqual(1, test.Count);
            var actual = test[0];

            ClassicAssert.AreEqual("Foo1", actual["foo"]);
            var bar = actual["bar"] as IList<IDictionary<string, object>>;
            ClassicAssert.NotNull(bar);
            ClassicAssert.AreEqual("Quux1", bar[0]["quux"]);
            ClassicAssert.AreEqual("Quux2", bar[1]["quux"]);
            var wibble = actual["wibble"] as IList<IDictionary<string, object>>;
            ClassicAssert.NotNull(wibble);
            ClassicAssert.AreEqual("Wobble1", wibble[0]["wobble"]);
            ClassicAssert.AreEqual("Wobble2", wibble[1]["wobble"]);
        }
    }
}