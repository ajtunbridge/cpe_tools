using System;
using System.Linq;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPE.Data.EntityFramework.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private CustomerRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new CustomerRepository(new CPEEntities());
        }

        [TestMethod]
        public void Can_Retrieve_Customer_By_Id()
        {
            var customer = _repository.GetByIdAsync(13);

            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Can_Retrieve_All_Customers()
        {
            var customers = _repository.GetAllAsync();

            Assert.IsTrue(customers.Result.Any());
        }
    }
}
