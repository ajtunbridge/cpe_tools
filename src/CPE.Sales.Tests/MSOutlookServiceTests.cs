using System;
using CPE.Sales.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPE.Sales.Tests
{
    [TestClass]
    public class MSOutlookServiceTests
    {
        [TestMethod]
        public void Can_Get_Emails()
        {
            var result = MSOutlookService.GetSalesOrderMail();

            Assert.IsTrue(result.Count > 0);
        }
    }
}
