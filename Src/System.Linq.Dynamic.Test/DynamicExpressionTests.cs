using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
