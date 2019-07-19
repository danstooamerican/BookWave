﻿using Commons.Util;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Commons.AudiobookManagement
{
    /// <summary>
    /// Metadata for audiobooks which adds a Genre, Contributors, ReleaseYear and a CoverPath.
    /// </summary>
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

        public static readonly string StandardCover = @"/Commons.Styles;component/Resources/Player/sampleCover.png";
            //System.IO.Path.Combine(Environment.CurrentDirectory, @"/Commons.Styles;component/Resources/Player", "sampleCover.png");

        private string mPath;
        /// <summary>
        /// Path to the audiobook folder.
        /// </summary>
        public string Path
        {
            get { return mPath; }
            set
            {
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.Path, ref mPath, value.Trim());
                }
                else
                {
                    if (value != null && value.Equals(string.Empty))
                    {
                        Set<string>(() => this.Path, ref mPath, value);
                    }
                }
            }
        }

        private string mGenre;
        /// <summary>
        /// Genre of the audiobook.
        /// </summary>
        public string Genre
        {
            get { return mGenre; }
            set { mGenre = value; }
        }

        private Contributors mContributors;
        /// <summary>
        /// All contributors.
        /// </summary>
        public Contributors Contributors
        {
            get { return mContributors; }
            set { Set<Contributors>(() => this.Contributors, ref mContributors, value); }
        }

        private int mReleaseYear;
        /// <summary>
        /// Release Year.
        /// </summary>
        public int ReleaseYear
        {
            get { return mReleaseYear; }
            set { Set<int>(() => this.ReleaseYear, ref mReleaseYear, value); }
        }

        /// <summary>
        /// The path for the cover image.
        /// </summary>
        public string CoverPath
        {
            get {
                string coverPath = System.IO.Path.Combine(MetadataPath, "cover.jpg");

                return File.Exists(coverPath) ? coverPath : StandardCover;
            }
        }

        public bool HasCoverPath {
            get
            {
                string coverPath = System.IO.Path.Combine(MetadataPath, "cover.jpg");

                return File.Exists(coverPath);
            }
        }

        #endregion

        #region Constructor
        public AudiobookMetadata() : base()
        {
            Path = string.Empty;
            MetadataPath = string.Empty;
            Genre = string.Empty;
            Contributors = new Contributors();
            ReleaseYear = 0;
        }

        #endregion

        #region Methods

        public new XElement ToXML()
        {
            XElement metadataXML = base.ToXML();

            if (!Path.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Path", Path));
            }

            if (!Genre.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Genre", Genre));
            }

            if (Contributors != null)
            {
                XElement contributorsXML = Contributors.ToXML();
                if (contributorsXML != null)
                {
                    metadataXML.Add(contributorsXML);
                }
            }

            if (ReleaseYear != 0) //TODO what is standard value?
            {
                metadataXML.Add(new XElement("ReleaseYear", ReleaseYear));
            }

            if (!CoverPath.Equals(StandardCover))
            {
                metadataXML.Add(new XElement("Cover", CoverPath));
            }

            return metadataXML;
        }

        public new void FromXML(XElement xmlElement)
        {
            base.FromXML(xmlElement);

            Path = XMLHelper.GetSingleValue(xmlElement, "Path");
            Genre = XMLHelper.GetSingleValue(xmlElement, "Genre");

            Contributors.FromXML(xmlElement);

            string strReleaseYear = XMLHelper.GetSingleValue(xmlElement, "ReleaseYear");
            if (Regex.IsMatch(strReleaseYear, "[0-9]+"))
            {
                ReleaseYear = int.Parse(strReleaseYear);
            }
        }

        public override object Clone()
        {
            AudiobookMetadata copy = new AudiobookMetadata();

            copy.Title = Title;
            copy.Description = Description;
            copy.Genre = Genre;
            copy.Path = Path;
            copy.MetadataPath = MetadataPath;
            copy.ReleaseYear = ReleaseYear;
            copy.Contributors = (Contributors)Contributors.Clone();

            return copy;
        }

        #endregion

    }
}