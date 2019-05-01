using ATL;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class Metadata : ObservableObject
    {
        #region Public Properties

        private string mPath;
        public string Path
        {
            get { return mPath; }
            set { Set<string>(() => this.Path, ref mPath, value); }
        }

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { Set<string>(() => this.Title, ref mTitle, value); }
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
                Path = track.Path;
                Title = track.Title;
                Description = track.Description;
                ReleaseYear = track.Year;

                Contributors.Authors.Add(track.Artist);
            }
        }

        #endregion

    }
}
