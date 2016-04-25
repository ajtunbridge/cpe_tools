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
using CPE.PartPhotoManager.ViewModels;

namespace CPE.PartPhotoManager.Views
{
    /// <summary>
    /// Interaction logic for PartSearchView.xaml
    /// </summary>
    public partial class PartSearchView
    {
        public PartSearchView()
        {
            InitializeComponent();
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            var model = DataContext as PartSearchViewModel;

            await model.PerformSearchAsync(WorksOrderNumberEdit.Text);
        }
    }
}
