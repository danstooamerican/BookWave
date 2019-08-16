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

        private object IDLock = new object();
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
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Libraries.Clear();            

            string[] libraryFolders = Directory.GetDirectories(MetadataPath);
            int threadCount = Math.Max(1, Math.Min(libraryFolders.Length / 25, Environment.ProcessorCount));
            int batchSize = (int)Math.Ceiling((double)libraryFolders.Length / threadCount);
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int start = batchSize * i;
                int end = Math.Min((i + 1) * batchSize, libraryFolders.Length);

                Thread thread = new Thread(() => LoadLibraries(libraryFolders, start, end, progress));
                threads[i] = thread;
                thread.Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
        }

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
            lock (IDLock)
            {
                var temp = IDCount;
                IDCount++;
                return temp;
            }            
        }

        #endregion

    }
}
