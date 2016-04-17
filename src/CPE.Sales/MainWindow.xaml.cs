﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as SalesOrderListViewModel;

            model.GetSalesOrdersAsync();
        }
    }
}