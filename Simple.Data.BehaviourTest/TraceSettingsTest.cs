using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.IntegrationTest
{
    [TestFixture]
    public class TraceSettingsTest
    {
        [Test]
        public void TraceLevelShouldBePickedUpFromConfig()
        {
            ClassicAssert.AreEqual(TraceLevel.Error, Database.TraceLevel);
        }

        [Test]
        public void TraceLevelShouldBeSettableFromCode()
        {
            Database.TraceLevel = TraceLevel.Off;
            ClassicAssert.AreEqual(TraceLevel.Off, Database.TraceLevel);

        }
    }
}
