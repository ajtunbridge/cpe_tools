using System.Collections.Generic;
using System.IO;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.ViewModel
{
    public class SalesOrderViewModel : ViewModelBase
    {
        private SalesOrder _currentSalesOrder;

        public SalesOrderViewModel()
        {
            Messenger.Default.Register<SalesOrderSelectedMessage>(this, HandleSalesOrderSelectedMessage);
        }

        public string SalesOrderFileName
        {
            get
            {
                if (_currentSalesOrder == null)
                {
                    return null;
                }

                foreach (var attachment in _currentSalesOrder.Mail.ExtractedAttachments)
                {
                    if (File.Exists(attachment) && Path.GetExtension(attachment).ToLower() == ".pdf")
                    {
                        return attachment;
                    }
                }

                return null;
            }
        }

        public List<SalesOrderLine> SalesOrderLines => _currentSalesOrder?.Lines;

        public string HeaderText
        {
            get
            {
                if (_currentSalesOrder == null)
                {
                    return "No sales order currently selected!";
                }

                return
                    string.Format(
                        "Currently viewing order number {0} from {1} at {2}.\nThe earliest delivery date on this order is {3:d}\nThe total value of this order is {4:C}",
                        _currentSalesOrder.OrderNumber,
                        _currentSalesOrder.Buyer,
                        _currentSalesOrder.CustomerName,
                        _currentSalesOrder.EarliestDeliveryDate,
                        _currentSalesOrder.TotalValue);
            }
        }

        private void HandleSalesOrderSelectedMessage(SalesOrderSelectedMessage message)
        {
            _currentSalesOrder = message.SelectedSalesOrder;

            RaisePropertyChanged("SalesOrderMail");
            RaisePropertyChanged("SalesOrderFileName");
            RaisePropertyChanged("SalesOrderLines");
            RaisePropertyChanged("HeaderText");
        }
    }
}