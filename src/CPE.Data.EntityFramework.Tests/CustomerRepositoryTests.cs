using System;
using System.Linq;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using CPE.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPE.Data.EntityFramework.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private CustomerRepository _repository;
        private Customer _customer;

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

        [TestMethod]
        public void Can_Insert_Customer()
        {
            _customer = new Customer
            {
                Name="Unit Test Customer",
                CreatedBy=8,
                ModifiedBy=8
            };

            _repository.Insert(_customer);
            _repository.Commit();

            Assert.IsTrue(_customer.Id > 0);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_customer == null)
            {
                return;
            }

            _repository.Delete(_customer);
            
            _repository.Commit();
        }
    }
}
