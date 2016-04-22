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
using CPE.Sales.Messages;
using CPE.Sales.Models;
using CPE.Sales.ViewModel;
using GalaSoft.MvvmLight.Messaging;

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
            if (IsInDesignMode || AlreadyLoaded)
            {
                return;
            }
            
            AlreadyLoaded = true;
            
            await RefreshViewModel();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NewSalesOrdersViewModel;

            model.ShowDueThisMonthOnly = true;
        }

        private void CurrentMonthOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NewSalesOrdersViewModel;

            model.ShowDueThisMonthOnly = false;
        }

        private async void RescanButton_OnClick(object sender, RoutedEventArgs e)
        {
            await RefreshViewModel();
        }

        private async Task RefreshViewModel()
        {
            RescanButton.IsEnabled = false;
            LoadingPanel.Visibility = Visibility.Visible;
            HeaderText.Visibility = Visibility.Hidden;

            var model = DataContext as NewSalesOrdersViewModel;

            await model.GetNewSalesOrdersAsync();

            RescanButton.IsEnabled = true;
            LoadingPanel.Visibility = Visibility.Hidden;
            HeaderText.Visibility = Visibility.Visible;
        }

        private void DataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            var selectedOrder = DataGrid.SelectedItem as Models.SalesOrder;
            
            var model = DataContext as NewSalesOrdersViewModel;

            model.SelectedSalesOrder = selectedOrder;
        }
    }
}
