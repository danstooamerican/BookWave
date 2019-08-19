using GalaSoft.MvvmLight;

namespace BookWave.ViewModel
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
            CoverImage = "/BookWave.Styles;component/Resources/Player/sampleCover.png";
        }

        #endregion

    }
}
