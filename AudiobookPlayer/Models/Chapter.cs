using ATL;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

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
            //TODO: add track as an AudioPath

            Metadata = new Metadata(track);
        }

        #endregion

    }
}
