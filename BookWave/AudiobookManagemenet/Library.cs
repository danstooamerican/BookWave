using Commons.AudiobookManagemenet.Scanner;
using Commons.Logic;
using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Commons.AudiobookManagemenet
{
    public class Library : ObservableObject, XMLSaveObject
    {
        #region Properties

        private readonly string DEFAULT_SCANNER = typeof(AudiobooksTopScanner).FullName;

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
            this.Audiobooks = new Dictionary<int, Audiobook>();
        }

        /// <summary>
        /// Creates a new Library object with an id, name, metadata folder path and library folder path.
        /// </summary>
        /// <param name="id">Intern id for the library</param>
        /// <param name="name">Display name of the library</param>
        /// <param name="metadataFolder">Folder where all metadata files for this library are located</param>
        /// <param name="libraryFolder">Folder where all audio files for this library are located.
        ///                             This folder is manages by the user.</param>
        public Library(int id, string name, string metadataFolder, string libraryFolder)
        {
            this.ID = id;
            this.Name = name;
            this.LibraryPath = libraryFolder;
            this.mMetadataFolder = metadataFolder;
            this.Audiobooks = new Dictionary<int, Audiobook>();
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
        /// Checks whether this library contains an audiobook with the same id.
        /// </summary>
        /// <param name="audiobook">audiobook which is searched for</param>
        /// <returns>true if an audiobook with the same id is already added to this library</returns>
        public bool Contains(Audiobook audiobook)
        {
            return Contains(audiobook.ID);
        }

        /// <summary>
        /// Searches for an audio book with the given path.
        /// </summary>
        /// <param name="path">is the path of the audiobook</param>
        /// <returns>audiobook</returns>
        public Audiobook GetAudiobook(int id)
        {
            if (Contains(id))
            {
                return Audiobooks[id];
            }
            return null;
        }

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
                RemoveAudiobook(audiobook);
            } else
            {
                audiobook.Metadata.MetadataPath = Path.Combine(MetadataFolder, Guid.NewGuid().ToString());
            }

            AddAudiobook(audiobook);
        }

        /// <summary>
        /// Adds an audiobook to this library if it is not already added. A new 
        /// </summary>
        /// <param name="audiobook"></param>
        private void AddAudiobook(Audiobook audiobook)
        {
            if (!Contains(audiobook) && audiobook != null)
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

                Directory.Delete(audiobook.Metadata.MetadataPath);
            }
        }

        /// <summary>
        /// Uses the set scanner object to scan the library folder for new files. If new files are found corresponding 
        /// metadata files are created.
        /// </summary>
        public void ScanLibrary()
        {
            Audiobooks.Clear();
            foreach (string audiobookFolder in Directory.GetFiles(MetadataFolder))
            {
                Directory.Delete(audiobookFolder);
            }
            
            foreach (Audiobook audiobook in Scanner.ScanLibrary(LibraryPath))
            {
                AddAudiobook(audiobook);
            }
        }

        /// <summary>
        /// Loads all metadata files currently created for this library.
        /// </summary>
        public void LoadMetadata()
        {
            if (!Directory.Exists(MetadataFolder))
            {
                throw new FileNotFoundException("Library metadata folder not found at '" + MetadataFolder + "'.");
            }

            foreach (string audiobookFolder in Directory.GetDirectories(MetadataFolder))
            {
                string audiobookMetadata = Path.Combine(audiobookFolder, ConfigurationManager.AppSettings.Get("audiobook_metadata_filename")) 
                    + "." + ConfigurationManager.AppSettings.Get("metadata_extensions");

                // ignore folders without audiobook metadata
                if (!File.Exists(audiobookMetadata))
                {
                    continue;
                }

                Audiobook audiobook = AudiobookManager.Instance.CreateAudiobook(audiobookMetadata);
                AddAudiobook(audiobook);
            }
        }

        /*
        /// <summary>
        /// Parses an audio book from a xml file. No chapters are loaded.
        /// </summary>
        /// <param name="path">path to the audio book folder</param>
        /// <returns>audio book created from the folder</returns>
        public Audiobook LoadAudiobookFromFile(string path)
        {
            // parse audiobook from xml file
            string audiobookMetadataPath = Path.Combine(path, ConfigurationManager.AppSettings.Get("metadata_folder"),
                ConfigurationManager.AppSettings.Get("audiobook_metadata_filename") + "."
                + ConfigurationManager.AppSettings.Get("metadata_extensions"));

            Audiobook audiobook = XMLHelper.XMLToAudiobook(audiobookMetadataPath);

            if (audiobook.Metadata.Path.Equals(string.Empty))
            {
                audiobook.Metadata.Path = path;
            }

            return audiobook;
        }*/

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

            foreach (Chapter chapter in audiobook.Chapters)
            {
                string fileName = Path.GetFileNameWithoutExtension(chapter.AudioPath.Path);

                XMLHelper.SaveToXML(chapter, Path.Combine(audiobookMetadataPath, fileName + "."
                    + ConfigurationManager.AppSettings.Get("metadata_extensions")));
            }

            XMLHelper.SaveToXML(audiobook, Path.Combine(audiobookMetadataPath,
                ConfigurationManager.AppSettings.Get("audiobook_metadata_filename") + "."
                        + ConfigurationManager.AppSettings.Get("metadata_extensions")));
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

            if (!Scanner.GetType().FullName.Equals(DEFAULT_SCANNER))
            {
                libraryXML.Add(new XElement("LibraryScanner", Scanner.GetType().FullName));
            }

            return libraryXML;
        }

        public void FromXML(XElement xmlElement)
        {
            LibraryPath = XMLHelper.GetSingleValue(xmlElement, "Path");
            Name = XMLHelper.GetSingleValue(xmlElement, "Name");
            Scanner = LibraryScanner.GetInstance(XMLHelper.GetSingleValue(xmlElement, "LibraryScanner", DEFAULT_SCANNER));
        }

        #endregion

    }
}
