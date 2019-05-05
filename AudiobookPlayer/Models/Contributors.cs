using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    /// <summary>
    /// Includes lists of people who contribute to an audiobook.
    /// </summary>
    public class Contributors : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<string> mAuthors;
        /// <summary>
        /// List of authors.
        /// </summary>
        public ObservableCollection<string> Authors
        {
            get { return mAuthors; }
            set { Set<ObservableCollection<string>>(() => this.Authors, ref mAuthors, value); }
        }

        private ObservableCollection<string> mReaders;
        /// <summary>
        /// List of readers.
        /// </summary>
        public ObservableCollection<string>  Readers
        {
            get { return mReaders; }
            set { Set<ObservableCollection<string>>(() => this.Readers, ref mReaders, value); }
        }

        /// <summary>
        /// String representation of all authors. If this Property is set
        /// in the right format (name1, name2, ..., name n) then the current 
        /// author list is cleared and new elements are added.
        /// </summary>
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

        /// <summary>
        /// Creates a new Contributors class with empty lists of
        /// authors and readers.
        /// </summary>
        public Contributors()
        {
            Authors = new ObservableCollection<string>();
            Readers = new ObservableCollection<string>();
        }

        #endregion

    }
}
