using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using Syncfusion.UI.Xaml.Spreadsheet;
using Syncfusion.Windows.Controls.Spreadsheet;
using Syncfusion.XlsIO;

namespace CPE.Sales.Views
{
    /// <summary>
    /// Interaction logic for OpenOrderReportView.xaml
    /// </summary>
    public partial class OpenOrderReportView : ViewBase
    {
        public OpenOrderReportView()
        {
            InitializeComponent();

            if (IsInDesignMode)
            {
                return;
            }
        }
    }
}
