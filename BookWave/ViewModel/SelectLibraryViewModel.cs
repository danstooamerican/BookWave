using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Commons.ViewModel
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

        #region Constructor

        public SelectLibraryViewModel()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Audiobooks = CollectionViewSource.GetDefaultView(AudiobookManager.Instance.Audiobooks.Values);
                    Audiobooks.SortDescriptions.Add(new SortDescription("Metadata.Title", ListSortDirection.Ascending));
                }));                
            });
        }

        #endregion

    }
}
