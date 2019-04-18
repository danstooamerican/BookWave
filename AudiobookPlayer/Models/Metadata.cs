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

        private string mPath;
        public string Path
        {
            get { return mPath; }
            set { Set<string>(() => this.Path, ref mPath, value); }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { Set<string>(() => this.Name, ref mName, value); }
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

        private DateTime mRelease;

        public DateTime Release
        {
            get { return mRelease; }
            set { mRelease = value; }
        }




        #endregion
    }
}
