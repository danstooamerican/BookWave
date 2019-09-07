using BookWave.Desktop.Models.AudiobookManagement;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace BookWave.Desktop.Models.AudiobookPlayer
{
    public class Player
    {

        #region Properties

        private Audiobook mAudiobook;
        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            private set
            {
                mAudiobook = value;
                if (mAudiobook != null)
                {
                    chapters = new SortedSet<Chapter>(mAudiobook.Chapters);
                }                
            }
        }

        private bool mIsPlaying;

        public bool IsPlaying
        {
            get { return mIsPlaying; }
            private set { mIsPlaying = value; }
        }

        public int MaxSeconds
        {
            get
            {
                if (mediaReader != null)
                {
                    return (int)mediaReader.TotalTime.TotalSeconds;
                }
                return 0;                
            }
        }

        public double SecondsPlayed
        {
            get
            {
                if (mediaReader != null)
                {
                    return mediaReader.CurrentTime.TotalSeconds;
                }
                return 0;
            }
            set
            {
                if (mediaReader != null)
                {
                    mediaReader.CurrentTime = TimeSpan.FromSeconds(value);
                }                
            }
        }

        private float mVolume;
        public float Volume
        {
            get
            {
                return mVolume;
            }
            set
            {
                mVolume = value;
                if (mediaPlayer != null)
                {
                    mediaPlayer.Volume = value;
                }
            }
        }

        private int mCurrentChapterIndex;
        private int CurrentChapterIndex
        {
            get
            {
                return mCurrentChapterIndex;
            }
            set
            {
                if (Audiobook != null && value < Audiobook.Chapters.Count)
                {
                    mCurrentChapterIndex = value;
                    LoadChapter();
                    ChapterChangedEvent?.Invoke(this, null);
                }
                else
                {
                    mCurrentChapterIndex = -1;
                }                
            }
        }

        private SortedSet<Chapter> chapters;
        public Chapter CurrentChapter
        {
            get
            {
                if (Audiobook != null)
                {
                    if (CurrentChapterIndex >= 0 && CurrentChapterIndex < chapters.Count)
                    {
                        return chapters.ElementAt(CurrentChapterIndex);
                    }                    
                }
                return null;
            }
        }


        private IWavePlayer mediaPlayer;

        private WaveStream mediaReader;

        #endregion

        #region Events

        public event EventHandler PlaybackStoppedEvent;

        public event EventHandler PlaybackStartedEvent;

        public event EventHandler PlaybackPausedEvent;

        public event EventHandler ChapterChangedEvent;

        #endregion

        #region Constructors

        public Player()
        {
            IsPlaying = false;
            Volume = 1;
            CurrentChapterIndex = 0;
        }

        #endregion

        #region Methods

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {         
            if (sender == mediaPlayer)
            {
                PlaybackStoppedEvent?.Invoke(this, null);

                CurrentChapterIndex++;
                Play();
            }            
        }

        public void SelectAudiobook(Audiobook audiobook)
        {
            Audiobook = audiobook;

            if (Audiobook != null)
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Stop();
                    mediaPlayer.Dispose();
                }
                if (mediaReader != null)
                {
                    mediaReader.Dispose();
                    mediaReader = null;
                }

                IsPlaying = false;
                mediaPlayer = new WaveOutEvent();
                mediaPlayer.Volume = mVolume;
                mediaPlayer.PlaybackStopped += OnPlaybackStopped;

                CurrentChapterIndex = 0;
            }
            else
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Dispose();
                }
                if (mediaReader != null)
                {
                    mediaReader.Dispose();
                }                
            }           
        }

        private void LoadChapter()
        {
            if (mediaPlayer != null)
            {
                SecondsPlayed = 0;
                mediaReader = new MediaFoundationReader(CurrentChapter.AudioPath.Path);
                mediaPlayer.Init(mediaReader);
            }            
        }

        public void TogglePlay(bool isPlayingUpdate = true)
        {
            if (IsPlaying)
            {
                Pause(isPlayingUpdate);
            }
            else
            {
                Play(isPlayingUpdate);
            }
        }

        public void Play(bool isPlayingUpdate = true)
        {
            if (Audiobook != null)
            {
                if (CurrentChapter != null)
                {
                    mediaPlayer.Play();

                    if (isPlayingUpdate)
                    {
                        IsPlaying = true;
                    }

                    PlaybackStartedEvent?.Invoke(this, null);
                }                
            }            
        }

        public void Pause(bool isPlayingUpdate = true)
        {
            if (Audiobook != null)
            {
                mediaPlayer.Pause();

                if (isPlayingUpdate)
                {
                    IsPlaying = false;
                }

                PlaybackPausedEvent?.Invoke(this, null);
            }            
        }

        public void Stop(bool isPlayingUpdate = true)
        {
            if (Audiobook != null)
            {
                mediaPlayer.Stop();
            }
        }

        public void SkipToStart()
        {
            SecondsPlayed = 0;
        }

        public void SkipToEnd()
        {
            SecondsPlayed = MaxSeconds;
        }

        public void Rewind30()
        {
            SecondsPlayed = Math.Max(0, SecondsPlayed - 30);
        }

        public void Forward30()
        {
            SecondsPlayed = Math.Min(SecondsPlayed + 30, MaxSeconds);
        }

        #endregion

    }
}
