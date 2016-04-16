using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using CPE.Domain.Model;
using CPE.Domain.Services;
using CPE.Sales.Services;
using CPE.Sales.ViewModels;

namespace CPE.Sales
{
    /// <summary>
    /// Interaction logic for MainWindows.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SalesOrderParserService _parser;

        public MainWindow()
        {
            InitializeComponent();

            _parser = Bootstrapper.Parser;
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var salesOrders = MSOutlookService.GetSalesOrderMail();

            var reportParser = new OpenOrderReportParserService();
            var orderParser = new SalesOrderParserService(new CustomerRepository(new CPEEntities()));

            foreach (var so in salesOrders)
            {
                var orderNumber = await orderParser.ParseOrderNumberAsync(so);

                DateTime rescheduleDate = DateTime.MinValue;

                var isRescheduled = reportParser.HasBeenRescheduled(orderNumber, orderNumber, out rescheduleDate);

                if (isRescheduled)
                {
                    MessageBox.Show(string.Format("{0} has been rescheduled out to {1:d}", orderNumber, rescheduleDate));
                }
            }
        }
    }
}
