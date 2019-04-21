using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class Contributors : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<string> mAuthors;
        public ObservableCollection<string> Authors
        {
            get { return mAuthors; }
            set { Set<ObservableCollection<string>>(() => this.Authors, ref mAuthors, value); }
        }

        private ObservableCollection<string> mReaders;
        public ObservableCollection<string>  Readers
        {
            get { return mReaders; }
            set { Set<ObservableCollection<string>>(() => this.Readers, ref mReaders, value); }
        }

        public string AuthorString {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Authors)
                {
                    sb.Append(item).Append(", ");
                }

                sb.Length = Math.Max(sb.Length - 2, 0);

                return sb.ToString();
            }
        }

        #endregion

        #region Constructors

        public Contributors()
        {
            Authors = new ObservableCollection<string>();
            Readers = new ObservableCollection<string>();
        }

        #endregion

    }
}
