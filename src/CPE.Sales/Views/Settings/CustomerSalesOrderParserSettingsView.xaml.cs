﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CPE.Domain.Model;
using CPE.Sales.ViewModels.Settings;

namespace CPE.Sales.Views.Settings
{
    /// <summary>
    /// Interaction logic for CustomerSalesOrderParserSettingsView.xaml
    /// </summary>
    public partial class CustomerSalesOrderParserSettingsView : UserControl
    {
        public CustomerSalesOrderParserSettingsView()
        {
            InitializeComponent();
        }

        private CustomerSalesOrderParserSettingsViewModel GetModel()
        {
            return DataContext as CustomerSalesOrderParserSettingsViewModel;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await GetModel().SaveSettingsAsync();
        }

        private void CustomerSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerSelector.SelectedItem == null)
            {
                return;
            }

            GetModel().CurrentCustomer = CustomerSelector.SelectedItem as ICustomer;
        }

        private async void CustomerSalesOrderParserSettingsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            IsEnabled = false;

            var model = DataContext as CustomerSalesOrderParserSettingsViewModel;

            await model.RetrieveParseableCustomersAsync();

            if (CustomerSelector.Items.Count > 0)
            {
                CustomerSelector.SelectedIndex = 0;
                IsEnabled = true;
            }
        }
    }
}
