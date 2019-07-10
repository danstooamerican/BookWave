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

        private string mAudioFilePath;
        public string AudioFilePath
        {
            get { return mAudioFilePath; }
            set
            {
                Set<string>(() => this.AudioFilePath, ref mAudioFilePath, value);
            }
        }

        public string AudioFileName
        {
            get { return Path.GetFileName(AudioFilePath); }
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
