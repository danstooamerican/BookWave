using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class ContributorsModel
    {

        #region Public Properties

        private string[] mAuthors;

        public string[] Authors
        {
            get { return mAuthors; }
            set { mAuthors = value; }
        }

        private string[] mReaders;

        public string[] Readers
        {
            get { return mReaders; }
            set { mReaders = value; }
        }

        #endregion

    }
}
