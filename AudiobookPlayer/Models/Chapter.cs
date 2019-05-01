using ATL;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Commons.Models
{
    public class Chapter : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<AudioPath> mAudioPaths;
        public ObservableCollection<AudioPath> AudioPaths
        {
            get { return mAudioPaths; }
            set { Set<ObservableCollection<AudioPath>>(() => this.AudioPaths, ref mAudioPaths, value); }
        }

        private Metadata mMetadata;
        public Metadata Metadata
        {
            get { return mMetadata; }
            set { mMetadata = value; }
        }

        #endregion

        #region Constructors

        public Chapter(Track track)
        {
            AudioPaths = new ObservableCollection<AudioPath>();
            Metadata = new Metadata(track);
        }

        #endregion

    }
}
