using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
  public  class GetPredicateTests
    {
        private List<Customer> CustomerList;

        public GetPredicateTests()
        {
            CustomerList = new List<Customer>
            {
                new Customer {EmpId = 1, EmpName = "John"},
                new Customer {EmpId = 2, EmpName = "Martin"}
            };

        }

        [TestMethod]
        public void Parse_GetPredicate()
        {
            // Find method of list need Type Predicate

            var predicate = DynamicExpression.GetPredicate<Customer>("EmpID==1");
            var customer = CustomerList.Find(predicate);
            Assert.AreEqual(customer.EmpId, 1);
        }
    }


    public class Customer
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int EmpSalary { get; set; }

    }

}
