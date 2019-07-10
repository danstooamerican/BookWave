using Commons.Logic;
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

namespace Commons.Models
{
    public class Library : ObservableObject, XMLSaveObject
    {
        #region Properties

        private int mId;
        public int Id
        {
            get { return mId; }
            set { Set<int>(() => this.Id, ref mId, value); }
        }

        private string mLibraryPath;
        public string LibraryPath
        {
            get { return mLibraryPath; }
            set { Set<string>(() => this.LibraryPath, ref mLibraryPath, value); }
        }

        private string mMetadataFolderName;
        public string MetadataFolder
        {
            get { return mMetadataFolderName; }
            set { mMetadataFolderName = value; }
        }


        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { Set<string>(() => this.Title, ref mTitle, value); }
        }

        private Dictionary<int, Audiobook> mAudiobooks;
        public Dictionary<int, Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<Dictionary<int, Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        #endregion

        #region Constructor

        public Library(int id)
        {
            this.Id = id;
            this.Title = string.Empty;
            this.LibraryPath = string.Empty;
            this.mMetadataFolderName = string.Empty;
            this.Audiobooks = new Dictionary<int, Audiobook>();
        }

        #endregion

        #region Methods

        public void ScanLibrary()
        {

        }

        public void LoadMetadata()
        {

        }

        /// <summary>
        /// Saves the audiobook metadata.
        /// </summary>
        /// <param name="audiobook">Audiobook to be saved</param>
        public void SaveMetadata(Audiobook audiobook)
        {
            XElement audiobookXML = audiobook.ToXML();

            string libraryMetadataFolderPath = Path.Combine(LibraryManager.Instance.MetadataPath, MetadataFolder, audiobook.Metadata.MetadataPath);

            Directory.CreateDirectory(libraryMetadataFolderPath);

            foreach (Chapter chapter in audiobook.Chapters)
            {
                string fileName = Path.GetFileNameWithoutExtension(chapter.AudioPath.Path);

                XMLHelper.SaveToXML(chapter, Path.Combine(libraryMetadataFolderPath, fileName + "."
                    + ConfigurationManager.AppSettings.Get("metadata_extensions")));
            }

            XMLHelper.SaveToXML(audiobook, Path.Combine(libraryMetadataFolderPath,
                ConfigurationManager.AppSettings.Get("audiobook_metadata_filename") + "."
                        + ConfigurationManager.AppSettings.Get("metadata_extensions")));
        }

        public XElement ToXML()
        {
            var libraryXML = new XElement("Library");

            if (!Title.Equals(string.Empty))
            {
                libraryXML.Add(new XElement("Title", Title));
            }

            if (!LibraryPath.Equals(string.Empty))
            {
                libraryXML.Add(new XElement("LibraryPath", LibraryPath));
            }

            return libraryXML;
        }

        public void FromXML(XElement xmlElement)
        {
            LibraryPath = XMLHelper.GetSingleElement(xmlElement, "LibraryPath");
            Title = XMLHelper.GetSingleElement(xmlElement, "Title");
        }

        #endregion

    }
}
