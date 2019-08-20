using BookWave.Desktop.AudiobookManagement.Scanner;
using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
{
    public class Library : ObservableObject, XMLSaveObject, IComparable<Library>
    {
        #region Properties

        public readonly int ID;

        private string mLibraryPath;
        public string LibraryPath
        {
            get { return mLibraryPath; }
            set { Set<string>(() => this.LibraryPath, ref mLibraryPath, value); }
        }

        private string mMetadataFolder;
        public string MetadataFolder
        {
            get { return mMetadataFolder; }
            set { mMetadataFolder = value; }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { Set<string>(() => this.Name, ref mName, value); }
        }

        private IDictionary<int, Audiobook> mAudiobooks;
        public IDictionary<int, Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<IDictionary<int, Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        private LibraryScanner mScanner;
        public LibraryScanner Scanner
        {
            get { return mScanner; }
            set { mScanner = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Library object with an id and a metadata folder path.
        /// The library is not initialized.
        /// </summary>
        /// <param name="id">Intern id for the library</param>
        /// <param name="metadataFolder">Folder where all metadata files for this library are located</param>
        public Library(int id, string metadataFolder)
        {
            this.ID = id;
            this.Name = string.Empty;
            this.LibraryPath = string.Empty;
            this.mMetadataFolder = metadataFolder;
            Directory.CreateDirectory(metadataFolder);
            this.Audiobooks = new ConcurrentDictionary<int, Audiobook>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether this library contains an audiobook with the same id.
        /// </summary>
        /// <param name="id">id which is searched for</param>
        /// <returns>true if an audiobook with the same id is already added to this library</returns>
        public bool Contains(int id)
        {
            return Audiobooks.ContainsKey(id);
        }

        /// <summary>
        /// Checks whether this library contains the given audiobook.
        /// </summary>
        /// <param name="audiobook">audiobook which is searched for</param>
        /// <returns>true if the audiobook is already in the library</returns>
        public bool Contains(Audiobook audiobook)
        {
            return Contains(audiobook.ID);
        }

        /// <summary>
        /// Returns a list of all audiobooks in this library.
        /// </summary>
        /// <returns>list of all audiobooks</returns>
        public ICollection<Audiobook> GetAudiobooks()
        {
            return Audiobooks.Values;
        }

        /// <summary>
        /// Adds an audiobook to the Library. If it is already added the audiobook is updated.
        /// Every audiobook that is new to the library gets a new metadata path.
        /// </summary>
        public void UpdateAudiobook(Audiobook audiobook)
        {
            if (Contains(audiobook))
            {
                ClearChapterMetadata(audiobook);
                Audiobooks.Remove(audiobook.ID);
            }
            else
            {
                audiobook.Metadata.MetadataPath = Path.Combine(MetadataFolder, Guid.NewGuid().ToString());
            }

            AddAudiobook(audiobook);
        }

        /// <summary>
        /// Adds an audiobook to this library if it is not already added.
        /// </summary>
        /// <param name="audiobook"></param>
        private void AddAudiobook(Audiobook audiobook)
        {
            if ( !(audiobook == null || Contains(audiobook)) && audiobook.Chapters.Count > 0)
            {
                if (audiobook.Library != null)
                {
                    audiobook.Library.RemoveAudiobook(audiobook);
                }
                audiobook.Library = this;

                Audiobooks.Add(audiobook.ID, audiobook);

                SaveMetadata(audiobook);
            }
        }

        /// <summary>
        /// Removes an audiobook from this library and removes the link to it from the audiobook.
        /// The link is only removed if it points to this library.
        /// </summary>
        /// <param name="audiobook">audiobook to be removed</param>
        public void RemoveAudiobook(Audiobook audiobook)
        {
            if (Contains(audiobook))
            {
                Audiobooks.Remove(audiobook.ID);

                if (audiobook.Library.Equals(this))
                {
                    audiobook.Library = null;
                }
                DeleteMetadataFolder(audiobook.Metadata.MetadataPath);
            }
        }

        private void ClearChapterMetadata(Audiobook audiobook)
        {
            string chapterMetadataPath = Path.Combine(audiobook.Metadata.MetadataPath, "chapters");
            try
            {
                Directory.Delete(chapterMetadataPath, true);
            }
            catch (IOException)
            {
                throw new DeleteMetadataException(chapterMetadataPath, "could not be cleared.");
            }
        }

        private void DeleteMetadataFolder(string metadataPath)
        {
            try
            {
                Directory.Delete(metadataPath, true);
            }
            catch (IOException)
            {
                throw new DeleteMetadataException(metadataPath, "could not be deleted.");
            }
        }

        /// <summary>
        /// Uses the scanner object to scan the library folder for new files. If new files are found corresponding 
        /// metadata files are created.
        /// </summary>
        /// <param name="hardScan">if set to true all files which are not found will be deleted permanently</param>
        public void ScanLibrary(bool hardScan = false)
        {
            // build index to access audiobooks by their paths
            Dictionary<string, Audiobook> audiobookIndex = new Dictionary<string, Audiobook>();
            foreach (Audiobook audiobook in GetAudiobooks())
            {
                audiobookIndex.Add(audiobook.Metadata.Path, audiobook);
            }

            // look what changed
            foreach (Audiobook audiobook in Scanner.ScanLibrary(LibraryPath))
            {
                if (audiobookIndex.ContainsKey(audiobook.Metadata.Path))
                {
                    Audiobook changedAudiobook = audiobookIndex[audiobook.Metadata.Path];

                    // build index to access chapters by their paths
                    Dictionary<string, Chapter> chapterIndex = new Dictionary<string, Chapter>();
                    foreach (Chapter chapter in changedAudiobook.Chapters)
                    {
                        chapterIndex.Add(chapter.AudioPath.Path, chapter);
                    }

                    foreach (Chapter chapter in audiobook.Chapters)
                    {
                        if (chapterIndex.ContainsKey(chapter.AudioPath.Path))
                        {
                            chapterIndex.Remove(chapter.AudioPath.Path);                            
                        } else
                        {
                            changedAudiobook.Chapters.Add(chapter);
                        }
                    }

                    if (hardScan)
                    {
                        // delete unused files
                        foreach (Chapter chapter in chapterIndex.Values)
                        {
                            changedAudiobook.Chapters.Remove(chapter);
                            File.Delete(Path.Combine(changedAudiobook.Metadata.MetadataPath, "chapters", chapter.Metadata.MetadataPath));
                        }
                    }

                    SaveMetadata(changedAudiobook);

                    audiobookIndex.Remove(audiobook.Metadata.Path);
                } else
                {
                    UpdateAudiobook(audiobook);
                }                
            }

            if (hardScan)
            {
                // delete unused audiobooks
                foreach (Audiobook audiobook in audiobookIndex.Values)
                {
                    RemoveAudiobook(audiobook);
                }
            }
        }

        /// <summary>
        /// Loads all metadata files currently created for this library.
        /// </summary>
        public void LoadMetadata(IProgress<UpdateReport> progress = null)
        {
            if (!Directory.Exists(MetadataFolder))
            {
                throw new FileNotFoundException("Library metadata folder not found at '" + MetadataFolder + "'.");
            }

            string[] audiobookFolders = Directory.GetDirectories(MetadataFolder);
            int threadCount = Math.Max(1, Math.Min(audiobookFolders.Length / 2, Environment.ProcessorCount));
            int batchSize = (int)Math.Ceiling((double)audiobookFolders.Length / threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                int start = batchSize * i;
                int end = Math.Min((i + 1) * batchSize, audiobookFolders.Length);

                Thread thread = new Thread(() => LoadMetadata(audiobookFolders, start, end, progress));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        /// <summary>
        /// Loads a section of the audiobook folders from the appdata/metadata/library folder.
        /// </summary>
        /// <param name="audiobookFolders">list of all audiobook folders</param>
        /// <param name="start">start index</param>
        /// <param name="end">end index</param>
        /// <param name="progress">progress object to give updates to the caller</param>
        private void LoadMetadata(string[] audiobookFolders, int start, int end, IProgress<UpdateReport> progress = null)
        {
            for (int i = start; i < end; i++)
            {
                string audiobookMetadataPath = Path.Combine(audiobookFolders[i], ConfigurationManager.AppSettings.Get("audiobook_metadata_filename"))
                    + "." + ConfigurationManager.AppSettings.Get("metadata_extensions");

                // ignore folders without audiobook metadata
                if (!File.Exists(audiobookMetadataPath))
                {
                    continue;
                }

                Audiobook audiobook = AudiobookManager.Instance.CreateAudiobook(audiobookMetadataPath);

                string chapterMetadataPath = Path.Combine(audiobookFolders[i], "chapters");
                string[] chapterFiles = Directory.GetFiles(chapterMetadataPath);
                Parallel.ForEach(chapterFiles, (chapterFile) =>
                {
                    Chapter chapter = AudiobookManager.Instance.CreateChapter(chapterFile);
                    audiobook.Chapters.Add(chapter);
                });

                AddAudiobook(audiobook);                
            }

            if (progress != null)
            {
                progress.Report(new UpdateReport());
            }
        }

        /// <summary>
        /// Saves the audiobook metadata in the library metadata folder.
        /// </summary>
        /// <param name="audiobook">Audiobook to be saved</param>
        public void SaveMetadata(Audiobook audiobook)
        {
            if (string.IsNullOrEmpty(audiobook.Metadata.MetadataPath))
            {
                throw new FileNotFoundException("No metadata path is set for this audiobook.");
            }

            XElement audiobookXML = audiobook.ToXML();

            string audiobookMetadataPath = Path.Combine(MetadataFolder, audiobook.Metadata.MetadataPath);
            Directory.CreateDirectory(audiobookMetadataPath);

            XMLHelper.SaveToXML(audiobook, Path.Combine(audiobookMetadataPath,
                ConfigurationManager.AppSettings.Get("audiobook_metadata_filename") + "."
                        + ConfigurationManager.AppSettings.Get("metadata_extensions")));

            string chapterMetadataPath = Path.Combine(audiobookMetadataPath, "chapters");
            Directory.CreateDirectory(chapterMetadataPath);

            foreach (Chapter chapter in audiobook.Chapters)
            {
                if (string.IsNullOrEmpty(chapter.Metadata.MetadataPath))
                {
                    chapter.Metadata.MetadataPath = Guid.NewGuid().ToString() + "." + ConfigurationManager.AppSettings.Get("metadata_extensions");
                }

                XMLHelper.SaveToXML(chapter, Path.Combine(chapterMetadataPath, chapter.Metadata.MetadataPath));
            }
        }

        public XElement ToXML()
        {
            var libraryXML = new XElement("Library");

            if (!Name.Equals(string.Empty))
            {
                libraryXML.Add(new XElement("Name", Name));
            }

            if (!LibraryPath.Equals(string.Empty))
            {
                libraryXML.Add(new XElement("Path", LibraryPath));
            }

            if (!LibraryScannerFactory.GetDefault().GetIdentifier().Equals(Scanner.GetIdentifier()))
            {
                libraryXML.Add(new XElement("LibraryScanner", Scanner.GetIdentifier()));
            }            

            return libraryXML;
        }

        public void FromXML(XElement xmlElement)
        {
            LibraryPath = XMLHelper.GetSingleValue(xmlElement, "Path");
            Name = XMLHelper.GetSingleValue(xmlElement, "Name");

            string scannerIdentifier = XMLHelper.GetSingleValue(xmlElement, "LibraryScanner");
            if (string.IsNullOrEmpty(scannerIdentifier))
            {
                Scanner = LibraryScannerFactory.GetDefault();
            } else
            {
                Scanner = LibraryScannerFactory.GetScanner(scannerIdentifier);
            }            
        }

        /// <summary>
        /// Compares two library objects by their names.
        /// </summary>
        /// <param name="other">another library</param>
        /// <returns>result of the string CompareTo method</returns>
        public int CompareTo(Library other)
        {
            if (other == null)
            {
                return 1;
            }

            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Returns the string representation of a library.
        /// </summary>
        /// <returns>the name of the library</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}
