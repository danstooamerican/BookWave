using ATL;
using GalaSoft.MvvmLight;

namespace Commons.Models
{
    /// <summary>
    /// Basis metadata for audio files.
    /// </summary>
    public class ChapterMetadata : Metadata
    {
        #region Public Properties

        private int mTrackNumber;
        public int TrackNumber
        {
            get { return mTrackNumber; }
            set { Set<int>(() => this.TrackNumber, ref mTrackNumber, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Metadata object off a track.
        /// </summary>
        /// <param name="track">Track to reference.</param>
        public ChapterMetadata(Track track = null)
        {
            Title = string.Empty;
            Description = string.Empty;
            TrackNumber = -1;
            ReleaseYear = -1;

            if (track != null)
            {
                Title = track.Title;
                TrackNumber = track.TrackNumber;
                Description = track.Description;
                ReleaseYear = track.Year;
            }
        }

        #endregion

    }
}
