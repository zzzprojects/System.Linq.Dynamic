using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic.Tests.Helpers;

namespace System.Linq.Dynamic.Tests
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void Operator_Multiplication_Single_Float_ParseException()
        {
            //Arrange
            var models = new SimpleValuesModel[] { new SimpleValuesModel() }.AsQueryable();

            //Act
            models.Select("FloatValue * DecimalValue");
        }

        [TestMethod]
        public void Operator_Multiplication_Single_Float_Cast()
        {
            //Arrange
            var models = new SimpleValuesModel[] { new SimpleValuesModel() { FloatValue = 2, DecimalValue = 3 } }.AsQueryable();

            //Act
            var result = models.Select("Decimal(FloatValue) * DecimalValue").First();

            //Assert
            Assert.AreEqual(6.0m, result);
        }

        [TestMethod]
        public void Operator_Equal_With_Explicit_Convertion_From_String_To_Int32()
        {
            //Arrange
            var models = new SimpleValuesModel[] { new SimpleValuesModel() { FloatValue = 2, DecimalValue = 3 } };
            var query = models.AsQueryable();

            //Act
            var result1 = query.Where("FloatValue == int\"2\"").First();
            var result2 = query.Where("FloatValue == Int32\"2\"").First();

            //Assert
            Assert.AreEqual(models[0], result1);
            Assert.AreEqual(models[0], result2);
        }

        [TestMethod]
        public void Operator_Equal()
        {
            //Arrange
            var models = new SimpleValuesModel[] { new SimpleValuesModel() { FloatValue = 2, DecimalValue = 3 } };
            var query = models.AsQueryable();

            //Act
            var result1 = query.Where("FloatValue == 2").First();
            var result2 = query.Where("FloatValue = 2").First();
            var result3 = query.Where("FloatValue eq 2").First();

            //Assert
            Assert.AreEqual(models[0], result1);
            Assert.AreEqual(models[0], result2);
            Assert.AreEqual(models[0], result3);
        }

        [TestMethod]
        public void Operator_NotEqual()
        {
            //Arrange
            var models = new SimpleValuesModel[] { 
                new SimpleValuesModel() { FloatValue = 2, DecimalValue = 3 },
                new SimpleValuesModel() { FloatValue = 1, DecimalValue = 3 }
            };
            var query = models.AsQueryable();

            //Act
            var result1 = query.Where("FloatValue != 2").First();
            var result2 = query.Where("FloatValue <> 2").First();
            var result3 = query.Where("FloatValue ne 2").First();

            //Assert
            Assert.AreEqual(models[1], result1);
            Assert.AreEqual(models[1], result2);
            Assert.AreEqual(models[1], result3);
        }
    }
}
