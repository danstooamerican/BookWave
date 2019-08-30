using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Notifications;
using BookWave.Desktop.Views.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Image = System.Drawing.Image;

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
                Set<string>(() => this.Destination, ref mDestination, value);
            }
        }

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
            }
        }

        public bool LibrarySelected { get { return Library != null; } }

        private Audiobook mAudiobook;
        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            set
            {
                Set<Audiobook>(() => this.Audiobook, ref mAudiobook, value);
                RaiseAudiobookChanged();
            }
        }

        public bool IsInLibrary
        {
            get { return Library != null && Library.Contains(Audiobook); }
        }

        public bool AudiobookSelected { get { return Audiobook.ID != AudiobookDummy.DummyId; } }

        private Page mPage;

        public Page Page
        {
            get { return mPage; }
            set { mPage = value; }
        }

        private ListCollectionView mChapters;
        public ListCollectionView Chapters
        {
            get { return mChapters; }
            set { Set<ListCollectionView>(() => this.Chapters, ref mChapters, value); }
        }


        #endregion

        #region Commands

        public ICommand ResolveAudiobookPathWarningCommand { private set; get; }

        public ICommand ResolveChapterPathWarningCommand { private set; get; }

        public ICommand ImportAudiobookCommand { private set; get; }

        public ICommand SaveAudiobookCommand { private set; get; }

        public ICommand RemoveAudiobookCommand { private set; get; }

        public ICommand SelectCoverImageCommand { private set; get; }

        public ICommand RemoveCoverImageCommand { private set; get; }

        public ICommand CopyCoverImageFromClipboardCommand { private set; get; }

        public ICommand SplitChapterCommand { private set; get; }

        public ICommand BrowseLibraryCommand { private set; get; }

        public ICommand CreateLibraryCommand { private set; get; }

        #endregion

        #region Constructors

        public EditLibraryViewModel()
        {
            Audiobook = new AudiobookDummy();

            ResolveAudiobookPathWarningCommand = new RelayCommand(ResolveAudiobookPathWarning);
            ResolveChapterPathWarningCommand = new RelayCommand<Chapter>((c) => ResolveChapterPathWarning(c));
            ImportAudiobookCommand = new RelayCommand(ImportAudiobook);
            SaveAudiobookCommand = new RelayCommand(SaveAudiobook, CanSaveAudiobook);
            SelectCoverImageCommand = new RelayCommand(SelectCoverImage, CanSelectCoverImage);
            RemoveCoverImageCommand = new RelayCommand(RemoveCoverImage, CanRemoveCoverImage);
            CopyCoverImageFromClipboardCommand = new RelayCommand(CopyCoverImageFromClipboard, CanCopyCoverImageFromClipboard);
            RemoveAudiobookCommand = new RelayCommand(RemoveAudiobook);
            SplitChapterCommand = new RelayCommand<Chapter>((c) => SplitChapter(c));
            BrowseLibraryCommand = new RelayCommand(BrowseLibrary, CanBrowseLibrary);
            CreateLibraryCommand = new RelayCommand(CreateLibrary);
        }

        #endregion

        #region Methods

        private void RaiseAudiobookChanged()
        {
            Destination = Audiobook.Metadata.Path;
            Library = Audiobook.Library;

            RaisePropertyChanged(nameof(IsInLibrary));
            RaisePropertyChanged(nameof(AudiobookSelected));
            Task.Factory.StartNew(() =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Chapters = CollectionViewSource.GetDefaultView(Audiobook.Chapters) as ListCollectionView;
                    Chapters.CustomSort = Comparer<Chapter>.Create((x, y) =>
                    {
                        var xnumber = x.Metadata.TrackNumber;
                        var ynumber = y.Metadata.TrackNumber;
                        if (string.IsNullOrEmpty(xnumber))
                        {
                            return -1;
                        }
                        if (string.IsNullOrEmpty(ynumber))
                        {
                            return 1;
                        }
                        int xint;
                        int yint;

                        int.TryParse(xnumber, out xint);
                        int.TryParse(ynumber, out yint);
                        return xint.CompareTo(yint);
                    });
                }));
            });
        }

        /// <summary>
        /// Opens a OpenFileDialog and sets the Path property of the chapter.
        /// </summary>
        private void ResolveChapterPathWarning(Chapter chapter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ConfigurationManager.AppSettings.Get("allowed_audio_extensions_filter");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Audiobook.SetChapterPath(chapter, openFileDialog.FileName);
                    RaiseAudiobookChanged();
                }
                catch (FileNotFoundException)
                {
                    NotificationManager.DisplayException("File not found.");
                }
                catch (InvalidArgumentException)
                {
                    NotificationManager.DisplayException("Path could not be set.");
                }
            }
        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the Destination property of the audiobook.
        /// </summary>
        private void ResolveAudiobookPathWarning()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Audiobook.SetPath(folderBrowserDialog.SelectedPath);
                    RaiseAudiobookChanged();
                }
                catch (FileNotFoundException)
                {
                    NotificationManager.DisplayException("File not found.");
                }               
            }
        }

        private void ImportAudiobook()
        {
            ImportDialog dialog = new ImportDialog(Page);

            if (dialog.ShowDialog() == true)
            {

            }
        }

        private void BrowseLibrary()
        {
            SelectLibraryItemDialog dialog = new SelectLibraryItemDialog(Page);

            if (dialog.ShowDialog() == true)
            {
                Audiobook = (Audiobook)dialog.Selected.Clone();
            }
        }

        private void CreateLibrary()
        {
            CreateLibraryDialog dialog = new CreateLibraryDialog(Page);
            if (dialog.ShowDialog() == true)
            {
                RaisePropertyChanged(nameof(Libraries));
                Library = dialog.CreatedLibrary;
            }
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

            RaiseAudiobookChanged();
        }

        /// <summary>
        /// Removes an audiobook from its library.
        /// </summary>
        public void RemoveAudiobook()
        {
            AudiobookManager.Instance.RemoveAudiobook(Audiobook);
            Destination = string.Empty;
            Audiobook = new AudiobookDummy();
            RaiseAudiobookChanged();
        }

        public void SelectCoverImage()
        {
            if (CanSelectCoverImage())
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (!string.IsNullOrEmpty(Audiobook.Metadata.Path))
                    {
                        openFileDialog.InitialDirectory = Audiobook.Metadata.Path;
                    }
                    openFileDialog.Filter = ConfigurationManager.AppSettings.Get("allowed_image_extensions_filter");
                    openFileDialog.Title = "Choose a cover image";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ChangeCoverImage(Image.FromFile(openFileDialog.FileName));
                    }
                }
            }
        }

        private void ChangeCoverImage(Image image)
        {
            string saveToPath = Path.Combine(Audiobook.Metadata.MetadataPath, "cover.jpg");

            Image resized = BookWave.Desktop.Util.ImageConverter.Resize(image, 512, 512);

            Task.Factory.StartNew(() =>
            {
                BookWave.Desktop.Util.ImageConverter.SaveCompressedImage(resized, saveToPath);
            }).ContinueWith((e) =>
            {
                Audiobook.Metadata.RaiseCoverChanged();
            });
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
            Image image = null;

            if (Clipboard.ContainsImage())
            {
                image = Clipboard.GetImage();
            }
            else
            {
                IDataObject myDataObject = Clipboard.GetDataObject();
                string[] files = (string[])myDataObject.GetData(DataFormats.FileDrop);

                if (files.Length == 1 && BookWave.Desktop.Util.ImageConverter.FileIsValid(files[0]))
                {
                    image = Image.FromFile(files[0]);
                }
            }

            if (image != null)
            {
                ChangeCoverImage(image);
            }
        }

        private bool CanBrowseLibrary()
        {
            return Library != null;
        }

        private bool CanSelectCoverImage()
        {
            return Library != null && Library.Contains(Audiobook);
        }

        private bool CanRemoveCoverImage()
        {
            return Audiobook.Metadata.HasCoverPath;
        }

        private bool CanSaveAudiobook()
        {
            return Audiobook.Chapters.Count > 0
                && Library != null && !string.IsNullOrEmpty(Audiobook.Metadata.Title);
        }

        private bool CanCopyCoverImageFromClipboard()
        {
            IDataObject myDataObject = Clipboard.GetDataObject();
            string[] files = (string[])myDataObject.GetData(DataFormats.FileDrop);

            if (files != null && files.Length == 1)
            {
                return BookWave.Desktop.Util.ImageConverter.FileIsValid(files[0]);
            }

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
