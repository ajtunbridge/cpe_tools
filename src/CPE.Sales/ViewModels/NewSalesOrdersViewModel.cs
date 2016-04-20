using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPE.Domain.Services;
using CPE.Sales.Models;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class NewSalesOrdersViewModel : ViewModelBase
    {
        private readonly NewSalesOrdersService _parserService;
        private readonly ITricornService _tricorn;

        private List<NewSalesOrder> _allNewSaleOrders = new List<NewSalesOrder>();

        private bool _dueThisMonthOnly;

        public NewSalesOrdersViewModel(NewSalesOrdersService parserService, ITricornService tricornService)
        {
            _parserService = parserService;
            _tricorn = tricornService;
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
                    $"Currently showing {FilteredSalesOrders.Count()} sales orders with a total value of {FilteredSalesOrders.Sum(so => so.TotalValue).ToString("C")}";
            }
        }

        public async Task GetNewSalesOrdersAsync()
        {
            _allNewSaleOrders = new List<NewSalesOrder>();
            OnPropertyChanged("FilteredSalesOrders");

            _allNewSaleOrders = await _parserService.GetNewSalesOrdersAsync();

            OnPropertyChanged("FilteredSalesOrders");
            OnPropertyChanged("ViewHeader");
        }
    }
}