using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Models.AudiobookPlayer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace BookWave.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {

        #region Public Properties

        public string CoverImage
        {
            get
            {
                if (player.Audiobook != null)
                {
                    return player.Audiobook.Metadata.CoverPath;
                } else
                {
                    return AudiobookMetadata.StandardCover;
                }                
            }
        }

        private Player player;

        #endregion

        #region Commands

        public ICommand TogglePlayCommand { private set; get; }

        #endregion

        #region Constructor

        public PlayerViewModel()
        {            
            player = new Player();
            TogglePlayCommand = new RelayCommand(TogglePlay);
        }

        #endregion

        #region Methods

        public void SelectAudiobook(Audiobook audiobook)
        {
            player.SelectAudiobook(audiobook);
            RaisePropertyChanged(nameof(CoverImage));
        }

        private void TogglePlay()
        {
            player.TogglePlay();
        }

        #endregion
    }
}
