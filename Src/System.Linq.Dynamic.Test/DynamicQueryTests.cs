using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic.Test.Models;

namespace System.Linq.Dynamic.Test
{
    /// <summary>
    /// DynamicQueryTests
    /// </summary>
    [TestClass]
    public class DynamicQueryTests
    {
        public DynamicQueryTests()
        {
        }

        [TestMethod]
        public void TestContains()
        {
            IList<Person> persons = new List<Person>
            {
                new Person { Name = "123abcddd", Age = 20 },
                new Person { Name = "456defeee", Age = 21},
                new Person { Name = "789abcfff", Age = 22},
                new Person { Name = "sdfsdfasd", Age = 22}
            };

            //query ok
            var data1 = persons.Where("Name.Contains(@0)", "abc").ToList();

            string[] array = new string[] { "123abcddd", "456defeee" };

            //query ok
            var data2 = persons.Where("@0.Contains(Name)", array[0]).ToList();

            //query ok
            var data3 = persons.Where("@0.Contains(Name)", (object)array).ToList();

            Assert.AreEqual(data1.Count, 2);
            Assert.AreEqual(data2.Count, 1);
            Assert.AreEqual(data3.Count, 2);
        }
    }
}
