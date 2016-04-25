using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.Domain.Model.Tricorn
{
    public sealed class WorksOrder
    {
        public int CustomerReference { get; set; }

        public string UserReference { get; set; }

        public string DrawingNumber { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }
    }
}
