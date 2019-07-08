using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        private ObservableCollection<Audiobook> mAudiobooks;
        public ObservableCollection<Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ObservableCollection<Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        /// <summary>
        /// Fetches all currently loaded audio books from the AudiobookManager and displays them.
        /// </summary>
        public void ReloadLibrary()
        {
            Audiobooks = new ObservableCollection<Audiobook>();

            AudiobookManager.Instance.PopulateAudiobookList(Audiobooks);
        }

    }
}
