using Commons.Controls;
using Commons.Logic;
using Commons.Util;
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

        private HistoryList<MenuButton> mNavigationHistory;
        public HistoryList<MenuButton> NavigationHistory
        {
            get { return mNavigationHistory; }
            set { mNavigationHistory = value; }
        }



        #endregion

        #region Commands

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            SelectedPageTitle = string.Empty;
            NavigationHistory = new HistoryList<MenuButton>();
        }

        #endregion

        #region Methods

        public void SelectPage(MenuButton clickedBtn)
        {
            NavigationHistory.AddAtCurrentElementDeleteBehind(clickedBtn);
        }
        
        #endregion

    }
}
