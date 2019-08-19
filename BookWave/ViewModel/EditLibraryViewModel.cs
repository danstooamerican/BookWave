﻿using BookWave.Desktop.AudiobookManagement;
using BookWave.Desktop.AudiobookManagement.Dialogs;
using BookWave.Desktop.Exceptions;
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

                var tmpAudiobook = AudiobookManager.Instance.GetAudiobook(Destination);
                if (tmpAudiobook != null)
                {
                    Audiobook = (Audiobook)tmpAudiobook.Clone();
                    Library = Audiobook.Library;
                }
                else
                {
                    Audiobook.Metadata.Path = Destination;
                }
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
            set { Set<Library>(() => this.Library, ref mLibrary, value); }
        }


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

        private ICollectionView mChapters;
        public ICollectionView Chapters
        {
            get { return mChapters; }
            set { Set<ICollectionView>(() => this.Chapters, ref mChapters, value); }
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

        public ICommand BrowseLibraryCommand { private set; get; }

        public ICommand CreateLibraryCommand { private set; get; }

        #endregion

        #region Constructors

        public EditLibraryViewModel()
        {
            Audiobook = new AudiobookDummy();
            if (LibraryManager.Instance.GetLibraries().Count > 0)
            {
                Library = LibraryManager.Instance.GetLibrary(0);
            }

            SelectFolderCommand = new RelayCommand(SelectFolder, CanSelectFolder);
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
            RaisePropertyChanged(nameof(IsInLibrary));
            RaisePropertyChanged(nameof(AudiobookSelected));
            Task.Factory.StartNew(() =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Chapters = CollectionViewSource.GetDefaultView(Audiobook.Chapters);
                    Chapters.SortDescriptions.Clear();
                    Chapters.SortDescriptions.Add(new SortDescription("Metadata.TrackNumber", ListSortDirection.Ascending));
                }));
            });

        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the Destination property.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Destination = folderBrowserDialog.SelectedPath;
            }
        }

        private void BrowseLibrary()
        {
            SelectLibraryItemDialog dialog = new SelectLibraryItemDialog(Page);

            if (dialog.ShowDialog() == SelectLibraryItemDialog.ITEM_SELECTED)
            {
                Destination = dialog.Selected.Metadata.Path;
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
                    if (!Audiobook.Metadata.Path.Equals(string.Empty))
                    {
                        openFileDialog.InitialDirectory = Audiobook.Metadata.Path;
                    }
                    openFileDialog.Filter = ConfigurationManager.AppSettings.Get("allowed_image_extensions");
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

        private bool CanSelectFolder()
        {
            return Library != null;
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
