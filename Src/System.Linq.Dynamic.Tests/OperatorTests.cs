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
    }
}
