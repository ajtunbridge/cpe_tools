using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;
using GalaSoft.MvvmLight;

namespace CPE.Sales.ViewModel.Settings
{
    public class CustomerSalesOrderParserSettingsViewModel : ViewModelBase
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

                RaisePropertyChanged("IdentifierExpression");
                RaisePropertyChanged("IdentifierIsSingleLine");
                RaisePropertyChanged("IdentifierIsMultiLine");
                RaisePropertyChanged("IdentifierIsExplicitCapture");

                RaisePropertyChanged("BuyerExpression");
                RaisePropertyChanged("BuyerIsSingleLine");
                RaisePropertyChanged("BuyerIsMultiLine");
                RaisePropertyChanged("BuyerIsExplicitCapture");

                RaisePropertyChanged("DeliveryDateExpression");
                RaisePropertyChanged("DeliveryDateIsSingleLine");
                RaisePropertyChanged("DeliveryDateIsMultiLine");
                RaisePropertyChanged("DeliveryDateIsExplicitCapture");

                RaisePropertyChanged("DrawingNumberExpression");
                RaisePropertyChanged("DrawingNumberIsSingleLine");
                RaisePropertyChanged("DrawingNumberIsMultiLine");
                RaisePropertyChanged("DrawingNumberIsExplicitCapture");

                RaisePropertyChanged("OrderNumberExpression");
                RaisePropertyChanged("OrderNumberIsSingleLine");
                RaisePropertyChanged("OrderNumberIsMultiLine");
                RaisePropertyChanged("OrderNumberIsExplicitCapture");

                RaisePropertyChanged("MultiLineOrderExpression");
                RaisePropertyChanged("MultiLineOrderIsSingleLine");
                RaisePropertyChanged("MultiLineOrderIsMultiLine");
                RaisePropertyChanged("MultiLineOrderIsExplicitCapture");

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

        public bool IdentifierIsSingleLine
        {
            get { return _parserSettings.CustomerIdentifierOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.CustomerIdentifierOptions = value
                    ? _parserSettings.CustomerIdentifierOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.CustomerIdentifierOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool IdentifierIsMultiLine
        {
            get { return _parserSettings.CustomerIdentifierOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.CustomerIdentifierOptions = value
                    ? _parserSettings.CustomerIdentifierOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.CustomerIdentifierOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool IdentifierIsExplicitCapture
        {
            get { return _parserSettings.CustomerIdentifierOptions.HasFlag(RegexOptions.ExplicitCapture); }
            set
            {
                _parserSettings.CustomerIdentifierOptions = value
                    ? _parserSettings.CustomerIdentifierOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.CustomerIdentifierOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

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

        public bool BuyerIsSingleLine
        {
            get { return _parserSettings.BuyerOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.BuyerOptions = value
                    ? _parserSettings.BuyerOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.BuyerOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool BuyerIsMultiLine
        {
            get { return _parserSettings.BuyerOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.BuyerOptions = value
                    ? _parserSettings.BuyerOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.BuyerOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool BuyerIsExplicitCapture
        {
            get { return _parserSettings.BuyerOptions.HasFlag(RegexOptions.ExplicitCapture); }
            set
            {
                _parserSettings.BuyerOptions = value
                    ? _parserSettings.BuyerOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.BuyerOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

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

        public bool DeliveryDateIsSingleLine
        {
            get { return _parserSettings.DeliveryDateOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.DeliveryDateOptions = value
                    ? _parserSettings.DeliveryDateOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.DeliveryDateOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool DeliveryDateIsMultiLine
        {
            get { return _parserSettings.DeliveryDateOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.DeliveryDateOptions = value
                    ? _parserSettings.DeliveryDateOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.DeliveryDateOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool DeliveryDateIsExplicitCapture
        {
            get { return _parserSettings.DeliveryDateOptions.HasFlag(RegexOptions.ExplicitCapture); }
            set
            {
                _parserSettings.DeliveryDateOptions = value
                    ? _parserSettings.DeliveryDateOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.DeliveryDateOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

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

        public bool DrawingNumberIsSingleLine
        {
            get { return _parserSettings.DrawingNumberOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.DrawingNumberOptions = value
                    ? _parserSettings.DrawingNumberOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.DrawingNumberOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool DrawingNumberIsMultiLine
        {
            get { return _parserSettings.DrawingNumberOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.DrawingNumberOptions = value
                    ? _parserSettings.DrawingNumberOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.DrawingNumberOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool DrawingNumberIsExplicitCapture
        {
            get { return _parserSettings.DrawingNumberOptions.HasFlag(RegexOptions.ExplicitCapture); }
            set
            {
                _parserSettings.DrawingNumberOptions = value
                    ? _parserSettings.DrawingNumberOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.DrawingNumberOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

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

        public bool OrderNumberIsSingleLine
        {
            get { return _parserSettings.OrderNumberOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.OrderNumberOptions = value
                    ? _parserSettings.OrderNumberOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.OrderNumberOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool OrderNumberIsMultiLine
        {
            get { return _parserSettings.OrderNumberOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.OrderNumberOptions = value
                    ? _parserSettings.OrderNumberOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.OrderNumberOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool OrderNumberIsExplicitCapture
        {
            get { return _parserSettings.OrderNumberOptions.HasFlag(RegexOptions.ExplicitCapture); }
            set
            {
                _parserSettings.OrderNumberOptions = value
                    ? _parserSettings.OrderNumberOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.OrderNumberOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

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

        public bool MultiLineOrderIsSingleLine
        {
            get { return _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.HasFlag(RegexOptions.Singleline); }
            set
            {
                _parserSettings.MultiLineDrawingNumberAndDeliveryOptions = value
                    ? _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Add(RegexOptions.Singleline)
                    : _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Remove(RegexOptions.Singleline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool MultiLineOrderIsMultiLine
        {
            get { return _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.HasFlag(RegexOptions.Multiline); }
            set
            {
                _parserSettings.MultiLineDrawingNumberAndDeliveryOptions = value
                    ? _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Add(RegexOptions.Multiline)
                    : _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Remove(RegexOptions.Multiline);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public bool MultiLineOrderIsExplicitCapture
        {
            get
            {
                return _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.HasFlag(RegexOptions.ExplicitCapture);
            }
            set
            {
                _parserSettings.MultiLineDrawingNumberAndDeliveryOptions = value
                    ? _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Add(RegexOptions.ExplicitCapture)
                    : _parserSettings.MultiLineDrawingNumberAndDeliveryOptions.Remove(RegexOptions.ExplicitCapture);

                RaisePropertyChanged();

                ChangesMade = true;
            }
        }

        public ObservableCollection<ICustomer> ParseableCustomers { get; set; } = new ObservableCollection<ICustomer>();

        public bool ChangesMade
        {
            get { return _changesMade; }
            set
            {
                _changesMade = value;
                RaisePropertyChanged("ChangesMade");
            }
        }

        public async Task RetrieveParseableCustomersAsync()
        {
            ParseableCustomers.Clear();

            var parseableCustomers = await _customers.GetSalesOrderParseableAsync();

            foreach (var customer in parseableCustomers.OrderBy(c => c.Name))
            {
                ParseableCustomers.Add(customer);
            }
        }

        public async Task SaveSettingsAsync()
        {
            foreach (var customer in ParseableCustomers)
            {
                var settings = customer.GetSalesOrderParserSettings();
                customer.SetSalesOrderParserSettings(settings);
                _customers.Update(customer);
            }

            await Task.Factory.StartNew(() => _customers.Commit());

            ChangesMade = false;
        }
    }
}