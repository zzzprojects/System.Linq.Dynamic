using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic;
using System.Collections.Generic;

namespace System.Linq.Dynamic.Tests
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void ExpressionTests_Sum()
        {
            //Arrange
            int[] initValues = new int[] { 1, 2, 3, 4, 5 };
            var qry = initValues.AsQueryable().Select(x => new { strValue = "str", intValue = x }).GroupBy(x => x.strValue);

            //Act
            var result = qry.Select("Sum(intValue)").AsEnumerable().ToArray()[0];

            //Assert
            Assert.AreEqual(15, result);
        }



        [DynamicLinqType]
        public enum TestEnum
        {
            Var1 = 0,
            Var2 = 1,
            Var3 = 2,
            Var4 = 4,
            Var5 = 8,
            Var6 = 16,
        }

        [TestMethod]
        public void ExpressionTests_Enum()
        {
            //Arrange
            var lst = new List<TestEnum>() { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3, TestEnum.Var4, TestEnum.Var5, TestEnum.Var6 };
            var qry = lst.AsQueryable();

            //Act
            var result1 = qry.Where("it < TestEnum.Var4");
            var result2 = qry.Where("TestEnum.Var4 > it");
            var result3 = qry.Where("it = Var5");
            var result4 = qry.Where("it = @0", TestEnum.Var5);
            var result5 = qry.Where("it = @0", 8);

            //Assert
            CollectionAssert.AreEqual(result1.ToArray(), new TestEnum[] { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3});
            CollectionAssert.AreEqual(result2.ToArray(), new TestEnum[] { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3});
            Assert.AreEqual(TestEnum.Var5, result3.Single());
            Assert.AreEqual(TestEnum.Var5, result4.Single());
            Assert.AreEqual(TestEnum.Var5, result5.Single());
        }
    }
}
