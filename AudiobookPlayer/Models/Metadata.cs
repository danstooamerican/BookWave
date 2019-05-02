using ATL;
using GalaSoft.MvvmLight;

namespace Commons.Models
{
    public class Metadata : ObservableObject
    {
        #region Public Properties

        private string mTitle;
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
        public string Description
        {
            get { return mDescription; }
            set { Set<string>(() => this.Description, ref mDescription, value); }
        }

        private Contributors mContributors;
        public Contributors Contributors
        {
            get { return mContributors; }
            set { Set<Contributors>(() => this.Contributors, ref mContributors, value); }
        }

        private int mReleaseYear;
        public int ReleaseYear
        {
            get { return mReleaseYear; }
            set { Set<int>(() => this.ReleaseYear, ref mReleaseYear, value); }
        }

        #endregion

        #region Constructors

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
