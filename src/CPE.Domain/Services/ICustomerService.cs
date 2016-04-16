using System.Collections.Generic;
using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface ICustomerService : IServiceBase<ICustomer>
    {
        /// <summary>
        /// Gets all customers whose sales orders are parseable
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICustomer> GetSalesOrderParseable();

        /// <summary>
        /// Asynchronously gets all customers whose sales orders are parseable
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ICustomer>> GetSalesOrderParseableAsync();
    }
}