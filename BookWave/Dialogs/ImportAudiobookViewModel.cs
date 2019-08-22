using BookWave.Desktop.AudiobookManagement.Scanner;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
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
