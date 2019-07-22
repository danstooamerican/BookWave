using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Commons.AudiobookManagement
{
    /// <summary>
    /// Represents a single audiobook with a list of chapters and metadata.
    /// </summary>
    public class Audiobook : ObservableObject, XMLSaveObject, ICloneable
    {

        #region Public Properties

        private readonly int mID;

        public int ID
        {
            get { return mID; }
        }

        private Library mLibrary;
        public Library Library
        {
            get { return mLibrary; }
            set { mLibrary = value; }
        }

        private ObservableCollection<Chapter> mChapters;
        /// <summary>
        /// A list of all chapters in the audiobook.
        /// </summary>
        public ObservableCollection<Chapter> Chapters
        {
            get { return mChapters; }
            set { Set<ObservableCollection<Chapter>>(() => this.Chapters, ref mChapters, value); }
        }

        /// <summary>
        /// The relevant metadata for the audiobook.
        /// </summary>
        private AudiobookMetadata mMetadata;

        public AudiobookMetadata Metadata
        {
            get { return mMetadata; }
            set { Set<AudiobookMetadata>(() => this.Metadata, ref mMetadata, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty audiobook with an id.
        /// </summary>
        internal Audiobook(int id, Library library = null)
        {
            mID = id;
            Chapters = new ObservableCollection<Chapter>();
            Metadata = new AudiobookMetadata();
            Library = library;
        }

        #endregion

        #region Methods

        public void SetChapters(ICollection<Chapter> chapters)
        {
            Chapters = new ObservableCollection<Chapter>(chapters);
        }

        public XElement ToXML()
        {
            var audiobookXML = new XElement("Audiobook");

            audiobookXML.Add(Metadata.ToXML());

            return audiobookXML;
        }

        public void FromXML(XElement xmlElement)
        {
            Metadata.FromXML(xmlElement);
        }

        public object Clone()
        {
            Audiobook copy = new Audiobook(ID, Library);

            copy.Metadata = (AudiobookMetadata)Metadata.Clone();

            foreach (Chapter chapter in Chapters)
            {
                copy.Chapters.Add((Chapter)chapter.Clone());
            }

            return copy;
        }

        #endregion
    }
}