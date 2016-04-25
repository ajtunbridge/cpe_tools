using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories
{
    public sealed class PartVersionRepository : RepositoryBase<PartVersion>, IPartVersionService
    {
        public PartVersionRepository(CPEEntities entities) : base(entities)
        {
        }

        public async Task<IPartVersion> GetLatestVersionAsync(int partId)
        {
            var latestVersion = await Entities.PartVersions.Where(pv => pv.PartId == partId)
                .OrderByDescending(pv => pv.VersionNumber)
                .SingleAsync();

            return latestVersion;
        }

        public new IPartVersion GetById(int id)
        {
            return base.GetById(id);
        }

        public new async Task<IPartVersion> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public new IEnumerable<IPartVersion> GetAll()
        {
            return base.GetAll();
        }

        public new async Task<IEnumerable<IPartVersion>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public void Insert(IPartVersion PartVersion)
        {
            base.Insert(PartVersion as PartVersion);
        }

        public void Update(IPartVersion PartVersion)
        {
            base.Update(PartVersion as PartVersion);
        }

        public void Delete(IPartVersion PartVersion)
        {
            base.Delete(PartVersion as PartVersion);
        }
    }
}