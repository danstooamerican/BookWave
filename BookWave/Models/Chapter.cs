using ATL;
using Commons.Util;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Commons.Models
{
    /// <summary>
    /// A single chapter in an audiobook with a list of AudioPaths and Metadata.
    /// </summary>
    public class Chapter : ObservableObject, XMLSaveObject
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
        public Chapter(Track track)
        {
            AudioPaths = new List<AudioPath>();
            AudioPaths.Add(new AudioPath(track.Path, 0, -1));
            Metadata = new ChapterMetadata(track);
        }

        public Chapter()
        {
            AudioPaths = new List<AudioPath>();
            Metadata = new ChapterMetadata();
        }

        public Chapter(ChapterMetadata metadata, List<AudioPath> audioPaths)
        {
            Metadata = metadata;
            AudioPaths = audioPaths;
        }

        public XElement ToXML()
        {
            var chapterXML = new XElement("Chapter");
            var audioPaths = new XElement("AudioPaths");

            foreach (AudioPath audioPath in AudioPaths)
            {                
                audioPaths.Add(audioPath.ToXML());
            }
            chapterXML.Add(audioPaths);

            chapterXML.Add(Metadata.ToXML());

            return chapterXML;
        }

        public void FromXML(XElement xmlElement)
        {
            foreach (var element in xmlElement.Descendants("AudioPath"))
            {
                AudioPath audioPath = new AudioPath();
                audioPath.FromXML(element);
                AudioPaths.Add(audioPath);
            }

            Metadata.FromXML(xmlElement);
        }

        #endregion
    }
}
