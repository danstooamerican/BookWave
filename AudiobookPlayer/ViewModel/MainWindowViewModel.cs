using Commons.Logic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Public Properties

        private string mCoverImage;

        public string CoverImage
        {
            get { return mCoverImage; }
            set { mCoverImage = value; }
        }

        private AudiobookManager mAudiobookManager;

        public AudiobookManager AudiobookManager
        {
            get { return mAudiobookManager; }
            set { mAudiobookManager = value; }
        }


        #endregion

        #region Commands

        public ICommand PageBackCommand { get; private set; }
        public ICommand PageForwardCommand { get; private set; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            CoverImage = "/Commons.Styles;component/Resources/Player/sampleCover.jpg";

        }

        #endregion

        #region Methods

        
        #endregion

    }
}
