using Commons.Exceptions;
using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class EditLibraryViewModel : ViewModelBase
    {

        #region Public Properties

        private string mDestination;
        public string Destination
        {
            get { return mDestination; }
            set
            {
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.Destination, ref mDestination, value.Trim());
                    AnalyzeFolder();
                }
                else
                {
                    if (value != null && value.Equals(string.Empty))
                    {
                        Set<string>(() => this.Destination, ref mDestination, value);
                        AnalyzeFolder();
                    }
                }
            }
        }


        private Audiobook mAudiobook;
        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            set
            {
                Set<Audiobook>(() => this.Audiobook, ref mAudiobook, value);
                UpdateIsInLibrary();
            }
        }

        public List<Audiobook> AudiobookLibrary {
            get
            {
                return new List<Audiobook>(AudiobookManager.Instance.Audiobooks.Values);
            }
        }

        private bool mIsInLibrary;
        public bool IsInLibrary
        {
            get { return mIsInLibrary; }
            set { Set<bool>(() => this.IsInLibrary, ref mIsInLibrary, value); }
        }

        #endregion

        #region Commands
        public ICommand SelectFolderCommand { private set; get; }

        public ICommand SaveAudiobookCommand { private set; get; }

        public ICommand RemoveAudiobookCommand { private set; get; }

        public ICommand SelectCoverImageCommand { private set; get; }

        public ICommand RemoveCoverImageCommand { private set; get; }

        public ICommand CopyCoverImageFromClipboardCommand { private set; get; }

        #endregion

        #region Constructors

        public EditLibraryViewModel()
        {
            Audiobook = new Audiobook();

            SelectFolderCommand = new RelayCommand(SelectFolder);
            SaveAudiobookCommand = new RelayCommand(SaveAudiobook, CanSaveAudiobook);
            SelectCoverImageCommand = new RelayCommand(SelectCoverImage);
            RemoveCoverImageCommand = new RelayCommand(RemoveCoverImage, CanRemoveCoverImage);
            CopyCoverImageFromClipboardCommand = new RelayCommand(CopyCoverImageFromClipboard, CanCopyCoverImageFromClipboard);
            RemoveAudiobookCommand = new RelayCommand(RemoveAudiobook);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the current audiobook is in the library and sets the IsInLibrary property.
        /// </summary>
        private void UpdateIsInLibrary()
        {
            IsInLibrary = Audiobook != null && AudiobookManager.Instance.AudiobookRepo.Items.Contains(Audiobook.Metadata.Path);
        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the FolderPath of the AudiobookFolder.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Destination = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Analyzes folder for Audiobook.
        /// </summary>
        public void AnalyzeFolder()
        {
            var tmpAudiobook = AudiobookManager.Instance.GetAudiobook(Destination);
            if (tmpAudiobook != null)
            {
                Audiobook = (Audiobook)tmpAudiobook.Clone();
            }
            else
            {
                Audiobook = AudiobookManager.Instance.LoadAudiobookFromFile(Destination);
            }

            Audiobook.Chapters = new ObservableCollection<Chapter>(AudiobookFolder.AnalyzeFolder(Audiobook.Metadata.Path));
            if (Audiobook.Metadata.Title.Equals(string.Empty))
            {
                Audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(Audiobook.Metadata.Path);
            }

            UpdateIsInLibrary();
        }

        private void SaveAudiobook()
        {
            AudiobookFolder.SaveAudiobookMetadata(Audiobook);

            if (!AudiobookManager.Instance.Contains(Audiobook.ID))
            {
                if (!Audiobook.Metadata.Title.Equals(string.Empty))
                {
                    AudiobookManager.Instance.UpdateAudioBook(Audiobook);
                } else
                {
                    throw new InvalidArgumentException("audiobook title is required");
                }                
            }

            UpdateIsInLibrary();
        }

        public void RemoveAudiobook()
        {
            AudiobookManager.Instance.RemoveAudioBook(Audiobook.ID);
            Destination = string.Empty;
            AnalyzeFolder();
        }

        public void SelectCoverImage()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (!Audiobook.Metadata.Path.Equals(string.Empty))
                {
                    openFileDialog.InitialDirectory = Audiobook.Metadata.Path;
                }
                openFileDialog.Filter = "JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|BMP|*.bmp|GIF|*.gif";
                openFileDialog.Title = "Choose a cover image";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Get the path of specified file
                    Audiobook.Metadata.CoverPath = openFileDialog.FileName;
                }
            }
        }

        private void RemoveCoverImage()
        {
            Audiobook.Metadata.CoverPath = string.Empty;
            Destination = string.Empty;
        }

        private void CopyCoverImageFromClipboard()
        {
            //TODO implement this method
        }

        private bool CanRemoveCoverImage()
        {
            return Audiobook.Metadata.HasCoverPath;
        }

        private bool CanSaveAudiobook()
        {
            return Audiobook.Chapters.Count > 0;
        }

        private bool CanCopyCoverImageFromClipboard()
        {
            return Clipboard.ContainsImage();
        }

        #endregion

    }
}
