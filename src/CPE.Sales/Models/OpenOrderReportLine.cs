using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.Sales.Models
{
    public class OpenOrderReportLine
    {
        public string DrawingNumber { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OriginalDeliveryDate { get; set; }

        public DateTime RescheduledDeliveryDate { get; set; }

        public int RowIndex { get; set; }
    }
}
