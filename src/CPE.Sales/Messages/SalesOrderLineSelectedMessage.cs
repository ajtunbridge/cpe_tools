using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Sales.Models;

namespace CPE.Sales.Messages
{
    public class SalesOrderLineSelectedMessage
    {
        public SalesOrderLine SelectedSalesOrderLine { get; private set; }

        public SalesOrderLineSelectedMessage(SalesOrderLine selectedSalesOrderLine)
        {
            SelectedSalesOrderLine = selectedSalesOrderLine;
        }
    }
}
