using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Services;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class SalesOrderListViewModel
    {
        private readonly SalesOrderParserService _parserService;

        public SalesOrderListViewModel(SalesOrderParserService parserService)
        {
            _parserService = parserService;
        }

        public ObservableCollection<DisplayModel> SalesOrders { get; set; }

        public async Task GetSalesOrdersAsync()
        {
            var mailItems = await Task.Factory.StartNew(() => MSOutlookService.GetSalesOrderMail());

            foreach (var mail in mailItems)
            {
                var customer = await _parserService.ParseCustomerAsync(mail);
                var buyer = await _parserService.ParseBuyerAsync(mail);

            }
        } 

        public class DisplayModel
        {
            public string OrderNumber { get; set; }

            public DateTime EarliestDelivery { get; set; }
            
            public string Buyer { get; set; }

            public MSOutlookMailItem Mail { get; set; }

            public byte[] Logo { get; set; }
        }
    }
}