﻿using ATL;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Commons.Models
{
    /// <summary>
    /// A single chapter in an audiobook with a list of AudioPaths and metadata.
    /// </summary>
    public class Chapter : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<AudioPath> mAudioPaths;
        /// <summary>
        /// List of AudioPaths to allow a chapter to include more than 
        /// one audio file or just a part of it.
        /// </summary>
        public ObservableCollection<AudioPath> AudioPaths
        {
            get { return mAudioPaths; }
            set { Set<ObservableCollection<AudioPath>>(() => this.AudioPaths, ref mAudioPaths, value); }
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

        /// <summary>
        /// Creates a chapter off a single audio file and initializes 
        /// the metadata with the track's metadata.
        /// </summary>
        /// <param name="track">Track to reference</param>
        public Chapter(Track track)
        {
            AudioPaths = new ObservableCollection<AudioPath>();
            AudioPaths.Add(new AudioPath(track.Path, 0, -1));
            Metadata = new Metadata(track);
        }

        public Chapter(XDocument metadataDoc)
        {
            
            //metadata
        }

        #endregion

        #region Methods

        public XDocument ToXML()
        {
            var chapter = new XElement("Chapter");
            var audioPaths = new XElement("AudioPaths");
            foreach (AudioPath audioPath in AudioPaths)
            {
                var pathXML = new XElement("AudioPath");
                pathXML.Add(new XElement("FilePath", audioPath.Path));
                pathXML.Add(new XElement("StartMark", audioPath.StartMark));
                pathXML.Add(new XElement("EndMark", audioPath.EndMark));
                audioPaths.Add(pathXML);
            }
            chapter.Add(audioPaths);

            if (!Metadata.Title.Equals(string.Empty)) //TODO maybe != null?
            {
                chapter.Add(new XElement("Title", Metadata.Title));
            } 
            if (Metadata.TrackNumber != 0)
            {
                chapter.Add(new XElement("TrackNumber", Metadata.TrackNumber));
            }
            if (!Metadata.Description.Equals(string.Empty))
            {
                chapter.Add(new XElement("Description", Metadata.Description));
            }
            if (Metadata.ReleaseYear != 0) //TODO what is standard value?
            {
                chapter.Add(new XElement("ReleaseYear", Metadata.ReleaseYear));
            }
            if (Metadata.Contributors.Authors.Count != 0)
            {
                XElement authors = new XElement("Authors");
                foreach (string author in Metadata.Contributors.Authors)
                {
                    authors.Add(new XElement("Author", author));
                }
                chapter.Add(authors);
            }
            if (Metadata.Contributors.Readers.Count != 0)
            {
                XElement readers = new XElement("Readers");
                foreach (string reader in Metadata.Contributors.Authors)
                {
                    readers.Add(new XElement("Reader", reader));
                }
                chapter.Add(readers);
            }
            var metadataXML = new XDocument();
            metadataXML.Add(chapter);
            return metadataXML;
        }
        #endregion
    }
}
