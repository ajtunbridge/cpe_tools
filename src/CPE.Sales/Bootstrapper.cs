using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CPE.Data.EntityFramework.Repositories;
using CPE.Domain.Services;
using CPE.Sales.Services;
using CPE.Sales.ViewModels;
using CPE.Sales.ViewModels.Settings;
using Ninject;

namespace CPE.Sales
{
    public class Bootstrapper
    {
        private static readonly StandardKernel Kernel;

        static Bootstrapper()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<MainViewModel>().ToSelf();

            Kernel.Bind<SalesOrderListViewModel>().ToSelf();
            Kernel.Bind<CustomerSalesOrderParserSettingsViewModel>().ToSelf();
            Kernel.Bind<PathSettingsViewModel>().ToSelf();

            Kernel.Bind<ICustomerService>().To<CustomerRepository>();
            Kernel.Bind<IPartService>().To<PartRepository>();

            Kernel.Bind<SalesOrderParserService>().ToSelf().InSingletonScope();
            Kernel.Bind<OpenOrderReportParserService>().ToSelf().InSingletonScope();
        }

        public SalesOrderListViewModel MailListViewModel => Kernel.Get<SalesOrderListViewModel>();

        public CustomerSalesOrderParserSettingsViewModel ParserSettingsViewModel
            => Kernel.Get<CustomerSalesOrderParserSettingsViewModel>();

        public PathSettingsViewModel PathSettingsViewModel
            => Kernel.Get<PathSettingsViewModel>();

        public MainViewModel MainModel => Kernel.Get<MainViewModel>();

        public SalesOrderViewModel SalesOrderViewModel => Kernel.Get<SalesOrderViewModel>();

        public static void Cleanup()
        {
            
        }
    }
}
