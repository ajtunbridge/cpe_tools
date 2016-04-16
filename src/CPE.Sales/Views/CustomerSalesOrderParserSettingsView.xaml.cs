using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CPE.Sales.ViewModels;

namespace CPE.Sales.Views
{
    /// <summary>
    /// Interaction logic for CustomerSalesOrderParserSettingsView.xaml
    /// </summary>
    public partial class CustomerSalesOrderParserSettingsView : UserControl
    {
        public CustomerSalesOrderParserSettingsView()
        {
            InitializeComponent();
        }

        private CustomerSalesOrderParserSettingsViewModel GetModel()
        {
            return DataContext as CustomerSalesOrderParserSettingsViewModel;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await GetModel().SaveSettingsAsync();
        }
    }
}
