using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Models.AudiobookPlayer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;
using System.Windows.Threading;

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

        public bool IsPlaying
        {
            get { return player.IsPlaying; }
            set
            {
                if (value)
                {
                    player.Play(false);
                }
                else
                {
                    player.Pause(false);
                }
            }
        }

        public int MaxSeconds { get { return player.MaxSeconds; } }

        public int SecondsPlayed
        {
            get { return player.SecondsPlayed; }
            set
            {
                player.SecondsPlayed = value;
                RaisePropertyChanged(nameof(SecondsPlayed));
            }
        }

        public float Volume
        {
            get { return player.Volume; }
            set
            {
                player.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public Audiobook Audiobook { get { return player.Audiobook; } }

        public Chapter CurrentChapter { get { return player.CurrentChapter; } }

        private Player player;

        private DispatcherTimer timeLineUpdater;

        #endregion

        #region Commands

        public ICommand TogglePlayCommand { private set; get; }

        public ICommand SkipToEndCommand { private set; get; }

        public ICommand SkipToStartCommand { private set; get; }

        public ICommand Rewind30Command { private set; get; }

        public ICommand Forward30Command { private set; get; }

        #endregion

        #region Constructor

        public PlayerViewModel()
        {
            timeLineUpdater = new DispatcherTimer();
            timeLineUpdater.Interval = TimeSpan.FromSeconds(1);
            timeLineUpdater.Tick += (e, a) => 
            {
                RaisePropertyChanged(nameof(SecondsPlayed));
            };

            player = new Player();
            player.PlaybackStoppedEvent += OnPlaybackStopped;
            player.PlaybackStartedEvent += OnPlaybackStarted;
            player.PlaybackPausedEvent += (e, a) => { timeLineUpdater.Stop(); };
            player.ChapterChangedEvent += (e, a) =>
            {
                RaisePropertyChanged(nameof(MaxSeconds));
                RaisePropertyChanged(nameof(CurrentChapter));
            };

            TogglePlayCommand = new RelayCommand(TogglePlay);
            SkipToStartCommand = new RelayCommand(SkipToStart);
            SkipToEndCommand = new RelayCommand(SkipToEnd);
            Rewind30Command = new RelayCommand(Rewind30);
            Forward30Command = new RelayCommand(Forward30);
        }        

        #endregion

        #region Methods

        private void OnPlaybackStarted(object sender, object args)
        {
            RaisePropertyChanged(nameof(SecondsPlayed));
            RaisePropertyChanged(nameof(IsPlaying));
            timeLineUpdater.Start();
        }

        private void OnPlaybackStopped(object sender, object args)
        {
            RaisePropertyChanged(nameof(SecondsPlayed));
            RaisePropertyChanged(nameof(IsPlaying));
            timeLineUpdater.Stop();
        }

        public void SelectAudiobook(Audiobook audiobook)
        {
            player.SelectAudiobook(audiobook);
            RaisePropertyChanged(nameof(CoverImage));
            RaisePropertyChanged(nameof(MaxSeconds));
            RaisePropertyChanged(nameof(SecondsPlayed));
            RaisePropertyChanged(nameof(Audiobook));
            RaisePropertyChanged(nameof(CurrentChapter));
        }
        public void TogglePlay()
        {
            player.TogglePlay();
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void SkipToStart()
        {
            player.SkipToStart();
            RaisePropertyChanged(nameof(SecondsPlayed));

        }

        public void SkipToEnd()
        {
            player.SkipToEnd();
            RaisePropertyChanged(nameof(SecondsPlayed));
        }        

        public void Forward30()
        {
            player.Forward30();
            RaisePropertyChanged(nameof(SecondsPlayed));
        }

        public void Rewind30()
        {
            player.Rewind30();
            RaisePropertyChanged(nameof(SecondsPlayed));
        }

        #endregion
    }
}
