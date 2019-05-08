using ATL;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Commons.Models
{
    /// <summary>
    /// A single chapter in an audiobook with a list of AudioPaths and Metadata.
    /// </summary>
    public class Chapter : ObservableObject
    {

        #region Public Properties

        private List<AudioPath> mAudioPaths;
        /// <summary>
        /// List of AudioPaths to allow a chapter to include more than 
        /// one audio file or just a part of it.
        /// </summary>
        public List<AudioPath> AudioPaths
        {
            get { return mAudioPaths; }
            set { Set<List<AudioPath>>(() => this.AudioPaths, ref mAudioPaths, value); }
        }

        private Metadata mMetadata;
        /// <summary>
        /// Metadata of the chapter.
        /// </summary>
        public Metadata Metadata
        {
            get { return mMetadata; }
            set { mMetadata = value; }
        }

        #endregion

        #region Constructors

        private Chapter()
        {
            AudioPaths = new List<AudioPath>();
            Metadata = new Metadata();
        }

        /// <summary>
        /// Creates a chapter off a single audio file and initializes 
        /// the metadata with the track's metadata.
        /// </summary>
        /// <param name="track">Track to reference</param>
        public Chapter(Track track) : this()
        {
            AudioPaths.Add(new AudioPath(track.Path, 0, -1));
            Metadata = new Metadata(track);
        }

        public Chapter(Metadata metadata) : this()
        {
            Metadata = metadata;
        }

        #endregion
    }
}
