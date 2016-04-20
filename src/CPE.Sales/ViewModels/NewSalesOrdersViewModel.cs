using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPE.Sales.Models;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class NewSalesOrdersViewModel : ViewModelBase
    {
        private readonly NewSalesOrdersService _parserService;
        private List<NewSalesOrder> _allNewSaleOrders = new List<NewSalesOrder>();

        private bool _dueThisMonthOnly;

        public NewSalesOrdersViewModel(NewSalesOrdersService parserService)
        {
            _parserService = parserService;
        }

        public IEnumerable<NewSalesOrder> FilteredSalesOrders
        {
            get
            {
                var filtered = new List<NewSalesOrder>();

                if (DueThisMonthOnly)
                {
                    var currentMonth = DateTime.Today.Month;
                    var currentYear = DateTime.Today.Year;

                    foreach (var order in _allNewSaleOrders.Where(o => o.Lines.Any(l =>
                        l.OriginalDeliveryDate.Month <= currentMonth &&
                        l.OriginalDeliveryDate.Year <= currentYear)))
                    {
                        filtered.Add(order);
                    }
                }
                else
                {
                    filtered = _allNewSaleOrders;
                }

                return filtered.OrderBy(o => o.EarliestDeliveryDate);
            }
        }

        public bool DueThisMonthOnly
        {
            get { return _dueThisMonthOnly; }
            set
            {
                _dueThisMonthOnly = value;
                OnPropertyChanged("FilteredSalesOrders");
                OnPropertyChanged("ViewHeader");
            }
        }
        
        public string ViewHeader
        {
            get
            {
                return
                    $"{FilteredSalesOrders.Count()} sales orders totalling {FilteredSalesOrders.Sum(so => so.TotalValue).ToString("C")}";
            }
        }

        public async Task GetNewSalesOrdersAsync()
        {
            _allNewSaleOrders = await _parserService.GetNewSalesOrdersAsync();

            OnPropertyChanged("FilteredSalesOrders");
            OnPropertyChanged("ViewHeader");
        }
    }
}