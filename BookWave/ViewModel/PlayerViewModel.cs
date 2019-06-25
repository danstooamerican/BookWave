using GalaSoft.MvvmLight;

namespace Commons.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {

        #region Public Properties

        private string mCoverImage;

        public string CoverImage
        {
            get { return mCoverImage; }
            set { mCoverImage = value; }
        }

        #endregion

        #region Constructor

        public PlayerViewModel()
        {
            CoverImage = "/Commons.Styles;component/Resources/Player/sampleCover.png";
        }

        #endregion

    }
}
