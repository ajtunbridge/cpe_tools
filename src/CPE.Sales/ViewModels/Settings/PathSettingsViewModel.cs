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
            get { return CPE.Sales.Properties.Settings.Default.NewOrdersFolderName; }
            set
            {
                CPE.Sales.Properties.Settings.Default.NewOrdersFolderName = value;
                CPE.Sales.Properties.Settings.Default.Save();
            }
        }
}
}
