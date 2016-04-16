using System.ComponentModel;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Sales.ViewModels
{
    public class CustomerSalesOrderParserSettingsViewModel : INotifyPropertyChanged
    {
        private readonly ICustomerService _customers;
        private bool _changesMade;
        private ICustomer _currentCustomer;
        private SalesOrderParserSettingsBlob _parserSettings = new SalesOrderParserSettingsBlob();

        public CustomerSalesOrderParserSettingsViewModel(ICustomerService customers)
        {
            _customers = customers;
        }

        public ICustomer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                _parserSettings = value.GetSalesOrderParserSettings();

                if (_parserSettings == null)
                    _parserSettings = new SalesOrderParserSettingsBlob();

                OnPropertyChanged("IdentifierExpression");
                OnPropertyChanged("BuyerExpression");
                OnPropertyChanged("DeliveryDateExpression");
                OnPropertyChanged("DrawingNumberExpression");
                OnPropertyChanged("OrderNumberExpression");
                OnPropertyChanged("MultiLineExpression");

                _currentCustomer = value;
            }
        }

        public string IdentifierExpression
        {
            get { return _parserSettings.CustomerIdentifierExpr; }
            set
            {
                _parserSettings.CustomerIdentifierExpr = value;
                ChangesMade = true;
            }
        }

        public string BuyerExpression
        {
            get { return _parserSettings.BuyerExpr; }
            set
            {
                _parserSettings.BuyerExpr = value;
                ChangesMade = true;
            }
        }

        public string DeliveryDateExpression
        {
            get { return _parserSettings.DeliveryDateExpr; }
            set
            {
                _parserSettings.DeliveryDateExpr = value;
                ChangesMade = true;
            }
        }

        public string DrawingNumberExpression
        {
            get { return _parserSettings.DrawingNumberExpr; }
            set
            {
                _parserSettings.DrawingNumberExpr = value;
                ChangesMade = true;
            }
        }

        public string OrderNumberExpression
        {
            get { return _parserSettings.OrderNumberIdentifierExpr; }
            set
            {
                _parserSettings.OrderNumberIdentifierExpr = value;
                ChangesMade = true;
            }
        }

        public string MultiLineOrderExpression
        {
            get { return _parserSettings.MultiLineDrawingNumberAndDeliveryExpr; }
            set
            {
                _parserSettings.MultiLineDrawingNumberAndDeliveryExpr = value;
                ChangesMade = true;
            }
        }

        public bool ChangesMade
        {
            get { return _changesMade; }
            set
            {
                _changesMade = value;
                OnPropertyChanged("ChangesMade");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveSettingsAsync()
        {
            _currentCustomer.SetSalesOrderParserSettings(_parserSettings);

            _customers.Update(_currentCustomer);

            await Task.Factory.StartNew(() => _customers.Commit());

            ChangesMade = false;
        }
    }
}