using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using Syncfusion.Windows.Shared;

namespace CPE.Sales
{
    /// <summary>
    ///     Interaction logic for MainWindows.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void ExportParseSettings()
        {
            using (CustomerRepository customers = new CustomerRepository(new CPEEntities()))
            {
                var parseable = customers.GetSalesOrderParseable();

                foreach (Customer customer in parseable)
                {
                    string fileName = $"{customer.Id}.sops";

                    File.WriteAllBytes(fileName, customer.SalesOrderParserSettings);
                }
            }
        }

        private void ImportParseSettings()
        {
            using (CustomerRepository customers = new CustomerRepository(new CPEEntities()))
            {
                foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
                {
                    var ext = Path.GetExtension(file);

                    if (ext != ".sops")
                    {
                        continue;
                    }

                    var id = int.Parse(Path.GetFileNameWithoutExtension(file));

                    Customer customer = (Customer)customers.GetById(id);

                    customer.SalesOrderParserSettings = File.ReadAllBytes(file);

                    customers.Update(customer);
                }

                customers.Commit();
            }
        }
    }
}