using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Tests.Helpers;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Dynamic.Tests
{
    [TestClass]
    public class DynamicTests
    {
        [TestMethod]
        public void Where()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100, allowNullableProfiles: true);
            var qry = testList.AsQueryable();


            //Act
            var userById = qry.Where("Id=@0", testList[10].Id);
            var userByUserName = qry.Where("UserName=\"User5\"");
            var nullProfileCount = qry.Where("Profile=null");
            var userByFirstName = qry.Where("Profile!=null && Profile.FirstName=@0", testList[1].Profile.FirstName);


            //Assert
            Assert.AreEqual(testList[10], userById.Single());
            Assert.AreEqual(testList[5], userByUserName.Single());
            Assert.AreEqual(testList.Where(x => x.Profile == null).Count(), nullProfileCount.Count());
            Assert.AreEqual(testList[1], userByFirstName.Single());
        }

        [TestMethod]
        public void Where_Exceptions()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100, allowNullableProfiles: true);
            var qry = testList.AsQueryable();

            //Act
            Helper.ExpectException<ParseException>(() => qry.Where("Id"));
            Helper.ExpectException<ParseException>(() => qry.Where("Bad=3"));
            Helper.ExpectException<ParseException>(() => qry.Where("Id=123"));

            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.Where(null, "Id=1"));
            Helper.ExpectException<ArgumentNullException>(() => qry.Where(null));
            Helper.ExpectException<ArgumentException>(() => qry.Where(""));
            Helper.ExpectException<ArgumentException>(() => qry.Where(" "));
        }    
        
        [TestMethod]
        public void OrderBy()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            var qry = testList.AsQueryable();


            //Act
            var orderById = qry.OrderBy("Id");
            var orderByIdDesc = qry.OrderBy("Id DESC");
            var orderByAge = qry.OrderBy("Profile.Age");
            var orderByAgeDesc = qry.OrderBy("Profile.Age DESC");
            var orderByComplex = qry.OrderBy("Profile.Age, Id");
            var orderByComplex2 = qry.OrderBy("Profile.Age DESC, Id");
            

            //Assert
            CollectionAssert.AreEqual(testList.OrderBy(x => x.Id).ToArray(), orderById.ToArray());
            CollectionAssert.AreEqual(testList.OrderByDescending(x => x.Id).ToArray(), orderByIdDesc.ToArray());

            CollectionAssert.AreEqual(testList.OrderBy(x => x.Profile.Age).ToArray(), orderByAge.ToArray());
            CollectionAssert.AreEqual(testList.OrderByDescending(x => x.Profile.Age).ToArray(), orderByAgeDesc.ToArray());

            CollectionAssert.AreEqual(testList.OrderBy(x => x.Profile.Age).ThenBy(x => x.Id).ToArray(), orderByComplex.ToArray());
            CollectionAssert.AreEqual(testList.OrderByDescending(x => x.Profile.Age).ThenBy(x => x.Id).ToArray(), orderByComplex2.ToArray());
        }

        [TestMethod]
        public void OrderBy_Exceptions()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100, allowNullableProfiles: true);
            var qry = testList.AsQueryable();

            //Act
            Helper.ExpectException<ParseException>(() => qry.OrderBy("Bad=3"));
            Helper.ExpectException<ParseException>(() => qry.Where("Id=123"));

            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.OrderBy(null, "Id"));
            Helper.ExpectException<ArgumentNullException>(() => qry.OrderBy(null));
            Helper.ExpectException<ArgumentException>(() => qry.OrderBy(""));
            Helper.ExpectException<ArgumentException>(() => qry.OrderBy(" "));
        }    

        [TestMethod]
        public void Select()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            var qry = testList.AsQueryable();

            //Act
            var userNames = qry.Select("UserName");
            var userFirstName = qry.Select("new (UserName, Profile.FirstName as MyFirstName)");
            
            //Assert
            CollectionAssert.AreEqual(testList.Select(x => x.UserName).ToArray(), userNames.Cast<string>().ToArray());
            CollectionAssert.AreEqual(
                testList.Select(x => "{UserName=" + x.UserName + ", MyFirstName=" + x.Profile.FirstName + "}").ToArray(),
                userFirstName.Cast<DynamicClass>().Select(x => x.ToString()).ToArray());
        }

        [TestMethod]
        public void Select_Exceptions()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100, allowNullableProfiles: true);
            var qry = testList.AsQueryable();

            //Act
            Helper.ExpectException<ParseException>(() => qry.Select("Bad"));
            Helper.ExpectException<ParseException>(() => qry.Select("Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new (Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new (Id, UserName, Bad)"));

            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.Select(null, "Id"));
            Helper.ExpectException<ArgumentNullException>(() => qry.Select(null));
            Helper.ExpectException<ArgumentException>(() => qry.Select(""));
            Helper.ExpectException<ArgumentException>(() => qry.Select(" "));
        }

        [TestMethod]
        public void GroupBy()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100);
            var qry = testList.AsQueryable();

            //Act
            var byAgeReturnUserName = qry.GroupBy("Profile.Age", "UserName");
            var byAgeReturnAll = qry.GroupBy("Profile.Age");

            //Assert
            Assert.AreEqual(testList.GroupBy(x => x.Profile.Age).Count(), byAgeReturnUserName.Count());
            Assert.AreEqual(testList.GroupBy(x => x.Profile.Age).Count(), byAgeReturnAll.Count());
        }

        [TestMethod]
        public void GroupBy_Exceptions()
        {
            //Arrange
            var testList = User.GenerateSampleModels(100, allowNullableProfiles: true);
            var qry = testList.AsQueryable();

            //Act
            Helper.ExpectException<ParseException>(() => qry.GroupBy("Bad"));
            Helper.ExpectException<ParseException>(() => qry.GroupBy("Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.GroupBy("new Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.GroupBy("new (Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.GroupBy("new (Id, UserName, Bad)"));

            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.GroupBy(null, "Id"));
            Helper.ExpectException<ArgumentNullException>(() => qry.GroupBy(null));
            Helper.ExpectException<ArgumentException>(() => qry.GroupBy(""));
            Helper.ExpectException<ArgumentException>(() => qry.GroupBy(" "));

            Helper.ExpectException<ArgumentNullException>(() => qry.GroupBy("Id", (string)null));
            Helper.ExpectException<ArgumentException>(() => qry.GroupBy("Id", ""));
            Helper.ExpectException<ArgumentException>(() => qry.GroupBy("Id", " "));

        }
    }
}
