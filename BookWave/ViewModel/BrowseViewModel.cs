using BookWave.Desktop.AudiobookManagement;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace BookWave.ViewModel
{
    public class BrowseViewModel : BrowseViewModelBase
    {

        #region Commands

        public ICommand EditSelectedCommand { private set; get; }

        #endregion

        #region Constructor

        public BrowseViewModel() : base()
        {
            EditSelectedCommand = new RelayCommand<Audiobook>((a) => EditSelected(a));
        }

        #endregion

        #region Methods

        private void EditSelected(Audiobook audiobook)
        {
            ViewModelLocator.Instance.MainViewModel.SwitchToEditLibraryPage(audiobook);
        }

        #endregion

    }
}
