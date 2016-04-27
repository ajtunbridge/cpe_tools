using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.Sales.Models
{
    public sealed class BatchDelivery
    {
        public int Quantity { get; set; }

        public DateTime DeliveryDate { get; set; }
    }
}
