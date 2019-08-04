using BookWave.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateLibraryDialog.xaml
    /// </summary>
    public partial class CreateLibraryDialog : DialogWindow
    {

        private CreateLibraryViewModel viewModel;

        /// <summary>
        /// Creates a new SelectLibraryItemDialog.  
        /// </summary>
        /// <param name="parent">parent window</param>
        public CreateLibraryDialog(Page parent) : base(parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.CreateLibraryViewModel;
            this.DataContext = viewModel;            
        }

    }
}
