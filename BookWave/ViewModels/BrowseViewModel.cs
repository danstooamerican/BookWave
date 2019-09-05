using BookWave.Desktop.Models.AudiobookManagement;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace BookWave.ViewModel
{
    public class BrowseViewModel : BrowseViewModelBase
    {

        #region Commands

        public ICommand EditSelectedCommand { private set; get; }

        public ICommand PlaySelectedCommand { private set; get; }

        #endregion

        #region Constructor

        public BrowseViewModel() : base()
        {
            EditSelectedCommand = new RelayCommand<Audiobook>((a) => EditSelected(a));
            PlaySelectedCommand = new RelayCommand<Audiobook>((a) => PlaySelected(a));
        }

        #endregion

        #region Methods

        private void EditSelected(Audiobook audiobook)
        {
            ViewModelLocator.Instance.MainViewModel.SwitchToEditLibraryPage(audiobook);
        }

        private void PlaySelected(Audiobook audiobook)
        {
            ViewModelLocator.Instance.PlayerViewModel.SelectAudiobook(audiobook);
            ViewModelLocator.Instance.PlayerViewModel.TogglePlay();
        }

        #endregion

    }
}
