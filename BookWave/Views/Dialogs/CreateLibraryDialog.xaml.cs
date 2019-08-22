using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.ViewModel;
using System.Windows.Controls;

namespace BookWave.Desktop.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateLibraryDialog.xaml
    /// </summary>
    public partial class CreateLibraryDialog : DialogWindow
    {

        private CreateLibraryViewModel viewModel;

        public Library CreatedLibrary { get { return viewModel.Library; } }

        /// <summary>
        /// Creates a new SelectLibraryItemDialog.  
        /// </summary>
        /// <param name="parent">parent window</param>
        public CreateLibraryDialog(Page parent) : base(parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.CreateLibraryViewModel;
            viewModel.LibraryCreatedEvent += CloseDialog;
            this.DataContext = viewModel;
        }

        private void CloseDialog()
        {
            viewModel.LibraryCreatedEvent -= CloseDialog;
            this.DialogResult = true;
        }

    }
}
