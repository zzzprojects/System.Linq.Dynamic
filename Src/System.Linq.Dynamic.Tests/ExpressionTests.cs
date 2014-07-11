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
    }
}
