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
            private set { mAudiobook = value; }
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
                else
                {
                    return 0;
                }                
            }
        }

        public int SecondsPlayed
        {
            get
            {
                if (mediaReader != null)
                {
                    return (int)mediaReader.CurrentTime.TotalSeconds;
                }
                else
                {
                    return 0;
                }                
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

        private IWavePlayer mediaPlayer;

        private WaveStream mediaReader;

        #endregion

        #region Events

        public delegate void PlaybackStopped();
        public event PlaybackStopped PlaybackStoppedEvent;

        public delegate void PlaybackStarted();
        public event PlaybackStarted PlaybackStartedEvent;

        public delegate void PlaybackPaused();
        public event PlaybackPaused PlaybackPausedEvent;

        #endregion

        #region Constructors

        public Player()
        {
            IsPlaying = false;
            Volume = 1;
        }

        #endregion

        #region Methods

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {            
            Stop();
            SecondsPlayed = 0;
            PlaybackStoppedEvent();
        }

        public void SelectAudiobook(Audiobook audiobook)
        {
            Stop();

            Audiobook = audiobook;

            if (Audiobook != null)
            {
                mediaPlayer = new WaveOutEvent();
                mediaPlayer.Volume = mVolume;
                mediaPlayer.PlaybackStopped += OnPlaybackStopped;

                mediaReader = new MediaFoundationReader(audiobook.Chapters.ElementAt(0).AudioPath.Path);
                mediaPlayer.Init(mediaReader);
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
                mediaPlayer.Play();

                if (isPlayingUpdate)
                {
                    IsPlaying = true;
                }

                PlaybackStartedEvent();
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

                PlaybackPausedEvent();
            }            
        }

        public void Stop(bool isPlayingUpdate = true)
        {
            if (Audiobook != null)
            {
                mediaPlayer.Stop();

                if (isPlayingUpdate)
                {
                    IsPlaying = false;
                }
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
