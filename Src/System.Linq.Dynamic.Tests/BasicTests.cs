using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic.Tests.Helpers;

namespace System.Linq.Dynamic.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Any()
        {
            //Arrange
            IQueryable testListFull = User.GenerateSampleModels(100).AsQueryable();
            IQueryable testListOne = User.GenerateSampleModels(1).AsQueryable();
            IQueryable testListNone = User.GenerateSampleModels(0).AsQueryable();

            //Act
            var resultFull = testListFull.Any();
            var resultOne = testListOne.Any();
            var resultNone = testListNone.Any();

            //Assert
            Assert.IsTrue(resultFull);
            Assert.IsTrue(resultOne);
            Assert.IsFalse(resultNone);
        }

        [TestMethod]
        public void Count()
        {
            //Arrange
            IQueryable testListFull = User.GenerateSampleModels(100).AsQueryable();
            IQueryable testListOne = User.GenerateSampleModels(1).AsQueryable();
            IQueryable testListNone = User.GenerateSampleModels(0).AsQueryable();

            //Act
            var resultFull = testListFull.Count();
            var resultOne = testListOne.Count();
            var resultNone = testListNone.Count();

            //Assert
            Assert.AreEqual(100, resultFull);
            Assert.AreEqual(1, resultOne);
            Assert.AreEqual(0, resultNone);
        }

        [TestMethod]
        public void Skip()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var resultFull = testListQry.Skip(0);
            var resultMinus1 = testListQry.Skip(1);
            var resultHalf = testListQry.Skip(50);
            var resultNone = testListQry.Skip(100);

            //Assert
            CollectionAssert.AreEqual(testList.Skip(0).ToArray(), resultFull.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Skip(1).ToArray(), resultMinus1.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Skip(50).ToArray(), resultHalf.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Skip(100).ToArray(), resultNone.Cast<User>().ToArray());
        }

        [TestMethod]
        public void Take()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var resultFull = testListQry.Take(100);
            var resultMinus1 = testListQry.Take(99);
            var resultHalf = testListQry.Take(50);
            var resultOne = testListQry.Take(1);

            //Assert
            CollectionAssert.AreEqual(testList.Take(100).ToArray(), resultFull.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Take(99).ToArray(), resultMinus1.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Take(50).ToArray(), resultHalf.Cast<User>().ToArray());
            CollectionAssert.AreEqual(testList.Take(1).ToArray(), resultOne.Cast<User>().ToArray());
        }

        [TestMethod]
        public void Single()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var result = testListQry.Take(1).Single();

            //Assert
            Assert.AreEqual(testList[0].Id, result.GetDynamicProperty<Guid>("Id"));
        }

        [TestMethod]
        public void SingleOrDefault()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var singleResult = testListQry.Take(1).SingleOrDefault();
            var defaultResult = ((IQueryable)Enumerable.Empty<User>().AsQueryable()).SingleOrDefault();

            //Assert
            Assert.AreEqual(testList[0].Id, singleResult.GetDynamicProperty<Guid>("Id"));
            Assert.IsNull(defaultResult);
        }

        [TestMethod]
        public void First()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var result = testListQry.First();

            //Assert
            Assert.AreEqual(testList[0].Id, result.GetDynamicProperty<Guid>("Id"));
        }

        [TestMethod]
        public void FirstOrDefault()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var singleResult = testListQry.FirstOrDefault();
            var defaultResult = ((IQueryable)Enumerable.Empty<User>().AsQueryable()).FirstOrDefault();

            //Assert
            Assert.AreEqual(testList[0].Id, singleResult.GetDynamicProperty<Guid>("Id"));
            Assert.IsNull(defaultResult);
        }


        #region As Dynamic Types

#if !NET35

        [TestMethod]
        public void SingleDynamic()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var result = testListQry.Take(1).SingleDynamic();

            //Assert
            Assert.AreEqual(testList[0].Id, result.Id);
        }


        [TestMethod]
        public void SingleOrDefaultDynamic()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var singleResult = testListQry.Take(1).SingleOrDefaultDynamic();
            var defaultResult = ((IQueryable)Enumerable.Empty<User>().AsQueryable()).SingleOrDefaultDynamic();

            //Assert
            Assert.AreEqual(testList[0].Id, singleResult.Id);
            Assert.IsNull(defaultResult);
        }

        [TestMethod]
        public void FirstDynamic()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var result = testListQry.FirstDynamic();

            //Assert
            Assert.AreEqual(testList[0].Id, result.Id);
        }


        [TestMethod]
        public void FirstOrDefaultDynamic()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var singleResult = testListQry.FirstOrDefaultDynamic();
            var defaultResult = ((IQueryable)Enumerable.Empty<User>().AsQueryable()).FirstOrDefaultDynamic();

            //Assert
            Assert.AreEqual(testList[0].Id, singleResult.Id);
            Assert.IsNull(defaultResult);
        }

        [TestMethod]
        public void Reverse()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var reverseResult = testListQry.Reverse();

            //Assert
            CollectionAssert.AreEqual(testList.Reverse().Select(x => x.Id).ToArray(), reverseResult.Select("Id").Cast<Guid>().ToArray());
        }

        [TestMethod]
        public void AsEnumerableDynamic()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            IQueryable testListQry = testList.AsQueryable();

            //Act
            var enumerable = testListQry.AsEnumerableDynamic();

            //Assert
            CollectionAssert.AreEqual(testList.Select(x => x.Id).ToArray(), enumerable.Select(x => x.Id).ToArray());
        }
#endif

        #endregion

    }

}
