using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Services;
using CPE.Sales.Services;
using GalaSoft.MvvmLight;

namespace CPE.Sales.ViewModel
{
    public sealed class ParserSettingsTestViewModel : ViewModelBase
    {
        private NewSalesOrdersService _salesOrderService;
        private ICustomerService _customers;
        private ITricornService _tricorn;

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

        public ParserSettingsTestViewModel(NewSalesOrdersService salesOrderService, ICustomerService customers, ITricornService tricorn)
        {
            _salesOrderService = salesOrderService;
            _tricorn = tricorn;
            _customers = customers;
        }

        public async Task CheckCpeCentralConnection()
        {
            StatusMessage = "Checking....";

            try
            {
                var c = await _customers.GetAllAsync();

                StatusMessage = "Connected to CPECentral successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            } 
        }

        public async Task CheckTricornConnection()
        {
            StatusMessage = "Checking....";

            try
            {
                var c = await _tricorn.GetNameByDrawingNumberAsync("H17070A");

                StatusMessage = "Connected to Tricorn successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
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

                    if (!File.Exists(fileName))
                    {
                        File.Copy(so.MailItem.Attachments[0], fileName);
                    }
                }

                StatusMessage = $"Failed to parse {failedOrders.Count()} orders. Orders copied to selected directory.";
            }
            else
            {
                StatusMessage = "All sales order were processed successfully!";
            }
        }
    }
}
