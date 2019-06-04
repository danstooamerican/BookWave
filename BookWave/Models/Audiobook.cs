using Commons.Logic;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

namespace Commons.Models
{
    /// <summary>
    /// Represents a single audiobook with a list of chapters and metadata.
    /// </summary>
    public class Audiobook : ObservableObject
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

        /// <summary>
        /// Creates a new audiobook based on a directory. It scans the metadata
        /// folder and creates for each metadata file a chapter.
        /// </summary>
        /// <param name="path">Path to the main audiobook directory.</param>
        public Audiobook(string path)
        {
            List<Chapter> chapters = AudiobookFolder.LoadAudiobookChapters(
                Path.Combine(path, ConfigurationManager.AppSettings.Get("metadata_folder")));

            Chapters = new ObservableCollection<Chapter>(chapters);
            Metadata = new AudiobookMetadata();
            Metadata.Path = path;
        }

    }
}
