using Commons.Logic;
using Commons.Util;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace Commons.Models
{
    /// <summary>
    /// Represents a single audiobook with a list of chapters and metadata.
    /// </summary>
    public class Audiobook : ObservableObject, XMLSaveObject
    {

        #region Public Properties

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

        /// <summary>
        /// Creates an empty audiobook.
        /// </summary>
        public Audiobook()
        {
            Chapters = new ObservableCollection<Chapter>();
            Metadata = new AudiobookMetadata();
        }

        public void LoadChapters()
        {
            List<Chapter> chapters = AudiobookFolder.LoadAudiobookChapters(
                    Path.Combine(Metadata.Path, ConfigurationManager.AppSettings.Get("metadata_folder")));

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
    }
}