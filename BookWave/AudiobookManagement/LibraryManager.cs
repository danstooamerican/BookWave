using BookWave.Desktop.AudiobookManagement.Scanner;
using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
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

        private readonly object IDLock = new object();
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

        /// <summary>
        /// Checks whether a library with the given id is registered.
        /// </summary>
        /// <param name="libraryId">library id to search for</param>
        /// <returns>true if a library with this id is found</returns>
        public bool Contains(int libraryId)
        {
            return Libraries.ContainsKey(libraryId);
        }

        /// <summary>
        /// Returns a list with all current library objects.
        /// </summary>
        /// <returns>list of all loaded libraries</returns>
        public ICollection<Library> GetLibraries()
        {
            List<Library> libraries = new List<Library>(Libraries.Values);
            libraries.Sort();

            return libraries;
        }

        /// <summary>
        /// Returns the library with the given id. If no library with this id is found 
        /// a LibraryNotFoundException is thrown.
        /// </summary>
        /// <param name="id">id of the libaray</param>
        /// <returns>library object with the given id</returns>
        public Library GetLibrary(int id)
        {
            if (Contains(id))
            {
                return Libraries[id];
            }

            throw new LibraryNotFoundException(id, "not found");
        }

        /// <summary>
        /// Adds a library with the given parameters to the collection and saves it to the metadata folder.
        /// </summary>
        /// <param name="name">Name of the library</param>
        /// <param name="destination">path to the library folder</param>
        /// <param name="scanner">the scanner object used to scan the library folder</param>
        public Library AddLibrary(string name, string destination, LibraryScanner scanner)
        {
            Library library = new Library(GetNewID(), Path.Combine(MetadataPath, Guid.NewGuid().ToString()))
            {
                Name = name,
                LibraryPath = destination,
                Scanner = scanner
            };

            Libraries.Add(library.ID, library);

            XMLHelper.SaveToXML(library, Path.Combine(library.MetadataFolder, 
                ConfigurationManager.AppSettings.Get("library_metadata_filename")
                + "." + ConfigurationManager.AppSettings.Get("metadata_extensions")));

            library.ScanLibrary();

            return library;
        }

        /// <summary>
        /// Reads all libraries in the BookWave metadata folder and creates corresponding Library objects.
        /// Library folders must contain a library nfo file.
        /// 
        /// When performing this method all currently created Library objects are discarded and new
        /// ones are created.
        /// 
        /// This method uses multithreading for its computation.
        /// </summary>
        public void LoadLibraries(IProgress<UpdateReport> progress = null)
        {
            Libraries.Clear();            

            string[] libraryFolders = Directory.GetDirectories(MetadataPath);
            int threadCount = Math.Max(1, Math.Min(libraryFolders.Length / 25, Environment.ProcessorCount));
            int batchSize = (int)Math.Ceiling((double)libraryFolders.Length / threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                int start = batchSize * i;
                int end = Math.Min((i + 1) * batchSize, libraryFolders.Length);

                Thread thread = new Thread(() => LoadLibraries(libraryFolders, start, end, progress));
                thread.Start();
            }
        }

        /// <summary>
        /// Loads a section of the library folders from the appdata/metadata folder.
        /// </summary>
        /// <param name="libraryFolders">list of paths to library folders</param>
        /// <param name="start">start index</param>
        /// <param name="end">end index</param>
        /// <param name="progress">progress object to give updates to the caller</param>
        private void LoadLibraries(string[] libraryFolders, int start, int end, IProgress<UpdateReport> progress = null)
        {
            for (int i = start; i < end; i++)
            {
                string libraryNfo = Path.Combine(libraryFolders[i], ConfigurationManager.AppSettings.Get("library_metadata_filename") +
                    "." + ConfigurationManager.AppSettings.Get("metadata_extensions"));

                if (File.Exists(libraryNfo))
                {
                    Library library = new Library(GetNewID(), libraryFolders[i]);

                    XDocument xDocument = XDocument.Load(libraryNfo);
                    library.FromXML(xDocument.Root);

                    library.LoadMetadata(progress);

                    lock (Libraries)
                    {
                        Libraries.Add(library.ID, library);
                    }                                        
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
            int temp;

            lock (IDLock)
            {
                temp = IDCount;
                IDCount++;                
            }

            return temp;
        }

        #endregion

    }
}
