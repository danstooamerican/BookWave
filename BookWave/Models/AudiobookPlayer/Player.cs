using BookWave.Desktop.Models.AudiobookManagement;
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



        private int mSecondsPlayed;
        public int SecondsPlayed
        {
            get { return mSecondsPlayed; }
            private set { mSecondsPlayed = value; }
        }

        private MediaPlayer mediaPlayer;

        #endregion

        #region Constructors

        public Player()
        {
            mediaPlayer = new MediaPlayer();
            IsPlaying = false;
        }

        #endregion

        #region Methods

        public void SelectAudiobook(Audiobook audiobook)
        {
            Audiobook = audiobook;
            mediaPlayer.Open(new Uri(audiobook.Chapters.ElementAt(0).AudioPath.Path, UriKind.RelativeOrAbsolute));
            SecondsPlayed = 0;
        }

        public void TogglePlay()
        {
            if (IsPlaying)
            {
                mediaPlayer.Pause();
                IsPlaying = false;
            }
            else
            {
                mediaPlayer.Play();
                IsPlaying = true;
            }
        }

        #endregion

    }
}
