using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CoverImage = "/Commons.Styles;component/Resources/Player/sampleCover.jpg";
        }

        #endregion

    }
}
