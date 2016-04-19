using System;

namespace CPE.Sales.Models
{
    public class RescheduleResult
    {
        public bool HasBeenRescheduled { get; set; }

        public DateTime? RescheduledDate { get; set; }
    }
}
