using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Repositories;
using CPE.Sales.ViewModels;
using Syncfusion.Windows.Shared;

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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}