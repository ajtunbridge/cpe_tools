using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.Sales.Services
{
    public class RescheduleResult
    {
        public bool HasBeenRescheduled { get; set; }

        public DateTime? RescheduledDate { get; set; }
    }
}
