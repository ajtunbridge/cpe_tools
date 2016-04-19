using System;
using System.Collections.Generic;
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

        public byte[] GetPhotoByPart(IPart part)
        {
            if (part == null)
            {
                return null;
            }

            var latestVersion =
                Entities.PartVersions.Where(pv => pv.PartId == part.Id)
                    .OrderByDescending(pv => pv.VersionNumber)
                    .First();

            var address = $"PartVersion:{latestVersion.Id}";

            var photo = Entities.Photos.SingleOrDefault(p => p.Address == address);

            return photo?.Data;
        }
    }
}
