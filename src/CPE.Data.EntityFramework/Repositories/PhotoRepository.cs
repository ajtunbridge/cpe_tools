using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories
{
    public sealed class PhotoRepository : RepositoryBase<Photo>, IPhotoService
    {
        public PhotoRepository(CPEEntities entities) : base(entities)
        {
        }

        public async Task<byte[]> GetPhotoByPartAsync(IPart part)
        {
            if (part == null)
            {
                return null;
            }

            var latestVersion = await 
                Entities.PartVersions.Where(pv => pv.PartId == part.Id)
                    .OrderByDescending(pv => pv.VersionNumber)
                    .FirstAsync();

            var address = $"PartVersion:{latestVersion.Id}";

            var photo = await Entities.Photos.SingleOrDefaultAsync(p => p.Address == address);

            return photo?.Data;
        }
    }
}
