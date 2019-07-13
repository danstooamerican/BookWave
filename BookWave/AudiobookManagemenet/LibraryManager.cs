using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace Commons.AudiobookManagemenet
{
    public class LibraryManager : ObservableObject
    {
        #region Public Properties

        private static LibraryManager mInstance;
        public static LibraryManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Instance = new LibraryManager();
                }
                return mInstance;
            }
            private set { mInstance = value; }
        }

        /// <summary>
        /// Path to the BookWave metadata folder.
        /// </summary>
        public readonly string MetadataPath 
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BookWave", "metadata");

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
        /// Creates a new instance of LibraryManager and loads all libraries 
        /// from the metadata folder.
        /// </summary>
        private LibraryManager()
        {
            IDCount = 0; 
            Libraries = new Dictionary<int, Library>();

            LoadLibraries();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads all libraries in the BookWave metadata folder and creates corresponding Library objects.
        /// Library folders must contain a library nfo file.
        /// 
        /// When performing this method all currently created Library objects are discarded and new
        /// ones are created.
        /// </summary>
        private void LoadLibraries()
        {
            Libraries.Clear();

            foreach (string directory in Directory.GetDirectories(MetadataPath))
            {
                string libraryNfo = Path.Combine(directory, ConfigurationManager.AppSettings.Get("library_metadata_filename") +
                    "." + ConfigurationManager.AppSettings.Get("metadata_extensions"));

                if (File.Exists(libraryNfo))
                {
                    Library library = new Library(GetNewID(), directory);

                    XDocument xDocument = XDocument.Load(libraryNfo);
                    library.FromXML(xDocument.Root);

                    Libraries.Add(library.ID, library);
                }
            }
        }

        /// <summary>
        /// Performs the ScanLibrary method on all libraries currently loaded.
        /// </summary>
        public void ScanLibraries()
        {
            foreach (Library library in Libraries.Values)
            {
                library.ScanLibrary();
            }
        }

        /// <summary>
        /// Creates a new Library id. This method uses an auto increment method.
        /// </summary>
        /// <returns>unique runtime id for a library</returns>
        private int GetNewID()
        {
            var temp = IDCount;
            IDCount++;
            return temp;
        }

        #endregion

    }
}
