using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic.Tests.Helpers;
using System.Linq;

namespace System.Linq.Dynamic.Tests
{
    [TestClass]
    public class ComplexTests
    {


        /// <summary>
        /// The purpose of this test is to verify that after a group by of a dynamically created
        /// key, the Select clause can access the key's members
        /// </summary>
        [TestMethod]
        public void GroupByAndSelect_TestDynamicSelectMember()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            var qry = testList.AsQueryable();

            //Act
            var byAgeReturnAll = qry.GroupBy("new (Profile.Age)");
            var selectQry = byAgeReturnAll.Select("new (Key.Age, Sum(Income) As TotalIncome)");

            //Real Comparison
            var realQry = qry.GroupBy(x => new { x.Profile.Age }).Select(x => new { x.Key.Age, TotalIncome = x.Sum(y => y.Income) });

            //Assert
            Assert.AreEqual(realQry.Count(), selectQry.Count());
#if NET35
            CollectionAssert.AreEqual(
                realQry.Select(x => x.Age).ToArray(),
                selectQry.Cast<object>().Select(x => ((object)x).GetDynamicProperty<int?>("Age")).ToArray());

            CollectionAssert.AreEqual(
                realQry.Select(x => x.TotalIncome).ToArray(),
                selectQry.Cast<object>().Select(x => ((object)x).GetDynamicProperty<int>("TotalIncome")).ToArray());
#else
            CollectionAssert.AreEqual(
                realQry.Select(x => x.Age).ToArray(),
                selectQry.AsEnumerable().Select(x => x.Age).ToArray());

            CollectionAssert.AreEqual(
                realQry.Select(x => x.TotalIncome).ToArray(),
                selectQry.AsEnumerable().Select(x => x.TotalIncome).ToArray());
#endif
        }



        [TestMethod]
        public void CompareWithGuidTest()
        {
            var lst = new List<Guid>() { new Guid("{1AF7AD2B-7651-4045-962A-3D44DEE71398}"), new Guid("{99610563-8F80-4497-9125-C96DEE23037D}"), new Guid("{0A191E77-E32D-4DE1-8F1C-A144C2B0424D}") };
            var qry = lst.AsQueryable().Select(x => new { strValue = "str", gg = x }).AsQueryable();

            var sel=qry.AsQueryable().Where("gg = \"0A191E77-E32D-4DE1-8F1C-A144C2B0424D\"");

            Assert.AreEqual(sel.Count(), 1);
        }


        [TestMethod]
        public void ShiftTest()
        {
            var lst = new List<int>() { 10 };
            var qry = lst.AsQueryable().Select(x => new { strValue = "str", gg = x }).AsQueryable();

            var sel = qry.AsQueryable().Select("new ((gg << 1) as aa)").Select("aa").Cast<int>().First();

            Assert.AreEqual(sel, 20);
        }

        [TestMethod]
        public void LogicalAndTest()
        {
            var lst = new List<int>() { 0x020, 0x021, 0x30, 0x31, 0x41 };
            var qry = lst.AsQueryable().Select(x => new { strValue = "str", gg = x }).AsQueryable();

            var sel = qry.AsQueryable().Where("(gg & 1) > 0");

            Assert.AreEqual(sel.Count(), 3);
        }

        [TestMethod]
        public void UriTest()
        {
            var lst = new List<Uri>() { new Uri("http://127.0.0.1"), new Uri("http://192.168.1.1"), new Uri("http://127.0.0.1") };
            var qry = lst.AsQueryable().Select(x => new { strValue = "str", gg = x }).AsQueryable();

            var sel = qry.AsQueryable().Where("gg = @0", new Uri("http://127.0.0.1"));

            Assert.AreEqual(sel.Count(), 2);
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
        public void EnumTest()
        {
            var lst = new List<TestEnum>() { TestEnum.Var1, TestEnum.Var2, TestEnum.Var3, TestEnum.Var4, TestEnum.Var5, TestEnum.Var6 };
            var qry = lst.AsQueryable().Select(x => new { strValue = "str", gg = x }).AsQueryable();

            var sel = qry.AsQueryable().Where("gg < TestEnum.Var4");

            Assert.AreEqual(sel.Count(), 3);
        }

#if !NET35
        [TestMethod]
        public void GroupByManyTest()
        {
            var lst = new List<Tuple<int, int, int>>()
            {
                new Tuple<int, int, int>(1, 1, 1),
                new Tuple<int, int, int>(1, 1, 2),
                new Tuple<int, int, int>(1, 1, 3),
                new Tuple<int, int, int>(2, 2, 4),
                new Tuple<int, int, int>(2, 2, 5),
                new Tuple<int, int, int>(2, 2, 6),
                new Tuple<int, int, int>(2, 3, 7)
            };
            
            var sel = lst.AsQueryable().GroupByMany("Item1", "Item2");

            Assert.AreEqual(sel.Count(), 2);
            Assert.AreEqual(sel.First().SubGroups.Count(), 1);
            Assert.AreEqual(sel.Skip(1).First().SubGroups.Count(), 2);
        }

        [TestMethod]
        public void GroupByManyTest2()
        {
            var lst = new List<Tuple<int, int, int>>()
            {
                new Tuple<int, int, int>(1, 1, 1),
                new Tuple<int, int, int>(1, 1, 2),
                new Tuple<int, int, int>(1, 1, 3),
                new Tuple<int, int, int>(2, 2, 4),
                new Tuple<int, int, int>(2, 2, 5),
                new Tuple<int, int, int>(2, 2, 6),
                new Tuple<int, int, int>(2, 3, 7)
            };

            var sel = lst.AsQueryable().GroupByMany(x => x.Item1, x => x.Item2);

            Assert.AreEqual(sel.Count(), 2);
            Assert.AreEqual(sel.First().SubGroups.Count(), 1);
            Assert.AreEqual(sel.Skip(1).First().SubGroups.Count(), 2);
        }
#endif
    }
}
