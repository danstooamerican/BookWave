using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace BookWave.Desktop.Views.Dialogs
{
    public class ImportAudiobookViewModel : ViewModelBase
    {

        #region Properties

        public delegate void Imported();

        public event Imported ImportedEvent;

        public ICollection<Library> Libraries
        {
            get
            {
                return LibraryManager.Instance.GetLibraries();
            }
        }

        private Library mLibrary;
        public Library Library
        {
            get { return mLibrary; }
            set
            {
                Set<Library>(() => this.Library, ref mLibrary, value);
                RaisePropertyChanged(nameof(LibrarySelected));
                RaisePropertyChanged(nameof(LibraryTopFolder));
            }
        }

        public bool LibrarySelected { get { return Library != null; } }

        public string LibraryTopFolder {
            get
            {
                if (LibrarySelected)
                {
                    return Path.GetFileName(Library.LibraryPath) + @"/";
                }
                return string.Empty;
            }
        }

        private string mAudiobookPath;
        public string AudiobookPath
        {
            get { return mAudiobookPath; }
            set { Set<string>(() => this.AudiobookPath, ref mAudiobookPath, value); }
        }

        private string mDestination;
        public string Destination
        {
            get { return mDestination; }
            set { Set<string>(() => this.Destination, ref mDestination, value); }
        }

        #endregion

        #region Commands

        public ICommand SelectFolderCommand { private set; get; }
        public ICommand ImportCommand { private set; get; }

        #endregion

        #region Constructors

        public ImportAudiobookViewModel()
        {
            SelectFolderCommand = new RelayCommand(SelectFolder);
            //ImportCommand = new RelayCommand(Import, CanImport);
            ImportCommand = new RelayCommand(Import); // TODO make CanImport work
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
                var audiobookPath = folderBrowserDialog.SelectedPath;
                CheckFolderNotInLibrary(audiobookPath);
                AudiobookPath = audiobookPath;
                RaisePropertyChanged(nameof(AudiobookPath));
            }
        }

        /// <summary>
        /// Checks that the folder is not already contained in a library.
        /// </summary>
        /// <param name="folderPath">is the path to the folder</param>
        private void CheckFolderNotInLibrary(string folderPath)
        {
            foreach (Library library in LibraryManager.Instance.GetLibraries())
            {
                if (folderPath.StartsWith(library.LibraryPath))
                {
                    throw new AudiobookAlreadyInLibraryException(folderPath + " is already in library " + library.Name);
                }
            }
        }

        public void Import()
        {
            FileSystemHelper.DirectoryCopy(AudiobookPath, Path.Combine(Library.LibraryPath, Destination), true);
            Library.ScanLibrary();
            ImportedEvent();
        }

        public bool CanImport()
        {
            return LibrarySelected && !string.IsNullOrEmpty(Destination) && !string.IsNullOrEmpty(AudiobookPath);
        }

        #endregion

    }
}
