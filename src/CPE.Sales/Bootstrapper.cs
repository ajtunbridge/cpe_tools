using CPE.Data.EntityFramework.Repositories;
using CPE.Data.EntityFramework.Repositories.Tricorn;
using CPE.Domain.Services;
using CPE.Sales.Services;
using CPE.Sales.ViewModel;
using CPE.Sales.ViewModel.Settings;
using Ninject;
using SyncfusionPdfParser;

namespace CPE.Sales
{
    public class Bootstrapper
    {
        private static readonly StandardKernel Kernel;

        static Bootstrapper()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<CustomerSalesOrderParserSettingsViewModel>().ToSelf();
            Kernel.Bind<PathSettingsViewModel>().ToSelf();
            Kernel.Bind<NewSalesOrdersViewModel>().ToSelf();
            Kernel.Bind<SalesOrderViewModel>().ToSelf();
            Kernel.Bind<FindDrawingViewModel>().ToSelf();

            Kernel.Bind<ParserSettingsTestViewModel>().ToSelf();

            Kernel.Bind<ICustomerService>().To<CustomerRepository>();
            Kernel.Bind<IPartService>().To<PartRepository>();
            Kernel.Bind<IPhotoService>().To<PhotoRepository>();
            Kernel.Bind<ITricornService>().To<TricornRepository>();

            Kernel.Bind<IPdfParserService>().To<SfPdfParser>().InSingletonScope();

            Kernel.Bind<SalesOrderParserService>().ToSelf().InSingletonScope();
            Kernel.Bind<OpenOrderReportParserService>().ToSelf().InSingletonScope();
            Kernel.Bind<DrawingFileService>().ToSelf().InSingletonScope();
        }

        public CustomerSalesOrderParserSettingsViewModel ParserSettingsViewModel
            => Kernel.Get<CustomerSalesOrderParserSettingsViewModel>();

        public PathSettingsViewModel PathSettingsViewModel
            => Kernel.Get<PathSettingsViewModel>();

        public NewSalesOrdersViewModel NewSalesOrdersViewModel
            => Kernel.Get<NewSalesOrdersViewModel>();

        public SalesOrderViewModel SalesOrderViewModel
            => Kernel.Get<SalesOrderViewModel>();

        public FindDrawingViewModel FindDrawingViewModel
            => Kernel.Get<FindDrawingViewModel>();

        public ParserSettingsTestViewModel ParserSettingsTestViewModel
            => Kernel.Get<ParserSettingsTestViewModel>();

        public static void Cleanup()
        {
        }
    }
}