using System;

namespace CPE.Sales.Models
{
    public class SalesOrderLine
    {
        private byte[] test;

        public string DrawingNumber { get; set; }

        public DateTime DeliveryDate { get; set; }

        public byte[] Photo
        {
            get { return test; }
            set { test = value; }
        }
    }
}
