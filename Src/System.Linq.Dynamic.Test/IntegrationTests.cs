using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Dynamic;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class IntegrationTests
    {
        private class TestObject
        {
            public int Id { get; set; }
            public string Color { get; set; }
            public int Number { get; set; }
        }

        private IList<TestObject> GetTestData()
        {
            return new List<TestObject>()
            {
                new TestObject() {Id = 0, Color = "Red", Number = 10 },
                new TestObject() {Id = 1, Color = "Red", Number = 20 },
                new TestObject() {Id = 2, Color = "\"Bright\" Red", Number = 30 },
                new TestObject() {Id = 3, Color = "Blue", Number = 5 },
                new TestObject() {Id = 4, Color = "Brown", Number = 15 },
                new TestObject() {Id = 5, Color = "\"Faded\" Blue", Number = 25 },
                new TestObject() {Id = 6, Color = "Blue\\Green", Number = 30 },
                new TestObject() {Id = 7, Color = "Yellow\tOrange", Number = 35 },
                new TestObject() {Id = 8, Color = "Bright Orange", Number = 30 },
                new TestObject() {Id = 9, Color = "Faded\u0009Orange", Number = 40 }
            };
        }

        private bool TestItemsIncluded(IList<TestObject> data, IEnumerable<int> expectedIds)
        {
            Assert.AreEqual(expectedIds.Count(), data.Count(), "Unexpected number of items returned.");

            foreach (int id in expectedIds)
            {
                var results = data.Where(x => x.Id == id);
                Assert.IsTrue(results.Count() > 0, string.Format("Expected item {0} to be included.", id));
            }

            return true;
        }

        [TestMethod]
        public void Where_StringEquality()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color == \"Red\"").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 0, 1 });
        }

        [TestMethod]
        public void Where_StringContains()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color.Contains(\"Red\")").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 0, 1, 2 });
        }

        [TestMethod]
        public void Where_EscapedStringContains()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color.Contains(\"\\\"Bright\\\"\")").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 2 });
        }

        [TestMethod]
        public void Where_EscapedStringEquality()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color == \"\\\"Faded\\\" Blue\"").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 5 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.Linq.Dynamic.ParseException))]
        public void Where_InvalidClause()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color.Contains(\\\"Bright\\\")").ToList<TestObject>();
        }

        [TestMethod]
        public void Where_NumberRange()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Number <= 25 && Number > 5").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 0, 1, 4, 5 });
        }

        [TestMethod]
        public void Where_EscapeChar()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color.Contains(\"\\\\\")").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 6 });
        }

        [TestMethod]
        public void Where_EscapedTab()
        {
            var testData = GetTestData();
            var testResults = testData.Where("Color.Contains(\"\\t\")").ToList<TestObject>();

            TestItemsIncluded(testResults, new List<int>() { 7, 9 });
        }
    }
}
