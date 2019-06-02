using ATL;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

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

        /// <summary>
        /// Creates a chapter off a single audio file and initializes 
        /// the metadata with the track's metadata.
        /// </summary>
        /// <param name="track">Track to reference</param>
        public Chapter(Track track)
        {
            AudioPaths = new List<AudioPath>();
            AudioPaths.Add(new AudioPath(track.Path, 0, -1));
            Metadata = new Metadata(track);
        }

        public Chapter(Metadata metadata, List<AudioPath> audioPaths)
        {
            Metadata = metadata;
            AudioPaths = audioPaths;
        }

        #endregion
    }
}
