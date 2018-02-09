using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    class Value
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    [TestClass]
    public class StringComparisionTests
    {
        private readonly List<Value> data = new List<Value> { new Value { Id = 1, Text = "System.Linq.Dynamic" }, new Value { Id = 2, Text = "system.linq.dynamic" } };

        [TestMethod]
        public void TestStringStartsWith()
        {
            var queryable = (from d in data select d).AsQueryable();

            // Case sensitive 
            var resultCaseSensitive = queryable.Where("Text.StartsWith(\"System\")").ToList();
            Assert.AreEqual(1, resultCaseSensitive.Count());

            // Ignore case
            var resultIgnoreCase = queryable.Where("Text.StartsWith(\"System\", System.StringComparison.OrdinalIgnoreCase)").ToList();
            Assert.AreEqual(2, resultIgnoreCase.Count());
        }

        [TestMethod]
        public void TestStringEndsWith()
        {
            var queryable = (from d in data select d).AsQueryable();
            
            // Case sensitive 
            var resultCaseSensitive = queryable.Where("Text.EndsWith(\"Dynamic\")").ToList();
            Assert.AreEqual(1, resultCaseSensitive.Count());

            // Ignore case
            var resultIgnoreCase = queryable.Where("Text.EndsWith(\"Dynamic\", System.StringComparison.OrdinalIgnoreCase)").ToList();
            Assert.AreEqual(2, resultIgnoreCase.Count());
        }

        [TestMethod]
        public void TestStringContains()
        {
            var queryable = (from d in data select d).AsQueryable();
                        
            // Case sensitive
            var resultCaseSensitive = queryable.Where("Text.IndexOf(\"Dynamic\") > 0").ToList();
            Assert.AreEqual(1, resultCaseSensitive.Count());

            // Ignore case
            var resultIgnoreCase = queryable.Where("Text.IndexOf(\"Dynamic\", System.StringComparison.OrdinalIgnoreCase) > 0").ToList();
            Assert.AreEqual(2, resultIgnoreCase.Count());
        }
    }
}
