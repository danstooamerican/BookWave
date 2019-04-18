using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class Contributors : ObservableObject
    {

        #region Public Properties

        private string[] mAuthors;

        public string[] Authors
        {
            get { return mAuthors; }
            set { Set<string[]>(() => this.Authors, ref mAuthors, value); }
        }

        private string[] mReaders;

        public string[] Readers
        {
            get { return mReaders; }
            set { Set<string[]>(() => this.Readers, ref mReaders, value); }
        }

        #endregion

    }
}
