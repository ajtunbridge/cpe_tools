using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Sales.Models;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class NewSalesOrdersViewModel : ViewModelBase
    {
        private readonly NewSalesOrdersService _parserService;

        public ObservableCollection<NewSalesOrder> NewSalesOrders { get; set; } = new ObservableCollection<NewSalesOrder>();


        public NewSalesOrdersViewModel(NewSalesOrdersService parserService)
        {
            _parserService = parserService;
        }

        public async Task GetNewSalesOrdersAsync()
        {
            NewSalesOrders.Clear();

            var newSalesOrders = await _parserService.GetNewSalesOrdersAsync();

            foreach (var order in newSalesOrders.OrderBy(so => so.EarliestDeliveryDate))
            {
                NewSalesOrders.Add(order);
            }
        }
    }
}
