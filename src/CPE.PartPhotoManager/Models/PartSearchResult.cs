using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.PartPhotoManager.Models
{
    public sealed class PartSearchResult
    {
        public string DrawingNumber { get; set; }

        public string LatestVersion { get; set; }

        public string Name { get; set; }

        public byte[] Photo { get; set; }
    }
}
