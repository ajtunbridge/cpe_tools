using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Sales.Models;

namespace CPE.Sales.Messages
{
    public sealed class SalesOrderSelectedMessage
    {
        public SalesOrder SelectedSalesOrder { get; private set; }

        public SalesOrderSelectedMessage(SalesOrder selectedSalesOrder)
        {
            SelectedSalesOrder = selectedSalesOrder;
        }
    }
}
