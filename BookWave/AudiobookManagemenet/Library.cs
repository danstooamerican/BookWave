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

        public bool Contains(int id)
        {
            return Audiobooks.ContainsKey(id);
        }

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
        /// The metadata file is also updated.
        /// </summary>
        public void UpdateAudiobook(Audiobook audiobook)
        {
            if (Contains(audiobook))
            {
                Audiobooks.Remove(audiobook.ID);
            }

            SaveMetadata(audiobook);
            Audiobooks.Add(audiobook.ID, audiobook);
        }

        public void RemoveAudiobook(Audiobook audiobook)
        {
            if (Contains(audiobook))
            {
                Audiobooks.Remove(audiobook.ID);
            }            
        }

        /// <summary>
        /// Uses the set scanner object to scan the library folder for new files. If new files are found corresponding 
        /// metadata files are created.
        /// </summary>
        public void ScanLibrary()
        {
            Scanner.ScanLibrary(this);   
        }

        /// <summary>
        /// Loads all metadata files currently created for this library.
        /// </summary>
        public void LoadMetadata()
        {

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

            return libraryXML;
        }

        public void FromXML(XElement xmlElement)
        {
            LibraryPath = XMLHelper.GetSingleElement(xmlElement, "Path");
            Name = XMLHelper.GetSingleElement(xmlElement, "Name");
        }

        #endregion

    }
}
