using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

    }
}
