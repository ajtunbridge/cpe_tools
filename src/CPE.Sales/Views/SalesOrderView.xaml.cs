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
using GalaSoft.MvvmLight.Messaging;
using Syncfusion.Windows.PdfViewer;

namespace CPE.Sales.Views
{
    /// <summary>
    /// Interaction logic for SalesOrderView.xaml
    /// </summary>
    public partial class SalesOrderView : ViewBase
    {
        public SalesOrderView()
        {
            InitializeComponent();

            if (IsInDesignMode)
            {
                return;
            }

            AlreadyLoaded = true;

            var pdfViewer = new PdfViewerControl();
            pdfViewer.ZoomMode = ZoomMode.FitWidth;

            Binding myBinding = new Binding();
            myBinding.Source = DataContext;
            myBinding.Path = new PropertyPath("SalesOrderFileName");
            myBinding.Mode = BindingMode.OneWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            BindingOperations.SetBinding(pdfViewer, PdfViewerControl.ItemSourceProperty, myBinding);

            SalesOrderTabItem.Content = pdfViewer;
        }

        private void SalesOrderView_OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalesOrderLinesList.SelectedItem == null)
            {
                return;
            }

            var selectedLine = SalesOrderLinesList.SelectedItem as SalesOrderLine;

            Messenger.Default.Send(new SalesOrderLineSelectedMessage(selectedLine));
        }

        private void FrameworkElement_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (SalesOrderLinesList.Items.Count > 0)
            {
                SalesOrderLinesList.SelectedIndex = 0;
            }
        }
    }
}
