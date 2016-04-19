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
    /// Interaction logic for SalesOrderView.xaml
    /// </summary>
    public partial class SalesOrderView : UserControl
    {
        public SalesOrderView()
        {
            InitializeComponent();
        }

        public void DisplaySalesOrder(SalesOrderListViewModel.SalesOrder order)
        {
            var model = DataContext as SalesOrderViewModel;

            model.RetrieveLinesAsync(order);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLine = ItemsList.SelectedItem as SalesOrderViewModel.LineDetail;
            
            var model = DataContext as SalesOrderViewModel;


            model.ShowItem(selectedLine);
        }
    }
}
