using GalaSoft.MvvmLight;

namespace CPE.Sales.ViewModel.Settings
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
            get { return Properties.Settings.Default.OpenOrderReportsFolderId; }
            set
            {
                Properties.Settings.Default.OpenOrderReportsFolderId = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}