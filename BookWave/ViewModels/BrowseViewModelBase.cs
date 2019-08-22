using BookWave.Desktop.Models.AudiobookManagement;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BookWave.ViewModel
{
    public abstract class BrowseViewModelBase : ViewModelBase
    {
        #region Properties

        private Audiobook mSelected;
        public Audiobook Selected
        {
            get { return mSelected; }
            set { Set<Audiobook>(() => this.Selected, ref mSelected, value); }
        }

        private ICollectionView mAudiobooks;
        public ICollectionView Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ICollectionView>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        private Library mLibrary;

        public Library Library
        {
            get { return mLibrary; }
            set
            {
                Set<Library>(() => this.Library, ref mLibrary, value);
                UpdateBrowseList();
            }
        }

        public ICollection<Library> Libraries
        {
            get
            {
                return LibraryManager.Instance.GetLibraries();
            }
        }

        private IDictionary<string, string> FilterOptionsDictionary;
        public ICollection<string> FilterOptions
        {
            get { return FilterOptionsDictionary.Keys; }
        }

        private string mFilterOption;
        public string SelectedFilterOption
        {
            get { return mFilterOption; }
            set
            {
                if (FilterOptionsDictionary.ContainsKey(value))
                {
                    Set<string>(() => this.SelectedFilterOption, ref mFilterOption, value);
                    UpdateBrowseList();
                }
            }
        }


        #endregion

        #region Commands

        public ICommand ScanLibraryCommand { private set; get; }
        public ICommand HardScanLibraryCommand { private set; get; }

        #endregion

        #region Constructor

        public BrowseViewModelBase()
        {
            ScanLibraryCommand = new RelayCommand(() => { ScanLibrary(false); }, CanScanLibrary);
            HardScanLibraryCommand = new RelayCommand(() => { ScanLibrary(true); }, CanScanLibrary);

            FilterOptionsDictionary = new Dictionary<string, string>();
            FilterOptionsDictionary.Add("Title", "Metadata.Title");
            FilterOptionsDictionary.Add("Release Year", "Metadata.ReleaseYear");
            FilterOptionsDictionary.Add("Genre", "Metadata.Genre");
            FilterOptionsDictionary.Add("Author", "Metadata.Contributors.AuthorString");
            FilterOptionsDictionary.Add("Reader", "Metadata.Contributors.ReaderString");

            SelectedFilterOption = "Title";
        }

        #endregion

        #region Methods

        public void UpdateBrowseList()
        {
            if (Library != null)
            {
                Task.Factory.StartNew(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Audiobooks = CollectionViewSource.GetDefaultView(Library.GetAudiobooks());
                        Audiobooks.SortDescriptions.Clear();
                        Audiobooks.SortDescriptions.Add(new SortDescription(FilterOptionsDictionary[SelectedFilterOption], ListSortDirection.Ascending));
                    }));
                });
            }
        }

        private void ScanLibrary(bool hardScan)
        {
            Task.Factory.StartNew(() =>
            {
                Library.ScanLibrary(hardScan);
            }).ContinueWith((e) =>
            {
                UpdateBrowseList();
            });
        }

        private bool CanScanLibrary()
        {
            return Library != null;
        }

        public void UpdateLibrariesList()
        {
            RaisePropertyChanged(nameof(Libraries));

            if (Library == null && Libraries.Count > 0)
            {
                Library = Libraries.ElementAt(0);
            }
        }

        #endregion

    }
}
