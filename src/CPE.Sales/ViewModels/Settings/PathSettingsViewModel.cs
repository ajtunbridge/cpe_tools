using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE.Sales.ViewModels.Settings
{
    public class PathSettingsViewModel : ViewModelBase
    {
        public string NewSalesOrderFolderName
        {
            get { return Properties.Settings.Default.NewOrdersFolderName; }
            set
            {
                Properties.Settings.Default.NewOrdersFolderName = value;
                Properties.Settings.Default.Save();
            }
        }

        public string LaunchedSalesOrderFolderName
        {
            get { return Properties.Settings.Default.LaunchedOrdersFolderName; }
            set
            {
                Properties.Settings.Default.LaunchedOrdersFolderName = value;
                Properties.Settings.Default.Save();
            }
        }

        public string NewOpenOrderReportFolderName
        {
            get { return Properties.Settings.Default.NewOpenOrderReportsFolderName; }
            set
            {
                Properties.Settings.Default.NewOpenOrderReportsFolderName = value;
                Properties.Settings.Default.Save();
            }
        }

        public string CompleteOpenOrderReportFolderName
        {
            get { return Properties.Settings.Default.CompleteOpenOrderReportsFolderName; }
            set
            {
                Properties.Settings.Default.CompleteOpenOrderReportsFolderName = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}