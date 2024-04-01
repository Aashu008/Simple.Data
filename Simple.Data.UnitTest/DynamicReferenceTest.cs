using Simple.Data;
using NUnit.Framework;
using System;
using NUnit.Framework.Legacy;

namespace Simple.Data.UnitTest
{


    /// <summary>
    ///This is a test class for DynamicTableOrColumnTest and is intended
    ///to contain all DynamicTableOrColumnTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class DynamicReferenceTest
    {
        [Test]
        public void GetDynamicPropertyReturnsNewDynamicReferenceWithTableAndColumn()
        {
            // Arrange
            dynamic table = new ObjectReference("Table");

            // Act
            ObjectReference column = table.Column;

            // Assert
            ClassicAssert.AreEqual("Column", column.GetName());
            ClassicAssert.AreEqual("Table", column.GetOwner().GetName());
        }

        [Test]
        public void GetDynamicPropertyDotPropertyReturnsNewDynamicReferenceWithTwoOwners()
        {
            // Arrange
            dynamic table = new ObjectReference("Table1");

            // Act
            ObjectReference column = table.Table2.Column;

            // Assert
            ClassicAssert.AreEqual("Column", column.GetName());
            ClassicAssert.AreEqual("Table2", column.GetOwner().GetName());
            ClassicAssert.AreEqual("Table1", column.GetOwner().GetOwner().GetName());
        }

        /// <summary>
        ///A test for GetAllObjectNames
        ///</summary>
        [Test()]
        public void GetAllObjectNamesTest()
        {
            // Arrange
            dynamic table = new DynamicTable("One", null);
            var column = table.Two.Column;

            // Act
            string[] names = column.GetAllObjectNames();

            // Assert
            ClassicAssert.AreEqual("One", names[0]);
            ClassicAssert.AreEqual("Two", names[1]);
            ClassicAssert.AreEqual("Column", names[2]);
        }

        [Test]
        public void FromStringTest()
        {
            // Act
            var actual = ObjectReference.FromString("One.Two.Three");

            // Assert
            ClassicAssert.AreEqual("Three", actual.GetName());
            ClassicAssert.AreEqual("Two", actual.GetOwner().GetName());
            ClassicAssert.AreEqual("One", actual.GetOwner().GetOwner().GetName());
            ClassicAssert.IsNull(actual.GetOwner().GetOwner().GetOwner());
        }

        private static void DoAsserts<T>(SimpleExpression expression, ObjectReference column, T rightOperand, SimpleExpressionType expressionType)
        {
            ClassicAssert.AreEqual(column, expression.LeftOperand);
            ClassicAssert.AreEqual(rightOperand, expression.RightOperand);
            ClassicAssert.AreEqual(expressionType, expression.Type);
        }

        [Test]
        public void EqualOperatorReturnsSimpleExpressionWithEqualType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column == 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.Equal);
        }

        [Test]
        public void EqualOperatorReturnsSimpleExpressionWithEqualTypeWhenUsedAsDynamic()
        {
            // Arrange
            dynamic column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column == 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.Equal);
        }

        [Test]
        public void NotEqualOperatorReturnsSimpleExpressionWithNotEqualType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column != 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.NotEqual);
        }

        [Test]
        public void LessThanOperatorReturnsSimpleExpressionWithLessThanType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column < 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.LessThan);
        }

        [Test]
        public void LessThanOrEqualOperatorReturnsSimpleExpressionWithLessThanOrEqualType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column <= 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.LessThanOrEqual);
        }

        [Test]
        public void GreaterThanOperatorReturnsSimpleExpressionWithGreaterThanType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column > 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.GreaterThan);
        }

        [Test]
        public void GreaterThanOrEqualOperatorReturnsSimpleExpressionWithGreaterThanOrEqualType()
        {
            // Arrange
            var column = ObjectReference.FromStrings("foo", "bar");

            // Act
            var expression = column >= 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.GreaterThanOrEqual);
        }
    }
}
