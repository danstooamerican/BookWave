using Commons.Dialogs;
using Commons.Util;
using Commons.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Commons.Pages
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
