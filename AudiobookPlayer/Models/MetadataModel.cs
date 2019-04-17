using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public abstract class MetadataModel
    {
        #region Public Properties

        private string mPath;
        public string Path
        {
            get { return mPath; }
            set { mPath = value; }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        private string mDescription;
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        private ContributorsModel mContributors;

        public ContributorsModel Contributors
        {
            get { return mContributors; }
            set { mContributors = value; }
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
