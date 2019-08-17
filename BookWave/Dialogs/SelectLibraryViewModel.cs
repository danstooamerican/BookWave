using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
{
    public class SelectLibraryViewModel : ViewModelBase
    {

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

        public ICollection<Library> Libraries {
            get
            {
                return LibraryManager.Instance.GetLibraries();
            }
        }

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
                        Audiobooks.SortDescriptions.Add(new SortDescription("Metadata.Title", ListSortDirection.Ascending));
                    }));
                });
            }            
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
