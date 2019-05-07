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

        #region Private Properties

        private const char DELIMITER = ',';
        private readonly string DELIM_TEXT = DELIMITER + " ";

        #endregion

        #region Public Properties

        private List<string> mAuthors;
        /// <summary>
        /// List of authors.
        /// </summary>
        public List<string> Authors
        {
            get { return mAuthors; }
            set { Set<List<string>>(() => this.Authors, ref mAuthors, value); }
        }

        private List<string> mReaders;
        /// <summary>
        /// List of readers.
        /// </summary>
        public List<string>  Readers
        {
            get { return mReaders; }
            set { Set<List<string>>(() => this.Readers, ref mReaders, value); }
        }

        /// <summary>
        /// String representation of all authors. If this Property is set
        /// in the right format (name1, name2, ..., name n) then the current 
        /// author list is cleared and new elements are added.
        /// </summary>
        public string AuthorString {
            get
            {
                return BuildNameListString(Authors);
            }
            set
            {
                Authors = ParseNameList(value);
            }
        }

        public string ReaderString
        {
            get
            {
                return BuildNameListString(Readers);
            }
            set
            {
                Readers = ParseNameList(value);
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
            Authors = new List<string>();
            Readers = new List<string>();
        }

        #endregion

        #region Helper Methods

        private List<string> ParseNameList(string value)
        {
            string[] split = value.Split(DELIMITER);

            List<string> list = new List<string>();
            foreach (string name in split)
            {
                if (!name.Trim().Equals(string.Empty)
                    && !list.Contains(name.Trim()))
                {
                    list.Add(name.Trim());
                }
            }

            return list;
        }

        private string BuildNameListString(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item).Append(DELIM_TEXT);
            }

            sb.Length = Math.Max(sb.Length - DELIM_TEXT.Length, 0);

            return sb.ToString();
        }

        #endregion

    }
}
