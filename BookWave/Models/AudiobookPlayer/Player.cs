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
            set { mediaReader.CurrentTime = TimeSpan.FromSeconds(value); }
        }

        public float Volume
        {
            get { return mediaPlayer.Volume;  }
            set { mediaPlayer.Volume = value; }
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
            mediaPlayer = new WaveOutEvent();
            IsPlaying = false;
            Volume = 1;
            mediaPlayer.PlaybackStopped += OnPlaybackStopped;
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
            Audiobook = audiobook;
            mediaReader = new MediaFoundationReader(audiobook.Chapters.ElementAt(0).AudioPath.Path);

            Stop();
            mediaPlayer.Init(mediaReader);
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
            mediaPlayer.Play();

            if (isPlayingUpdate)
            {
                IsPlaying = true;
            }

            PlaybackStartedEvent();
        }

        public void Pause(bool isPlayingUpdate = true)
        {
            mediaPlayer.Pause();

            if (isPlayingUpdate)
            {
                IsPlaying = false;
            }

            PlaybackPausedEvent();
        }

        public void Stop(bool isPlayingUpdate = true)
        {
            mediaPlayer.Stop();

            if (isPlayingUpdate)
            {
                IsPlaying = false;
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
