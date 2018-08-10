using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class MemberSearchTests
    {
        [TestMethod]
        public void MemberSearch_ClassMemberPriority()
        {
            var expression = DynamicExpression.ParseLambda(
                new[] { Expression.Parameter(typeof(EnumerableClassWithConflictMembers), "") },
                typeof(bool),
                "it.Any()");
            var result = expression.Compile().DynamicInvoke(new EnumerableClassWithConflictMembers());

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void MemberSearch_ClassMemberPriority_ArgumentContext()
        {
            var expression = DynamicExpression.ParseLambda(
                new[] { Expression.Parameter(typeof(EnumerableClassWithConflictMembers), "") },
                typeof(int),
                "it.Sum(val)");
            var result = expression.Compile().DynamicInvoke(new EnumerableClassWithConflictMembers());

            Assert.AreEqual(90, result);
        }

        [TestMethod]
        public void MemberSearch_AggregateFunction()
        {
            var expression = DynamicExpression.ParseLambda(
                new[] { Expression.Parameter(typeof(EnumerableClassWithoutConflictMembers), "") },
                typeof(bool),
                "it.Any()", new EnumerableClassWithoutConflictMembers());
            var result = expression.Compile().DynamicInvoke(new EnumerableClassWithoutConflictMembers());

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void MemberSearch_AggregateFunction_ArgumentContext()
        {
            var expression = DynamicExpression.ParseLambda(
                new[] { Expression.Parameter(typeof(EnumerableClassWithoutConflictMembers), "") },
                typeof(int),
                "it.Sum(val)");
            var result = expression.Compile().DynamicInvoke(new EnumerableClassWithoutConflictMembers());

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void MemberSearch_AggregateFunction_ArgumentContext_WithoutIt()
        {
            var expression = DynamicExpression.ParseLambda(
                new[] { Expression.Parameter(typeof(EnumerableClassWithoutConflictMembers), "x") },
                typeof(int),
                "x.Sum(val)");
            var result = expression.Compile().DynamicInvoke(new EnumerableClassWithoutConflictMembers());

            Assert.AreEqual(6, result);
        }

        private class EnumerableClassWithoutConflictMembers : IEnumerable<SimpleObject>
        {
            public int val = 88;

            public IEnumerator<SimpleObject> GetEnumerator()
            {
                yield return new SimpleObject(1);
                yield return new SimpleObject(2);
                yield return new SimpleObject(3);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class EnumerableClassWithConflictMembers : IEnumerable<SimpleObject>
        {
            public int val = 88;

            public bool Any() => false;
            public int Sum(int input) => input + 2;

            public IEnumerator<SimpleObject> GetEnumerator()
            {
                yield return new SimpleObject(1);
                yield return new SimpleObject(2);
                yield return new SimpleObject(3);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class SimpleObject
        {
            public int val;

            public SimpleObject(int value)
            {
                val = value;
            }
        }
    }
}
