using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Commons.Logic
{
    public class LibraryManager : ObservableObject
    {
        #region Public Properties

        private static LibraryManager mInstance;
        public static LibraryManager Instance
        {
            get {
                if (mInstance == null)
                {
                    Instance = new LibraryManager();
                }
                return mInstance;
            }
            private set { mInstance = value; }
        }

        private int IDCount;

        private Dictionary<int, Library> mLibraries;
        public Dictionary<int, Library> Libraries
        {
            get { return mLibraries; }
            set { Set<Dictionary<int, Library>>(() => this.Libraries, ref mLibraries, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AudiobookManager.
        /// Reads the Path where the files are stored and 
        /// then reads the audiobooks.
        /// </summary>
        private LibraryManager()
        {
            Libraries = new Dictionary<int, Library>();
            // todo load libraries from appdata
            


            IDCount = 0;
        }

        #endregion

        #region Methods

        public void LoadLibraries()
        {

        }

        public void ScanLibraries()
        {

        }

        public int GetNewID()
        {
            var temp = IDCount;
            IDCount++;
            return temp;
        }

        #endregion

    }
}
