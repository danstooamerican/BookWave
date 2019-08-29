﻿using BookWave.Desktop.Exceptions;
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
        /// Sets the path of the audiobook if it exists. All chapters in the audiobook
        /// get updated to be in the given directory without checking if they exist.
        /// </summary>
        /// <param name="path">audiobook directory</param>
        /// <exception cref="FileNotFoundException">is thrown if the path does not exists</exception>
        public void SetPath(string path)
        {
            if (Directory.Exists(path))
            {
                Metadata.Path = path;

                foreach (Chapter c in Chapters)
                {
                    string fileName = Path.GetFileName(c.AudioPath.Path);

                    c.AudioPath.Path = Path.Combine(path, fileName);
                }
            } else
            {
                throw new FileNotFoundException("the directory does not exist", path);
            }        
        }

        /// <summary>
        /// Sets the path of the chapter if it exists and points to a file within the 
        /// audiobook folder.
        /// </summary>
        /// <param name="chapter">chapter to be updated</param>
        /// <param name="path">path to the audio file of the chapter</param>
        /// <exception cref="FileNotFoundException">is thrown if the file does not exist</exception>
        /// <exception cref="InvalidArgumentException">if the file is not in the audiobook folder</exception>
        public void SetChapterPath(Chapter chapter, string path)
        {
            if (chapter == null) return;

            if (File.Exists(path))
            {
                if (Path.GetDirectoryName(path).Equals(Metadata.Path))
                {
                    chapter.AudioPath.Path = path;
                } else
                {
                    throw new InvalidArgumentException(path, "is not in the audiobook folder.");
                }
            } else
            {
                throw new FileNotFoundException("the file does not exist", path);
            }
        }

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