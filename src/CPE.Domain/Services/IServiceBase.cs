using System.Collections.Generic;
using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IServiceBase<T>
    {
        T GetById(int id);

        Task<T> GetByIdAsync(int id);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        void Insert(T entity);

        void Delete(T entity);

        void Update(T entity);
    }
}