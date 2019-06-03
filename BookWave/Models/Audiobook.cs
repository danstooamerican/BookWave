using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public Audiobook()
        {
            Chapters = new ObservableCollection<Chapter>();
        }

        public void ClearChapters()
        {
            Chapters.Clear();
        }

    }
}
