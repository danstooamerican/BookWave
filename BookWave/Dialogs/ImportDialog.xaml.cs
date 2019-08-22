using BookWave.Desktop.AudiobookManagement;
using BookWave.Desktop.AudiobookManagement.Dialogs;
using BookWave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : DialogWindow
    {

        private ImportAudiobookViewModel viewModel;

        public Library CreatedLibrary { get { return viewModel.Library; } }

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
