using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SalesOrders = new ObservableCollection<MSOutlookMailItem>();
        }

        public ObservableCollection<MSOutlookMailItem> SalesOrders { get; set; }

        public async Task GetSalesOrdersAsync()
        {
            var salesOrders = await Task.Factory.StartNew(() => MSOutlookService.GetSalesOrderMail());

            foreach (var so in salesOrders)
            {
                SalesOrders.Add(so);
            }
        }
    }
}