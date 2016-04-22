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
using Syncfusion.Windows.PdfViewer;

namespace CPE.Sales.Views.SalesOrder
{
    /// <summary>
    /// Interaction logic for FindDrawingView.xaml
    /// </summary>
    public partial class FindDrawingView : ViewBase
    {
        private string _searchValue;

        public FindDrawingView()
        {
            InitializeComponent();

            if (IsInDesignMode)
            {
                return;
            }
            
            var pdfViewer = new PdfViewerControl();
            pdfViewer.ZoomMode = ZoomMode.FitWidth;
            pdfViewer.FontSize = 14;

            Binding myBinding = new Binding();
            myBinding.Source = DataContext;
            myBinding.Path = new PropertyPath("SelectedFile.FullPath");
            myBinding.Mode = BindingMode.OneWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            BindingOperations.SetBinding(pdfViewer, PdfViewerControl.ItemSourceProperty, myBinding);

            PdfViewerGroupBox.Content = pdfViewer;

            Messenger.Default.Register<SalesOrderLineSelectedMessage>(this, HandleSalesOrderLineSelectedMessage);
        }

        private void HandleSalesOrderLineSelectedMessage(SalesOrderLineSelectedMessage message)
        {
            _searchValue = message.SelectedSalesOrderLine.DrawingNumber;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchValue))
            {
                return;
            }

            SearchButton.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;

            var model = DataContext as FindDrawingViewModel;

            await model.FindDrawingFilesAsync(_searchValue);

            SearchButton.IsEnabled = true;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void ResultsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsList.SelectedItem == null)
            {
                return;
            }

            var selectedDrawingFile = ResultsList.SelectedItem as DrawingFile;

            var model = DataContext as FindDrawingViewModel;

            model.SelectedFile = selectedDrawingFile;
        }
    }
}
