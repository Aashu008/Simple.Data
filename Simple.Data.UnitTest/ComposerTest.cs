namespace Simple.Data.UnitTest
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ComposerTest
    {
        [Test]
        public void SetDefaultComposerWorks()
        {
            var stub = new StubComposer();
            Composer.SetDefault(stub);
            ClassicAssert.AreSame(stub, Composer.Default);
        }
    }
}
