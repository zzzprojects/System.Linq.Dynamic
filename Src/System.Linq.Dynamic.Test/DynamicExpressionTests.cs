using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class DynamicExpressionTests
    {
        [TestMethod]
        public void Parse_ParameterExpressionMethodCall_ReturnsIntExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(int), "x") },
                typeof(int),
                "x + 1");
            Assert.AreEqual(typeof(int), expression.Type);
        }

        [TestMethod]
        public void Parse_TupleToStringMethodCall_ReturnsStringLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(Tuple<int>),
                typeof(string),
                "it.ToString()");
            Assert.AreEqual(typeof(string), expression.ReturnType);
        }

        [TestMethod]
        public void Parse_StringLiteral_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(new[] { Expression.Parameter(typeof(string), "Property1") }, typeof(Boolean), "Property1 == \"test\"");
            Assert.AreEqual(typeof(Boolean), expression.Type);
        }

        [TestMethod]
        public void Parse_StringLiteralEmpty_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(new[] { Expression.Parameter(typeof(string), "Property1") }, typeof(Boolean), "Property1 == \"\"");
            Assert.AreEqual(typeof(Boolean), expression.Type);
        }

        [TestMethod]
        public void Parse_StringLiteralEmbeddedQuote_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") }, 
                typeof(Boolean), 
                string.Format("Property1 == {0}", "\"test \\\"string\""));

            string rightValue = ((BinaryExpression) expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual("\"test \"string\"", rightValue);
        }

        [TestMethod]
        public void Parse_StringLiteralStartEmbeddedQuote_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", "\"\\\"test\""));

            string rightValue = ((BinaryExpression)expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual("\"\"test\"", rightValue);
        }

        [ExpectedException(typeof(ParseException))]
        [TestMethod]
        public void Parse_StringLiteral_MissingClosingQuote()
        {
            string expectedRightValue = "\"test\\\"";
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", expectedRightValue));
        }

        [TestMethod]
        public void Parse_StringLiteralEscapedBackslash_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", "\"test\\\\string\""));

            string rightValue = ((BinaryExpression)expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual("\"test\\string\"", rightValue);
        }

        [TestMethod]
        public void ParseLambda_DelegateTypeMethodCall_ReturnsEventHandlerLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(EventHandler),
                new[] { Expression.Parameter(typeof(object), "sender"),
                        Expression.Parameter(typeof(EventArgs), "e") },
                null,
                "sender.ToString()");

            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(EventHandler), expression.Type);
        }

        [TestMethod]
        public void ParseLambda_VoidMethodCall_ReturnsActionDelegate()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(System.IO.FileStream),
                null,
                "it.Close()");
            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(Action<System.IO.FileStream>), expression.Type);
        }

        [TestMethod]
        public void CreateClass_TheadSafe()
        {
            const int numOfTasks = 15;

            var properties = new[] { new DynamicProperty("prop1", typeof(string)) };

            var tasks = new List<Task>(numOfTasks);

            for (var i = 0; i < numOfTasks; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => DynamicExpression.CreateClass(properties)));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
