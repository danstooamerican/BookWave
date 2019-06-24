using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace Commons.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static ViewModelLocator mInstance;
        public static ViewModelLocator Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ViewModelLocator();
                }
                return mInstance;
            }
        }

        private ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<EditLibraryViewModel>();
            SimpleIoc.Default.Register<SelectLibraryViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
        }

        public MainWindowViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public SelectLibraryViewModel SelectLibraryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SelectLibraryViewModel>();
            }
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayerViewModel>();
            }
        }

        public EditLibraryViewModel EditLibraryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditLibraryViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}