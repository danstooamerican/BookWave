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

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<AddPageViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
        }

        public MainWindowViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayerViewModel>();
            }
        }

        public AddPageViewModel AddPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddPageViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}