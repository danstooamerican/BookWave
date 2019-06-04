using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public abstract class Metadata : ObservableObject
    {

        #region Public Properties

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { Set<string>(() => this.Title, ref mTitle, value); }
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

        #region Constructor

        public Metadata()
        {
            Title = string.Empty;
            Description = string.Empty;
            ReleaseYear = 0;
        }

        #endregion

    }
}
