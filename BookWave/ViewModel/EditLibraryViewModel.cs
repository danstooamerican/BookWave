using BookWave.Desktop.AudiobookManagement;
using BookWave.Desktop.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace BookWave.ViewModel
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
                        Audiobook = AudiobookManager.Instance.CreateAudiobook();
                        UpdateIsInLibrary();
                    }
                }
            }
        }

        public ICollection<Library> Libraries {
            get
            {
                return LibraryManager.Instance.GetLibraries();
            }
        }

        private Library mLibrary;
        public Library Library
        {
            get { return mLibrary; }
            set { Set<Library>(() => this.Library, ref mLibrary, value); }
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

        public ICommand SplitChapterCommand { private set; get; }

        #endregion

        #region Constructors

        public EditLibraryViewModel()
        {
            Audiobook = AudiobookManager.Instance.CreateAudiobook();
            if (LibraryManager.Instance.GetLibraries().Count > 0)
            {
                Library = LibraryManager.Instance.GetLibrary(0);
            }           

            SelectFolderCommand = new RelayCommand(SelectFolder);
            SaveAudiobookCommand = new RelayCommand(SaveAudiobook, CanSaveAudiobook);
            SelectCoverImageCommand = new RelayCommand(SelectCoverImage, CanSelectCoverImage);
            RemoveCoverImageCommand = new RelayCommand(RemoveCoverImage, CanRemoveCoverImage);
            CopyCoverImageFromClipboardCommand = new RelayCommand(CopyCoverImageFromClipboard, CanCopyCoverImageFromClipboard);
            RemoveAudiobookCommand = new RelayCommand(RemoveAudiobook);
            SplitChapterCommand = new RelayCommand<Chapter>((c) => SplitChapter(c));
        }

        #endregion

        #region Methods

        public void RaiseLibrariesChanged()
        {
            RaisePropertyChanged(nameof(Libraries));
        }

        /// <summary>
        /// Checks whether the current audiobook is in the library and sets the IsInLibrary property.
        /// </summary>
        private void UpdateIsInLibrary()
        {
            if (Library == null || Audiobook == null)
            {
                IsInLibrary = false;
            }
            else
            {
                IsInLibrary = Library.Contains(Audiobook);
            }
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
                Audiobook = AudiobookManager.Instance.CreateAudiobook();
                Audiobook.Metadata.Path = Destination;
                Audiobook.SetChapters(new ObservableCollection<Chapter>(Library.Scanner.ScanAudiobookFolder(Destination)));

                if (Audiobook.Metadata.Title.Equals(string.Empty))
                {
                    Audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(Audiobook.Metadata.Path);
                }
            }

            Library = Audiobook.Library;

            UpdateIsInLibrary();
        }

        /// <summary>
        /// Saves an audiobook to the selected library.
        /// </summary>
        private void SaveAudiobook()
        {
            if (!Audiobook.Metadata.Title.Equals(string.Empty))
            {
                AudiobookManager.Instance.UpdateAudiobook(Library, Audiobook);
            }
            else
            {
                throw new InvalidArgumentException("audiobook title is required");
            }

            UpdateIsInLibrary();
        }

        /// <summary>
        /// Removes an audiobook from its library.
        /// </summary>
        public void RemoveAudiobook()
        {
            AudiobookManager.Instance.RemoveAudiobook(Audiobook);
            Destination = string.Empty;
        }

        public void SelectCoverImage()
        {
            if (CanSelectCoverImage())
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

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string saveToPath = Path.Combine(Audiobook.Metadata.MetadataPath, "cover.jpg");

                        Image image = Image.FromFile(openFileDialog.FileName);
                        Image resized = BookWave.Desktop.Util.ImageConverter.Resize(image, 512, 512);

                        Task.Factory.StartNew(() =>
                        {
                            BookWave.Desktop.Util.ImageConverter.SaveCompressedImage(resized, saveToPath);
                        }).ContinueWith((e) =>
                        {
                            Audiobook.Metadata.RaiseCoverChanged();
                        });
                    }
                }
            }
        }

        private void RemoveCoverImage()
        {
            if (Audiobook.Metadata.HasCoverPath)
            {
                File.Delete(Audiobook.Metadata.CoverPath);
                Audiobook.Metadata.RaiseCoverChanged();
            }
        }

        private void CopyCoverImageFromClipboard()
        {
            //TODO implement this method
        }

        private bool CanSelectCoverImage()
        {
            return Library.Contains(Audiobook);
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

        /// <summary>
        /// Opens the split chapter page for the selected chapter.
        /// </summary>
        /// <param name="chapter">selected chapter</param>
        private void SplitChapter(Chapter chapter)
        {
            ViewModelLocator.Instance.MainViewModel.SwitchToSplitChapterPage(chapter);
        }

        #endregion

    }
}
