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

        private Chapter mChapter;
        public Chapter Chapter
        {
            get { return mChapter; }
            set
            {
                Set<Chapter>(() => this.Chapter, ref mChapter, value);
            }
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
