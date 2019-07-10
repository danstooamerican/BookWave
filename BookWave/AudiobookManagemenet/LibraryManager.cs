using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace Commons.Logic
{
    public class LibraryManager : ObservableObject
    {
        #region Public Properties

        public readonly string MetadataPath 
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BookWave", "metadata");

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
        /// Creates a new instance of LibraryManager.
        /// Reads all libraries in the %AppData%/BookWave/metadata section and 
        /// </summary>
        private LibraryManager()
        {
            IDCount = 0;
            Libraries = new Dictionary<int, Library>();

            foreach (string directory in Directory.GetDirectories(MetadataPath))
            {
                string libraryNfo = Path.Combine(directory, "library." + ConfigurationManager.AppSettings.Get("metadata_extensions"));
                if (File.Exists(libraryNfo))
                {
                    Library library = new Library(GetNewID());
                    XDocument xDocument = XDocument.Load(libraryNfo);
                    library.FromXML(xDocument.Root);
                    Libraries.Add(library.Id, library);
                }
            }
        }

        #endregion

        #region Methods

        public void LoadLibraries()
        {
            foreach (Library library in Libraries.Values)
            {
                library.LoadMetadata();
            }
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
