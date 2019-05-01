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

        #region Private Properties

        private const char DELIMITER = ',';
        private readonly string DELIM_TEXT = DELIMITER + " ";

        #endregion

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

        public Contributors()
        {
            Authors = new ObservableCollection<string>();
            Readers = new ObservableCollection<string>();
        }

        #endregion

        #region Helper Methods

        private ObservableCollection<string> ParseNameList(string value)
        {
            string[] split = value.Split(DELIMITER);

            ObservableCollection<string> list = new ObservableCollection<string>();
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

        private string BuildNameListString(ObservableCollection<string> list)
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
