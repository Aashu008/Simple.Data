using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.UnitTest
{
    [TestFixture]
    public class DynamicTableTest
    {
        [Test]
        public void PropertyShouldReturnDynamicReference()
        {
            //Arrange
            dynamic table = new DynamicTable("Test", null);

            // Act
            ObjectReference column = table.TestColumn;

            // Assert
            ClassicAssert.AreEqual("Test", column.GetOwner().GetName());
            ClassicAssert.AreEqual("TestColumn", column.GetName());
        }
    }
}
