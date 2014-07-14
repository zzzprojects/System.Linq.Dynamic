using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Linq.Dynamic.Tests.Helpers;

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

        [TestMethod]
        public void ExpressionTests_ContainsGuid()
        {
            //Arrange
            var userList = User.GenerateSampleModels(5, false);
            var userQry = userList.AsQueryable();

            var failValues = new List<Guid>() { 
                new Guid("{22222222-7651-4045-962A-3D44DEE71398}"), 
                new Guid("{33333333-8F80-4497-9125-C96DEE23037D}"), 
                new Guid("{44444444-E32D-4DE1-8F1C-A144C2B0424D}") 
            };
            var successValues = failValues.Concat(new[] { userList[0].Id }).ToArray();


            //Act
            var found1 = userQry.Where("Id in @0", successValues);
            var found2 = userQry.Where("@0.Contains(Id)", successValues);
            var notFound1 = userQry.Where("Id in @0", failValues);
            var notFound2 = userQry.Where("@0.Contains(Id)", failValues);

            //Assert
#if NET35
            Assert.AreEqual(userList[0].Id, ((User)found1.Single()).Id);
            Assert.AreEqual(userList[0].Id, ((User)found2.Single()).Id);
#else
            Assert.AreEqual(userList[0].Id, found1.Single().Id);
            Assert.AreEqual(userList[0].Id, found2.Single().Id);
#endif
            Assert.IsFalse(notFound1.Any());
            Assert.IsFalse(notFound2.Any());
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
            CollectionAssert.AreEqual(new TestEnum[] { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3 }, result1.ToArray());
            CollectionAssert.AreEqual(new TestEnum[] { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3 }, result2.ToArray());
            Assert.AreEqual(TestEnum.Var5, result3.Single());
            Assert.AreEqual(TestEnum.Var5, result4.Single());
            Assert.AreEqual(TestEnum.Var5, result5.Single());
        }

        [TestMethod]
        public void ExpressionTests_CompareWithGuid()
        {
            //Arrange
            var lst = new List<Guid>() { 
                new Guid("{1AF7AD2B-7651-4045-962A-3D44DEE71398}"), 
                new Guid("{99610563-8F80-4497-9125-C96DEE23037D}"), 
                new Guid("{0A191E77-E32D-4DE1-8F1C-A144C2B0424D}") 
            };
            var qry = lst.AsQueryable();

            //Act
            var result1 = qry.Where("it = \"0A191E77-E32D-4DE1-8F1C-A144C2B0424D\"");
            var result2 = qry.Where("\"0A191E77-E32D-4DE1-8F1C-A144C2B0424D\" = it");
            var result3 = qry.Where("it = @0", "0A191E77-E32D-4DE1-8F1C-A144C2B0424D");
            var result4 = qry.Where("it = @0", lst[2]);

            //Assert
            Assert.AreEqual(lst[2], result1.Single());
            Assert.AreEqual(lst[2], result2.Single());
            Assert.AreEqual(lst[2], result3.Single());
            Assert.AreEqual(lst[2], result4.Single());
        }

        [TestMethod]
        public void ExpressionTests_Shift()
        {
            //Arrange
            var lst = new List<int>() { 10, 20, 30 };
            var qry = lst.AsQueryable();

            //Act
            var result1 = qry.Select("it << 1");
            var result2 = qry.Select("it >> 1");
            var result3 = qry.Where("it << 2 = 80");

            //Assert
            CollectionAssert.AreEqual(new int[] { 20, 40, 60 }, result1.Cast<object>().ToArray());
            CollectionAssert.AreEqual(new int[] { 5, 10, 15 }, result2.Cast<object>().ToArray());
            Assert.AreEqual(20, result3.Single());
        }

        [TestMethod]
        public void ExpressionTests_LogicalAndOr()
        {
            //Arrange
            var lst = new List<int>() { 0x20, 0x21, 0x30, 0x31, 0x41 };
            var qry = lst.AsQueryable();

            //Act
            var result1 = qry.Where("(it & 1) > 0");
            var result2 = qry.Where("(it & 32) > 0");

            //Assert
            CollectionAssert.AreEqual(new int[] { 0x21, 0x31, 0x41 }, result1.ToArray());
            CollectionAssert.AreEqual(qry.Where(x => (x & 32) > 0).ToArray(), result2.ToArray());
        }

        [TestMethod]
        public void ExpressionTests_Uri()
        {
            //Arrange
            var lst = new List<Uri>() { 
                new Uri("http://127.0.0.1"),
                new Uri("http://192.168.1.1"), 
                new Uri("http://127.0.0.1") 
            };

            var qry = lst.AsQueryable();

            //Act
            var result1 = qry.AsQueryable().Where("it = @0", new Uri("http://127.0.0.1"));

            //Assert
            Assert.AreEqual(result1.Count(), 2);
        }
    }
}
