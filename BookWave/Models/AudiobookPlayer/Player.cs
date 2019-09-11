using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            }
        }

        private bool mIsPlaying;

        public bool IsPlaying
        {
            get { return mIsPlaying; }
            private set { mIsPlaying = value; }
        }

        public double MaxSeconds
        {
            get
            {
                if (mediaReader != null)
                {
                    return Math.Min(mediaReader.TotalTime.TotalSeconds, CurrentChapter.AudioPath.EndMark - CurrentChapter.AudioPath.StartMark);
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
                    double diffToEnd = Math.Min(mediaReader.TotalTime.TotalSeconds, CurrentChapter.AudioPath.EndMark) - mediaReader.CurrentTime.TotalSeconds;
                    if (diffToEnd < 0.01)
                    {
                        NextChapter();
                        PlaybackStoppedEvent?.Invoke(this, null);
                        Play();
                    }
                    return mediaReader.CurrentTime.TotalSeconds - CurrentChapter.AudioPath.StartMark;
                }
                return 0;
            }
            set
            {
                if (mediaReader != null)
                {
                    mediaReader.CurrentTime = TimeSpan.FromSeconds(value + CurrentChapter.AudioPath.StartMark);
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
                // keep value between 0 and 1
                mVolume = Math.Min(Math.Max(value, 0), 1);

                if (mediaPlayer != null)
                {
                    mediaPlayer.Volume = mVolume;
                }

                Settings.Default["Player_Volume"] = Volume;
                Settings.Default.Save();
            }
        }

        public Chapter CurrentChapter
        {
            get
            {
                if (playerList.Current != null)
                {
                    return playerList.Current.Element.Chapter;
                }
                return null;
            }
        }

        private PlayerList<ChapterItem> playerList;

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
            Volume = (float)Settings.Default["Player_Volume"];
            playerList = new PlayerList<ChapterItem>();
        }

        #endregion

        #region Methods

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

                // add chapters from Audiobook as ChapterItems in sorted order to playerList
                playerList.Clear();
                List<ChapterItem> chapterItems = Audiobook.Chapters
                    .Select(c => new ChapterItem(c))
                    .ToList();
                chapterItems.Sort();
                playerList.AddRange(chapterItems);

                LoadCurrentChapter();
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

        private void NextChapter()
        {
            if (mediaPlayer != null)
            {
                playerList.Forward();
                LoadCurrentChapter();
                ChapterChangedEvent?.Invoke(this, null);
            }
        }

        private void PreviousChapter()
        {
            if (mediaPlayer != null)
            {
                playerList.Back();
                LoadCurrentChapter();
                ChapterChangedEvent?.Invoke(this, null);
            }
        }

        private void LoadCurrentChapter()
        {
            Stop();
            mediaReader?.Dispose();
            try
            {
                mediaReader = new MediaFoundationReader(CurrentChapter.AudioPath.Path);
                mediaPlayer.Init(mediaReader);

                mediaReader.CurrentTime = TimeSpan.FromSeconds(CurrentChapter.AudioPath.StartMark);
            }
            catch (FileNotFoundException)
            {
                IsPlaying = false;
                throw new ChapterNotFoundException(CurrentChapter, "was not found.");
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
                if (CurrentChapter != null && !CurrentChapter.AudioPath.PathNotValid)
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
                mediaPlayer?.Pause();

                if (isPlayingUpdate)
                {
                    IsPlaying = false;
                }

                PlaybackPausedEvent?.Invoke(this, null);
            }
        }

        public void Stop(bool isPlayingUpdate = true)
        {
            mediaPlayer?.Stop();
        }

        public void SkipToStart()
        {
            const double timeToGoToPreviousChapter = 2.5;

            if (SecondsPlayed < timeToGoToPreviousChapter)
            {
                PreviousChapter();

                if (IsPlaying)
                {
                    Play();
                }
            }
            else
            {
                SecondsPlayed = 0;
            }
        }

        public void SkipToEnd()
        {
            NextChapter();

            if (IsPlaying)
            {
                Play();
            }
        }

        public void Rewind(double amt)
        {
            SecondsPlayed = Math.Max(0, SecondsPlayed - amt);
        }

        public void Forward(double amt)
        {
            SecondsPlayed = Math.Min(SecondsPlayed + amt, MaxSeconds);
        }

        #endregion

    }
}
