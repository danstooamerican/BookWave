using ATL;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Xml.Linq;

namespace BookWave.Desktop.Models.AudiobookManagement
{
    /// <summary>
    /// A single chapter in an audiobook with a list of AudioPaths and Metadata.
    /// </summary>
    public class Chapter : ObservableObject, XMLSaveObject, ICloneable
    {

        #region Public Properties

        private AudioPath mAudioPath;
        public AudioPath AudioPath
        {
            get { return mAudioPath; }
            set { Set<AudioPath>(() => this.AudioPath, ref mAudioPath, value); }
        }

        private ChapterMetadata mMetadata;
        /// <summary>
        /// Metadata of the chapter.
        /// </summary>
        public ChapterMetadata Metadata
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
        internal Chapter(Track track)
        {
            AudioPath = new AudioPath(track.Path);
            Metadata = new ChapterMetadata(track);
        }

        internal Chapter()
        {
            Metadata = new ChapterMetadata();
            AudioPath = new AudioPath();
        }

        #endregion

        #region Methods
        public XElement ToXML()
        {
            var chapterXML = new XElement("Chapter");
            chapterXML.Add(AudioPath.ToXML());
            chapterXML.Add(Metadata.ToXML());

            return chapterXML;
        }

        public void FromXML(XElement xmlElement)
        {
            AudioPath.FromXML(xmlElement);

            Metadata.FromXML(xmlElement);
        }

        public object Clone()
        {
            Chapter copy = new Chapter();

            copy.Metadata = (ChapterMetadata)Metadata.Clone();

            copy.AudioPath = (AudioPath)AudioPath.Clone();

            return copy;
        }

        #endregion
    }
}
