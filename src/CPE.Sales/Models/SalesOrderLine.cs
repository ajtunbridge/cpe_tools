using System;

namespace CPE.Sales.Models
{
    public class SalesOrderLine
    {
        public string DrawingNumber { get; set; }

        public string Name { get; set; }

        public DateTime OriginalDeliveryDate { get; set; }

        public DateTime? RescheduledDeliveryDate { get; set; }

        public byte[] Photo { get; set; }
    }
}
