﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CPE.Data.EntityFramework.Repositories;
using CPE.Domain.Services;
using CPE.Sales.Services;
using CPE.Sales.ViewModels;
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

            Kernel.Bind<ICustomerService>().To<CustomerRepository>();

            Kernel.Bind<SalesOrderParserService>().ToSelf();
        }

        public SalesOrderListViewModel MailListViewModel => Kernel.Get<SalesOrderListViewModel>();

        public MainViewModel MainModel => Kernel.Get<MainViewModel>();

        public static SalesOrderParserService Parser => Kernel.Get<SalesOrderParserService>();

        public static ICustomerService Customers => Kernel.Get<ICustomerService>();

        public static void Cleanup()
        {
            
        }
    }
}
