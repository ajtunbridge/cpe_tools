using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Sales.Services;
using GalaSoft.MvvmLight;

namespace CPE.Sales.ViewModel
{
    public sealed class ParserSettingsTestViewModel : ViewModelBase
    {
        private NewSalesOrdersService _salesOrderService;
        private string _statusMessage = "Please ensure settings are correct then press the 'Scan folder now' button.";

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }

        public ParserSettingsTestViewModel(NewSalesOrdersService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        public async Task PerformTestAsync(string orderDirectory, string outputDirectory)
        {
            StatusMessage = "Scanning sales order folder....";

            var salesOrders = await _salesOrderService.GetNewSalesOrdersAsync(orderDirectory);

            if (!salesOrders.Any())
            {
                StatusMessage = "No orders found in a folder called '" + orderDirectory + "'";
                return;
            }

            var failedOrders = salesOrders.Where(so => so.EarliestDeliveryDate == DateTime.MinValue);

            if (failedOrders.Any())
            {
                foreach (var so in failedOrders)
                {
                    var fileName = Path.Combine(outputDirectory, Path.GetFileName(so.MailItem.Attachments[0]));

                    File.Copy(so.MailItem.Attachments[0], fileName);
                }

                StatusMessage = string.Format("Failed to parse {0} orders. Orders copied to selected directory.",
                    failedOrders.Count());
            }
            else
            {
                StatusMessage = "All sales order were processed successfully!";
            }
        }
    }
}
