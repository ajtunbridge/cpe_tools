using System.Collections.Generic;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerService
    {
        public CustomerRepository(CPEEntities entities) : base(entities)
        {
        }

        public new ICustomer GetById(int id)
        {
            return base.GetById(id);
        }

        public new async Task<ICustomer> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public new IEnumerable<ICustomer> GetAll()
        {
            return base.GetAll();
        }

        public new async Task<IEnumerable<ICustomer>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public void Insert(ICustomer entity)
        {
            base.Insert(entity as Customer);
        }

        public void Delete(ICustomer entity)
        {
            base.Delete(entity as Customer);
        }

        public void Update(ICustomer entity)
        {
            base.Update(entity as Customer);
        }
    }
}