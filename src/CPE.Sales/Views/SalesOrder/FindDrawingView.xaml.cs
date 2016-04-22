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
using CPE.Sales.ViewModel;

namespace CPE.Sales.Views.SalesOrder
{
    /// <summary>
    /// Interaction logic for FindDrawingView.xaml
    /// </summary>
    public partial class FindDrawingView : UserControl
    {
        public FindDrawingView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var searchValue = SearchValue.Text.Trim();

            if (string.IsNullOrEmpty(searchValue))
            {
                return;
            }

            var model = DataContext as FindDrawingViewModel;

            model.FindDrawingFilesAsync(searchValue);
        }
    }
}
