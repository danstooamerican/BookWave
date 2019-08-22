using GalaSoft.MvvmLight;
using System.Windows.Forms;

namespace BookWave.Desktop.Views.Dialogs
{
    public class ImportAudiobookViewModel : ViewModelBase
    {

        #region Properties

        public delegate void LibraryCreated();

        public event LibraryCreated LibraryCreatedEvent;

        #endregion

        #region Commands

        #endregion

        #region Constructors

        public ImportAudiobookViewModel()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the LibraryPath.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {

            }
        }

        #endregion

    }
}
