﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPE.Domain.Services;
using CPE.Sales;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using CPE.Sales.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.ViewModel
{
    public class NewSalesOrdersViewModel : ViewModelBase
    {
        private readonly NewSalesOrdersService _parserService;
        private readonly ITricornService _tricorn;
        private List<SalesOrder> _allNewSaleOrders = new List<SalesOrder>();
        private bool _showDueThisMonthOnly;
        private SalesOrder _selectedSalesOrder;
        private decimal _totalValueOfOrdersOnSystem;

        public NewSalesOrdersViewModel(NewSalesOrdersService parserService, ITricornService tricornService)
        {
            _parserService = parserService;
            _tricorn = tricornService;
        }

        public IEnumerable<SalesOrder> FilteredSalesOrders
        {
            get
            {
                var filtered = new List<SalesOrder>();

                if (ShowDueThisMonthOnly)
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

        public bool ShowDueThisMonthOnly
        {
            get { return _showDueThisMonthOnly; }
            set
            {
                _showDueThisMonthOnly = value;
                RaisePropertyChanged("FilteredSalesOrders");
                RaisePropertyChanged("ViewHeader");
            }
        }

        public string ViewHeader
        {
            get
            {
                return $"Currently showing {FilteredSalesOrders.Count()} sales orders with a total value of {FilteredSalesOrders.Sum(so => so.TotalValue).ToString("C")}";
            }
        }

        public decimal TotalValueOfOrdersOnSystem
        {
            get { return _totalValueOfOrdersOnSystem; }
            set
            {
                _totalValueOfOrdersOnSystem = value;
                RaisePropertyChanged();
            }
        }

        public SalesOrder SelectedSalesOrder
        {
            get { return _selectedSalesOrder; }
            set
            {
                _selectedSalesOrder = value;
                Messenger.Default.Send(new SalesOrderSelectedMessage(value));
            }
        }

        public async Task GetNewSalesOrdersAsync()
        {
            _allNewSaleOrders = new List<SalesOrder>();
            RaisePropertyChanged("FilteredSalesOrders");

            var folder = Properties.Settings.Default.NewOrdersFolderName;

            _allNewSaleOrders = await _parserService.GetNewSalesOrdersAsync(folder);
            TotalValueOfOrdersOnSystem = await _tricorn.GetTotalValueOfJobsForCurrentMonthAsync();
        
            RaisePropertyChanged("FilteredSalesOrders");
            RaisePropertyChanged("ViewHeader");
        }
    }
}