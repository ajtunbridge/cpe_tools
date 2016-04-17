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
    /// Interaction logic for SalesOrderMailListView.xaml
    /// </summary>
    public partial class SalesOrdersListView : ViewBase
    {
        public SalesOrdersListView()
        {
            InitializeComponent();
        }

        private async void SalesOrderMailListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }

            var model = DataContext as SalesOrderListViewModel;

            await model.GetSalesOrdersAsync();

            HeaderLabel.Text = "Sales orders to launch";
        }
    }
}
