using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
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

        #region Methods

        public void UpdateAudiobookList()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Audiobooks = CollectionViewSource.GetDefaultView(AudiobookManager.Instance.GetAllAudiobooks());
                    Audiobooks.SortDescriptions.Add(new SortDescription("Metadata.Title", ListSortDirection.Ascending));
                }));
            });
        }

        #endregion

    }
}
