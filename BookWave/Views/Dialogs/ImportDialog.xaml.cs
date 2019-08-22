using BookWave.Desktop.Views.Dialogs;
using BookWave.ViewModel;
using System.Windows.Controls;

namespace BookWave.Desktop.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : DialogWindow
    {

        private ImportAudiobookViewModel viewModel;

        /// <summary>
        /// Creates a new ImportDialog.  
        /// </summary>
        /// <param name="parent">parent window</param>
        public ImportDialog(Page parent) : base(parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.ImportAudiobookViewModel;
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
