using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;

namespace BookWave.Desktop.Models.AudiobookManagement
{
    /// <summary>
    /// Represents a single audiobook with a list of chapters and metadata.
    /// </summary>
    public class Audiobook : ObservableObject, XMLSaveObject, ICloneable
    {

        #region Public Properties

        private readonly int mID;
        /// <summary>
        /// Internal unique id of the audiobook.
        /// </summary>
        public int ID
        {
            get { return mID; }
        }

        private Library mLibrary;
        /// <summary>
        /// Library the audiobook belongs to. Null if no library is set.
        /// </summary>
        public Library Library
        {
            get { return mLibrary; }
            set { mLibrary = value; }
        }

        private ICollection<Chapter> mChapters;
        /// <summary>
        /// A list of all chapters in the audiobook.
        /// </summary>
        public ICollection<Chapter> Chapters
        {
            get { return mChapters; }
            private set { Set<ICollection<Chapter>>(() => this.Chapters, ref mChapters, value); }
        }

        /// <summary>
        /// The relevant metadata for the audiobook.
        /// </summary>
        private AudiobookMetadata mMetadata;

        public AudiobookMetadata Metadata
        {
            get { return mMetadata; }
            protected set { Set<AudiobookMetadata>(() => this.Metadata, ref mMetadata, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty audiobook with an id.
        /// </summary>
        internal Audiobook(int id, Library library = null)
        {
            mID = id;
            Chapters = new SynchronizedCollection<Chapter>();
            Metadata = new AudiobookMetadata();
            Library = library;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replaces all chapters of the audiobook with the given ones. If the parameter is null
        /// all current chapters are cleared.
        /// </summary>
        /// <param name="chapters">new chapters</param>
        public void SetChapters(ICollection<Chapter> chapters)
        {
            if (chapters != null)
            {
                Chapters = new ObservableCollection<Chapter>(chapters);
            }
            else
            {
                Chapters.Clear();
            }
        }

        public void UpdateProperties()
        {
            Metadata.UpdateProperties();
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