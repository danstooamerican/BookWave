using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class Library
    {
        #region Properties

        private int mId;
        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }

        private string mPath;
        public string Path
        {
            get { return mPath; }
            set { mPath = value; }
        }

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }

        #endregion

        #region Constructor

        public Library(int id, string title, string path)
        {
            this.Id = id;
            this.Title = title;
            this.Path = path;
        }

        #endregion

        #region Methods

        public void ScanLibrary()
        {

        }

        public void LoadMetadata()
        {

        }

        public void SaveMetadata(Audiobook audiobook)
        {

        }

        #endregion

    }
}
