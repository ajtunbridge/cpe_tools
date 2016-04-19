using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using CPE.Sales.DataTemplates;
using CPE.Sales.ViewModels;

namespace CPE.Sales
{
    /// <summary>
    ///     Interaction logic for MainWindows.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as SalesOrderListViewModel;

            await model.GetSalesOrdersAsync();
            
            foreach (var item in model.SalesOrders)
            {
                var view = new SalesOrderListDataTemplate();
                view.MouseUp += View_MouseUp;
                view.DataContext = item;
                Stack.Children.Add(view);
            }

            SalesOrderListHeader.Visibility = Visibility.Collapsed;
        }

        private async void View_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tile = sender as SalesOrderListDataTemplate;
            var model = tile.DataContext as SalesOrderListViewModel.SalesOrder;
            var ctx = SalesOrderView.DataContext as SalesOrderViewModel;

            if (model == null || ctx == null)
            {
                return;
            }

            await ctx.RetrieveLinesAsync(model);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}