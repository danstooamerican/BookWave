using Commons.Logic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Public Properties

        private string mSelectedPageTitle;
        public string SelectedPageTitle
        {
            get { return mSelectedPageTitle; }
            set { Set<string>(() => this.SelectedPageTitle, ref mSelectedPageTitle, value); }
        }

        private ControlTemplate mSharedPageTitleTemplate;
        public ControlTemplate SharedPageTitleTemplate
        {
            get { return mSharedPageTitleTemplate; }
            set { Set<ControlTemplate>(() => this.SharedPageTitleTemplate, ref mSharedPageTitleTemplate, value); }
        }


        private AudiobookManager mAudiobookManager;
        public AudiobookManager AudiobookManager
        {
            get { return mAudiobookManager; }
            set { mAudiobookManager = value; }
        }


        #endregion

        #region Commands
        
        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            SelectedPageTitle = string.Empty;
        }

        #endregion

        #region Methods

        
        #endregion

    }
}
