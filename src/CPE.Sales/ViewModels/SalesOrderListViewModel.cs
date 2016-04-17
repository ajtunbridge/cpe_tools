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

        public ObservableCollection<DisplayModel> SalesOrders { get; set; } = new ObservableCollection<DisplayModel>();

        public async Task GetSalesOrdersAsync()
        {
            var unsortedOrders = new List<DisplayModel>();

            var mailItems = await Task.Factory.StartNew(() => MSOutlookService.GetSalesOrderMail());

            foreach (var mail in mailItems)
            {
                var customer = await _parserService.ParseCustomerAsync(mail);
                var buyer = await _parserService.ParseBuyerAsync(mail);
                var isMultiLine = await _parserService.IsMultiLineOrder(mail);
                var order = await _parserService.ParseOrderNumberAsync(mail);
                var earliestDelivery = await _parserService.ParseEarliestDeliveryAsync(mail);

                var model = new DisplayModel
                {
                    Buyer = buyer,
                    EarliestDelivery = earliestDelivery,
                    IsMultiline = isMultiLine,
                    OrderNumber = order,
                    Mail = mail,
                    Logo = customer.LogoBlob
                };

                unsortedOrders.Add(model);
            }

            SalesOrders.Clear();

            foreach (var order in unsortedOrders.OrderBy(o => o.EarliestDelivery))
            {
                SalesOrders.Add(order);
            }
        } 

        public class DisplayModel
        {
            public string OrderNumber { get; set; }

            public DateTime EarliestDelivery { get; set; }
            
            public string Buyer { get; set; }

            public bool IsMultiline { get; set; }

            public MSOutlookMailItem Mail { get; set; }

            public byte[] Logo { get; set; }
        }
    }
}