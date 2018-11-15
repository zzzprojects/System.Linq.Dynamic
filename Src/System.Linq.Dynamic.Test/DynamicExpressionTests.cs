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
        public void ParseLambda_NewArrayInit()
        {
            DateTime dt = DateTime.Now;
            
            var expr = DynamicExpression.ParseLambda(
                typeof(Func<DateTime, object[]>),
                new []
                {
                    Expression.Parameter(typeof(DateTime), "dt")
                },
                null,
                "array(dt.Year, dt.Month, 15, \"ala ma kota\", dt.Ticks, dt.Date.Ticks)"
                );


            Assert.AreEqual(typeof(object[]), expr.ReturnType);
            Assert.AreEqual(typeof(Func<DateTime, object[]>), expr.Type);
            var texpr = (Expression<Func<DateTime, object[]>>)expr;
            var fun = texpr.Compile();
            var res = fun(dt);
        }

        [TestMethod]
        public void ParseLambda_NewDictionary()
        {
            //this does not work - didnt figure oout how to make useful lambda with dictionary initialization
            DateTime dt = DateTime.Now;

            var expr = DynamicExpression.ParseLambda(
                typeof(Func<DateTime, Dictionary<string, object>>),
                new[]
                {
                    Expression.Parameter(typeof(DateTime), "dt")
                },
                typeof(Dictionary<string, object>),
                "dictionary(dt.Year as y, dt.Month as m, 15 as fifteen, \"ala ma kota\" as text, dt.Ticks, dt.Date.Ticks as rounded_ticks)"
                );


            Assert.AreEqual(typeof(Dictionary<string, object>), expr.ReturnType);
            Assert.AreEqual(typeof(Func<DateTime, Dictionary<string, object>>), expr.Type);
            var texpr = (Expression<Func<DateTime, Dictionary<string, object>>>)expr;
            var fun = texpr.Compile();
            var res = fun(dt);
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
