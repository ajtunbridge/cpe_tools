using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories
{
    public sealed class PartRepository : RepositoryBase<Part>, IPartService
    {
        public PartRepository(CPEEntities entities) : base(entities)
        {
        }

        public IPart GetWhereDrawingNumberEquals(string drawingNumber)
        {
            return GetSet().SingleOrDefault(p => p.DrawingNumber == drawingNumber);
        }

        public new IPart GetById(int id)
        {
            return base.GetById(id);
        }

        public new async Task<IPart> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public new IEnumerable<IPart> GetAll()
        {
            return base.GetAll();
        }

        public new async Task<IEnumerable<IPart>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public void Insert(IPart part)
        {
            base.Insert(part as Part);
        }

        public void Update(IPart part)
        {
            base.Update(part as Part);
        }

        public void Delete(IPart part)
        {
            base.Delete(part as Part);
        }
    }
}