using System;
using System.Linq;
using System.Transactions;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using CPE.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPE.Data.EntityFramework.Tests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Can_Insert_Customer()
        {
            using (var scope = new TransactionScope())
            {
                var customers = new CustomerRepository(new CPEEntities());

                var customer = new Customer
                {
                    Name = "Unit Test Customer",
                    CreatedBy = 8,
                    ModifiedBy = 8
                };

                customers.Insert(customer);
                customers.Commit();

                Assert.IsTrue(customer.Id > 0);
            }
        }

        [TestMethod]
        public void Can_Retrieve_Customer_By_Id()
        {
            using (var scope = new TransactionScope())
            {
                var customers = new CustomerRepository(new CPEEntities());

                var customer = new Customer
                {
                    Name = "Unit Test Customer",
                    CreatedBy = 8,
                    ModifiedBy = 8
                };

                customers.Insert(customer);

                customers.Commit();

                var retrieved = customers.GetById(customer.Id);

                Assert.IsTrue(retrieved.Id == customer.Id);
            }
        }

        [TestMethod]
        public void Can_Get_Set_Sales_Order_Parser_Settings()
        {
            using (var scope = new TransactionScope())
            {
                var customers = new CustomerRepository(new CPEEntities());

                var customer = new Customer
                {
                    Name = "Unit Test Customer",
                    CreatedBy = 8,
                    ModifiedBy = 8
                };

                customer.SetSalesOrderParserSettings(new SalesOrderParserSettingsBlob {BuyerExpr = "TEST"});

                customers.Insert(customer);

                Assert.IsTrue(customer.GetSalesOrderParserSettings().BuyerExpr == "TEST");
            }
        }

        [TestMethod]
        public void Can_Retrieve_All_Customers()
        {
            using (var customers = new CustomerRepository(new CPEEntities()))
            {
                Assert.IsTrue(customers.GetAll().Any());
            }
        }
    }
}
