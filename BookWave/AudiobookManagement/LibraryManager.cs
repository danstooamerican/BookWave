using BookWave.Desktop.AudiobookManagement.Scanner;
using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
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

        private IDictionary<int, Library> mLibraries;
        public IDictionary<int, Library> Libraries
        {
            get { return mLibraries; }
            private set { Set<IDictionary<int, Library>>(() => this.Libraries, ref mLibraries, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LibraryManager.
        /// </summary>
        private LibraryManager()
        {
            IDCount = 0;
            Libraries = new Dictionary<int, Library>();
        }

        #endregion

        #region Methods

        public bool Contains(int libraryId)
        {
            return Libraries.ContainsKey(libraryId);
        }

        public bool Contains(Library library)
        {
            return Contains(library.ID);
        }

        public ICollection<Library> GetLibraries()
        {
            List<Library> libraries = new List<Library>(Libraries.Values);
            libraries.Sort();

            return libraries;
        }

        public Library GetLibrary(int id)
        {
            if (Contains(id))
            {
                return Libraries[id];
            }

            throw new LibraryNotFoundException(id, "not found");
        }

        public void AddLibrary(string name, string destination, LibraryScanner scanner)
        {
            Library library = new Library(GetNewID(), Path.Combine(MetadataPath, Guid.NewGuid().ToString()));
            library.Name = name;
            library.LibraryPath = destination;
            library.Scanner = scanner;

            Libraries.Add(library.ID, library);

            XMLHelper.SaveToXML(library, Path.Combine(library.MetadataFolder, 
                ConfigurationManager.AppSettings.Get("library_metadata_filename")
                + "." + ConfigurationManager.AppSettings.Get("metadata_extensions")));

            library.ScanLibrary();
        }

        /// <summary>
        /// Reads all libraries in the BookWave metadata folder and creates corresponding Library objects.
        /// Library folders must contain a library nfo file.
        /// 
        /// When performing this method all currently created Library objects are discarded and new
        /// ones are created.
        /// </summary>
        public void LoadLibraries(IProgress<UpdateReport> progress = null)
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

                    library.LoadMetadata(progress);                    
                }

                if (progress != null)
                {
                    progress.Report(new UpdateReport());
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
