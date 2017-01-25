using System.Dynamic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Tests
{
    [TestClass()]
    public class DynamicQueryableTests
    {
        private List<Car> Cars = new List<Car>();
        public DynamicQueryableTests()
        {
            int i = 10;
            for (int j = 0; j < 5; j++)
            {
                foreach (var item in "Honda,Toyota,Nissan".Split(','))
                {
                    Cars.Add(new Car { make = item, vin = i++ });
                }
            }
        }

        [TestMethod()]
        public void Where_StringMatch_PositiveTest()
        {
            int count = Cars.Where("make=\"Honda\"").Count();
            Assert.AreEqual(5, count);
        }

        [TestMethod()]
        public void Where_StringMatch_NegativeTest()
        {
            int count = Cars.Where("make=\"Tesla\"").Count();
            Assert.AreEqual(0, count);
        }

        [TestMethod()]
        public void Where_StringEndsWithSearch_PositiveTest()
        {
            int count = Cars.Where("make.EndsWith(\"a\")").Count();
            Assert.AreEqual(10, count);
        }

        [TestMethod()]
        public void Where_IntSearch_PositiveTest()
        {
            int count = Cars.Where("vin>0").Count();
            Assert.AreEqual(15, count);
        }

        [TestMethod()]
        public void Where_IntSearch_NegativeTest()
        {
            int count = Cars.Where("vin<0").Count();
            Assert.AreEqual(0, count);
        }
    }

    class Car
    {
        public int vin;
        public string make;
        public string model;
    }
}
