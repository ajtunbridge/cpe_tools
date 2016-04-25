using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Data.EntityFramework.Model
{
    public partial class PartVersion : IPartVersion
    {
        public override string ToString()
        {
            return VersionNumber;
        }
    }
}
