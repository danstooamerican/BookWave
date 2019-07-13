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

        private Dictionary<int, Audiobook> mAudiobooks;
        public Dictionary<int, Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<Dictionary<int, Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
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
        /// <param name="MetadataFolder">Folder where all metadata files for this library are located</param>
        public Library(int id, string MetadataFolder)
        {
            this.ID = id;
            this.Name = string.Empty;
            this.LibraryPath = string.Empty;
            this.mMetadataFolder = MetadataFolder;
            this.Audiobooks = new Dictionary<int, Audiobook>();
        }

        #endregion

        #region Methods

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
