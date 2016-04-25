using CPE.Data.EntityFramework.Model;
using CPE.Data.EntityFramework.Model.Tricorn;
using CPE.Data.EntityFramework.Repositories;
using CPE.Data.EntityFramework.Repositories.Tricorn;
using CPE.Domain.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CPE.PartPhotoManager.ViewModels
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            
            SimpleIoc.Default.Register<IPartService, PartRepository>();
            SimpleIoc.Default.Register<IPartVersionService, PartVersionRepository>();
            SimpleIoc.Default.Register<IPhotoService, PhotoRepository>();
            SimpleIoc.Default.Register<ITricornService, TricornRepository>();

            SimpleIoc.Default.Register<CPEEntities>();
            SimpleIoc.Default.Register<TricornEntities>();

            SimpleIoc.Default.Register<PartSearchViewModel>();
        }

        public PartSearchViewModel PartSearchViewModel
            => SimpleIoc.Default.GetInstance<PartSearchViewModel>();
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}