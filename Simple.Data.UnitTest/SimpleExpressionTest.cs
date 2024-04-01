using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Simple.Data.UnitTest
{
    [TestFixture]
    public class SimpleExpressionTest
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void AndOperatorCombinesTwoExpressions()
        {
            // Arrange
            var expr1 = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            var expr2 = new SimpleExpression(2, 2, SimpleExpressionType.Equal);

            // Act
            var newExpr = expr1 && expr2;

            // Assert
            ClassicAssert.AreEqual(expr1, newExpr.LeftOperand);
            ClassicAssert.AreEqual(expr2, newExpr.RightOperand);
            ClassicAssert.AreEqual(SimpleExpressionType.And, newExpr.Type);
        }

        [Test]
        public void OrOperatorCombinesTwoExpressions()
        {
            // Arrange
            var expr1 = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            var expr2 = new SimpleExpression(2, 2, SimpleExpressionType.Equal);

            // Act
            var newExpr = expr1 || expr2;

            // Assert
            ClassicAssert.AreEqual(expr1, newExpr.LeftOperand);
            ClassicAssert.AreEqual(expr2, newExpr.RightOperand);
            ClassicAssert.AreEqual(SimpleExpressionType.Or, newExpr.Type);
        }

        [Test]
        public void CompoundOperatorsRespectParentheses_AndOrAnd()
        {
            // Arrange
            var expr1 = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            var expr2 = new SimpleExpression(2, 2, SimpleExpressionType.Equal);
            var expr3 = new SimpleExpression(3, 3, SimpleExpressionType.Equal);
            var expr4 = new SimpleExpression(4, 4, SimpleExpressionType.Equal);

            // Act
            var newExpr = (expr1 && expr2) || (expr3 && expr4);
            var leftExpr = newExpr.LeftOperand as SimpleExpression;
            var rightExpr = newExpr.RightOperand as SimpleExpression;

            // Assert
            ClassicAssert.IsNotNull(leftExpr);
            ClassicAssert.IsNotNull(rightExpr);
            ClassicAssert.AreEqual(newExpr.Type, SimpleExpressionType.Or);
            ClassicAssert.AreEqual(expr1, leftExpr.LeftOperand);
            ClassicAssert.AreEqual(expr2, leftExpr.RightOperand);
            ClassicAssert.AreEqual(expr3, rightExpr.LeftOperand);
            ClassicAssert.AreEqual(expr4, rightExpr.RightOperand);
        }


        [Test]
        public void CompoundExpressionsEvaluateAndOperatorsFirst()
        {
            // Arrange
            var expr1 = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            var expr2 = new SimpleExpression(2, 2, SimpleExpressionType.Equal);
            var expr3 = new SimpleExpression(3, 3, SimpleExpressionType.Equal);
            var expr4 = new SimpleExpression(4, 4, SimpleExpressionType.Equal);

            // Act
            var newExpr = expr1 && expr2 || expr3 && expr4;
            var leftExpr = newExpr.LeftOperand as SimpleExpression;
            var rightExpr = newExpr.RightOperand as SimpleExpression;

            // Assert
            ClassicAssert.IsNotNull(leftExpr);
            ClassicAssert.IsNotNull(rightExpr);
            ClassicAssert.AreEqual(newExpr.Type, SimpleExpressionType.Or);
            ClassicAssert.AreEqual(expr1, leftExpr.LeftOperand);
            ClassicAssert.AreEqual(expr2, leftExpr.RightOperand);
            ClassicAssert.AreEqual(expr3, rightExpr.LeftOperand);
            ClassicAssert.AreEqual(expr4, rightExpr.RightOperand);
        }

        private static void CompoundExpressionEvaluationOrderHelper(Func<SimpleExpression, SimpleExpression, SimpleExpression, SimpleExpression, SimpleExpression> actor)
        {
            // Arrange
            var expr1 = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            var expr2 = new SimpleExpression(2, 2, SimpleExpressionType.Equal);
            var expr3 = new SimpleExpression(3, 3, SimpleExpressionType.Equal);
            var expr4 = new SimpleExpression(4, 4, SimpleExpressionType.Equal);

            // Act
            var actual = actor(expr1, expr2, expr3, expr4);

            // Assert
            ClassicAssert.AreEqual(expr4, GetExpression(actual, 0).RightOperand);
            ClassicAssert.AreEqual(expr3, GetExpression(actual, 1).RightOperand);
            ClassicAssert.AreEqual(expr2, GetExpression(actual, 2).RightOperand);
            ClassicAssert.AreEqual(expr1, GetExpression(actual, 2).LeftOperand);
        }

        /// <summary>
        /// Checks the order in which expressions are combined.
        /// Where the expression is A & B & C & D, the grouping should be (((A & B) & C) & D)
        /// </summary>
        [Test]
        public void CompoundExpressionsEvaluateRightToLeft_AndAndAnd()
        {
            CompoundExpressionEvaluationOrderHelper((e1, e2, e3, e4) => e1 & e2 & e3 & e4);
        }

        /// <summary>
        /// Checks the order in which expressions are combined.
        /// Where the expression is A | B | C | D, the grouping should be (((A | B) | C) | D)
        /// </summary>
        [Test]
        public void CompoundExpressionsEvaluateRightToLeft_OrOrOr()
        {
            CompoundExpressionEvaluationOrderHelper((e1, e2, e3, e4) => e1 | e2 | e3 | e4);
        }

        private static SimpleExpression GetExpression(SimpleExpression expression, int nestLevel)
        {
            for (int i = 0; i < nestLevel; i++)
            {
                expression = (SimpleExpression)expression.LeftOperand;
            }
            return expression;
        }
        // ReSharper restore InconsistentNaming
    }
}
