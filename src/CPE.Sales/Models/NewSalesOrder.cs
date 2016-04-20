using System;
using System.Collections.Generic;

namespace CPE.Sales.Models
{
    public class NewSalesOrder
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string Buyer { get; set; }
        public DateTime EarliestDeliveryDate { get; set; }
        public decimal TotalValue { get; set; }

        public int NumberOfLines => Lines.Count;

        public List<SalesOrderLine> Lines { get; set; } = new List<SalesOrderLine>();
    }
}