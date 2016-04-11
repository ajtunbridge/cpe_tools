using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;

namespace CPE.Data.EntityFramework.Repositories
{
    public class CustomerRepository : RepositoryBase
    {
        public CustomerRepository(CPEEntities entities) : base(entities)
        {
        }

        public async Task<ICustomer> GetByIdAsync(int id)
        {
            return await _entities.Customers.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ICustomer>> GetAllAsync()
        {
            return await _entities.Customers.ToListAsync();
        }
    }
}