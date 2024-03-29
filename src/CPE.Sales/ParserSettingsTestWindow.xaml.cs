﻿using System;
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
using System.Windows.Shapes;
using CPE.Sales.Services;
using CPE.Sales.ViewModel;

namespace CPE.Sales
{
    /// <summary>
    /// Interaction logic for ParserSettingsTestWindow.xaml
    /// </summary>
    public partial class ParserSettingsTestWindow : Window
    {
        public ParserSettingsTestWindow()
        {
            InitializeComponent();
        }
        
        private async void ScanNowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ParserSettingsTestViewModel;
            
                await model.PerformTestAsync(OrderFolderName.Text);
            
        }

        private async void CheckTricornButton_OnClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ParserSettingsTestViewModel;

            await model.CheckTricornConnection();
        }

        private async void CheckCpeCentralButton_OnClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ParserSettingsTestViewModel;

            await model.CheckCpeCentralConnection();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new FirstRunWindow().ShowDialog();
        }
    }
}
