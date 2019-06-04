﻿using Commons.Exceptions;
using GalaSoft.MvvmLight;
using System.IO;

namespace Commons.Models
{
    /// <summary>
    /// Metadata for audiobooks which adds a Genre Option.
    /// </summary>
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

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
                    Set<string>(() => this.Path, ref mPath, value);
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

        #endregion

        #region Constructor
        public AudiobookMetadata() : base()
        {
            Path = string.Empty;
            Genre = string.Empty;
        }
        #endregion

    }
}
