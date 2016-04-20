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
    /// Interaction logic for NewSalesOrdersGridView.xaml
    /// </summary>
    public partial class NewSalesOrdersGridView : ViewBase
    {
        public NewSalesOrdersGridView()
        {
            InitializeComponent();
        }

        private async void NewSalesOrdersGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }

            var model = DataContext as NewSalesOrdersViewModel;

            await model.GetNewSalesOrdersAsync();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NewSalesOrdersViewModel;

            model.DueThisMonthOnly = true;
        }

        private void CurrentMonthOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NewSalesOrdersViewModel;

            model.DueThisMonthOnly = false;
        }
    }
}
