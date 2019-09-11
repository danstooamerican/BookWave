using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Models.AudiobookPlayer;
using BookWave.Desktop.Notifications;
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

        public double MaxSeconds { get { return player.MaxSeconds; } }

        public double SecondsPlayed
        {
            get
            {
                try
                {
                    return player.SecondsPlayed;
                }
                catch (ChapterNotFoundException ex)
                {
                    HandleChapterNotFoundException(ex);
                }

                return 0;
            }
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

        public ICommand RewindCommand { private set; get; }

        public ICommand ForwardCommand { private set; get; }

        #endregion

        #region Constructor

        public PlayerViewModel()
        {
            timeLineUpdater = new DispatcherTimer();

            // update timeLine with 30 FPS
            timeLineUpdater.Interval = TimeSpan.FromMilliseconds(1000d/30);

            timeLineUpdater.Tick += (e, a) => 
            {
                try
                {
                    RaisePropertyChanged(nameof(SecondsPlayed));
                }
                catch (ChapterNotFoundException ex)
                {
                    HandleChapterNotFoundException(ex);
                }
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
            RewindCommand = new RelayCommand(Rewind);
            ForwardCommand = new RelayCommand(Forward);
        }        

        #endregion

        #region Methods

        private void OnPlaybackStarted(object sender, object args)
        {
            timeLineUpdater.Start();
            RaisePropertyChanged(nameof(SecondsPlayed));
            RaisePropertyChanged(nameof(IsPlaying));            
        }

        private void OnPlaybackStopped(object sender, object args)
        {
            timeLineUpdater.Stop();
            RaisePropertyChanged(nameof(SecondsPlayed));
            RaisePropertyChanged(nameof(IsPlaying));            
        }

        public void SelectAudiobook(Audiobook audiobook)
        {
            try
            {
                player.SelectAudiobook(audiobook);
            } catch (ChapterNotFoundException ex)
            {
                HandleChapterNotFoundException(ex);
            }
            
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
            try
            {
                player.SkipToStart();
            }
            catch (ChapterNotFoundException ex)
            {
                HandleChapterNotFoundException(ex);
            }

            RaisePropertyChanged(nameof(SecondsPlayed));

        }

        public void SkipToEnd()
        {
            try
            {
                player.SkipToEnd();
            }
            catch (ChapterNotFoundException ex)
            {
                HandleChapterNotFoundException(ex);
            }

            RaisePropertyChanged(nameof(SecondsPlayed));
        }        

        public void Forward()
        {
            player.Forward(30);
            RaisePropertyChanged(nameof(SecondsPlayed));
        }

        public void Rewind()
        {
            player.Rewind(30);
            RaisePropertyChanged(nameof(SecondsPlayed));
        }

        private void HandleChapterNotFoundException(ChapterNotFoundException ex)
        {
            NotificationManager.DisplayDecision("Chapter not found. Do you want to fix it?", () => {
                ViewModelLocator.Instance.MainViewModel.SwitchToEditLibraryPage(Audiobook);
            });
        }

        #endregion
    }
}
