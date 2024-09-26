using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.SqlTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using System.Reflection;

    [TestFixture]
    public class WeirdTypeTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
            var testDllName = Assembly.GetAssembly(GetType())
                                 .GetName()
                                 .Name;
            var configName = testDllName + ".dll.config";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configName);
        }

        [Test]
        public void TestInsertOnGeography()
        {
            var db = DatabaseHelper.Open();
            var actual = db.GeographyTest.Insert(Description: "Test");
            ClassicAssert.IsNotNull(actual);
        }
        [Test]
        public void TestInsertOnGeometry()
        {
            var db = DatabaseHelper.Open();
            var actual = db.GeometryTest.Insert(Description: "Test");
            ClassicAssert.IsNotNull(actual);
        }
        [Test]
        public void TestInsertOnHierarchyId()
        {
            var db = DatabaseHelper.Open();
            var actual = db.HierarchyIdTest.Insert(Description: "Test");
            ClassicAssert.IsNotNull(actual);
        }
    }
}
