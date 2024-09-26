namespace Simple.Data.IntegrationTest
{
    using Ado;
    using Mocking.Ado;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using System.Reflection;
    using System;

    [TestFixture]
    public class StopUsingMockTest : DatabaseIntegrationContext
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var testDllName = Assembly.GetAssembly(GetType())
                                 .GetName()
                                 .Name;
            var configName = testDllName + ".dll.config";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configName);
        }

        [Test]
        public void StopUsingMockAdapterStopsUsingMockAdapter()
        {
            var mock = new InMemoryAdapter();
            Database.UseMockAdapter(mock);
            Database db = Database.OpenNamedConnection("Mock");
            ClassicAssert.AreSame(mock, db.GetAdapter());
            Database.StopUsingMockAdapter();
            db = Database.OpenNamedConnection("Mock");
            ClassicAssert.IsInstanceOf<AdoAdapter>(db.GetAdapter());
        }

        protected override void SetSchema(MockSchemaProvider schemaProvider)
        {
        }
    }
}
