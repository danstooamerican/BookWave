using ATL;
using GalaSoft.MvvmLight;

namespace Commons.Models
{
    /// <summary>
    /// Basis metadata for audio files.
    /// </summary>
    public class Metadata : ObservableObject
    {
        #region Public Properties

        private string mTitle;
        /// <summary>
        /// Title of the file.
        /// </summary>
        public string Title
        {
            get { return mTitle; }
            set { Set<string>(() => this.Title, ref mTitle, value); }
        }

        private int mTrackNumber;
        public int TrackNumber
        {
            get { return mTrackNumber; }
            set { Set<int>(() => this.TrackNumber, ref mTrackNumber, value); }
        }

        private string mDescription;
        /// <summary>
        /// Description of the file.
        /// </summary>
        public string Description
        {
            get { return mDescription; }
            set { Set<string>(() => this.Description, ref mDescription, value); }
        }

        private Contributors mContributors;
        /// <summary>
        /// All contributors.
        /// </summary>
        public Contributors Contributors
        {
            get { return mContributors; }
            set { Set<Contributors>(() => this.Contributors, ref mContributors, value); }
        }

        private int mReleaseYear;
        /// <summary>
        /// Release Year.
        /// </summary>
        public int ReleaseYear
        {
            get { return mReleaseYear; }
            set { Set<int>(() => this.ReleaseYear, ref mReleaseYear, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Metadata object off a track.
        /// </summary>
        /// <param name="track">Track to reference.</param>
        public Metadata(Track track = null)
        {
            Contributors = new Contributors();

            if (track != null)
            {
                Title = track.Title;
                TrackNumber = track.TrackNumber;
                Description = track.Description;
                ReleaseYear = track.Year;

                Contributors.AuthorString = track.Artist;
            }
        }

        #endregion

    }
}
