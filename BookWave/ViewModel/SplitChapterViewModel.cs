using Commons.Exceptions;
using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class SplitChapterViewModel : ViewModelBase
    {

        #region Public Properties

        private string mAudioFile;
        public string AudioFile
        {
            get { return mAudioFile; }
            set
            {
                Set<string>(() => this.AudioFile, ref mAudioFile, value);
            }
        }

        private List<Chapter> mChapters;

        public List<Chapter> Chapters
        {
            get { return mChapters; }
            set { mChapters = value; }
        }


        #endregion

        #region Commands


        #endregion

        #region Constructors

        public SplitChapterViewModel()
        {
            
        }

        #endregion

        #region Methods

        #endregion

    }
}
