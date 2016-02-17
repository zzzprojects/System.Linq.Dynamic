using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class DynamicExpressionTests
    {
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
    }
}
