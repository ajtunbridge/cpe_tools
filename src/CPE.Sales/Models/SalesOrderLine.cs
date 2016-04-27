using System;
using System.Collections.Generic;

namespace CPE.Sales.Models
{
    public class SalesOrderLine
    {
        public string DrawingNumber { get; set; }

        public string Name { get; set; }

        public List<BatchDelivery> BatchDeliveries { get; } = new List<BatchDelivery>();

        public DateTime OriginalDeliveryDate { get; set; }

        public DateTime? RescheduledDeliveryDate { get; set; }

        public byte[] Photo { get; set; }

        public bool IsMultiDrop => BatchDeliveries.Count > 0;
    }
}
