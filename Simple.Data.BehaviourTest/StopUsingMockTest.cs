namespace Simple.Data.IntegrationTest
{
    using Ado;
    using Mocking.Ado;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class StopUsingMockTest : DatabaseIntegrationContext
    {
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
