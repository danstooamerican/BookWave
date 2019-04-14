using Commons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Commons.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        private string mCoverImage;

        public string CoverImage
        {
            get { return mCoverImage; }
            set { mCoverImage = value; }
        }

        public MainWindowViewModel()
        {
            CoverImage = "/Commons.Styles;component/Resources/Player/sampleCover.jpg";
        }

    }
}
