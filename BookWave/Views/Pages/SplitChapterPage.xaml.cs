using BookWave.ViewModel;
using System.Windows.Controls;

namespace BookWave.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for AuthorsPage.xaml
    /// </summary>
    public partial class SplitChapter : Page
    {

        private SplitChapterViewModel viewModel;

        public SplitChapter()
        {
            viewModel = ViewModelLocator.Instance.SplitChapterViewModel;
            DataContext = viewModel;
        }
    }
}
