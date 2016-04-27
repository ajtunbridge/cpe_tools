using System.Collections.Generic;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.ViewModel
{
    public class SalesOrderViewModel : ViewModelBase
    {
        private SalesOrder _currentSalesOrder;

        public string SalesOrderFileName => _currentSalesOrder?.Mail.ExtractedAttachments[0];

        public List<SalesOrderLine> SalesOrderLines => _currentSalesOrder?.Lines;

        public string HeaderText
        {
            get
            {
                if (_currentSalesOrder == null)
                {
                    return "No sales order currently selected!";
                }

                return string.Format("Currently viewing order number {0} from {1} at {2}.\nThe earliest delivery date on this order is {3:d}\nThe total value of this order is {4:C}",
                    _currentSalesOrder.OrderNumber,
                    _currentSalesOrder.Buyer, 
                    _currentSalesOrder.CustomerName,
                    _currentSalesOrder.EarliestDeliveryDate,
                    _currentSalesOrder.TotalValue);
            }
        }
        public SalesOrderViewModel()
        {
            Messenger.Default.Register<SalesOrderSelectedMessage>(this, HandleSalesOrderSelectedMessage);
        }

        private void HandleSalesOrderSelectedMessage(SalesOrderSelectedMessage message)
        {
            _currentSalesOrder = message.SelectedSalesOrder;

            RaisePropertyChanged("SalesOrderFileName");
            RaisePropertyChanged("SalesOrderLines");
            RaisePropertyChanged("HeaderText");
        }
    }
}