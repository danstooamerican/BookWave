using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class BrowseViewModel : ViewModelBase
    {
        #region Properties

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

        #endregion

        #region Commands

        public ICommand EditSelectedCommand { private set; get; }

        #endregion

        #region Constructor

        public BrowseViewModel()
        {
            EditSelectedCommand = new RelayCommand<Audiobook>((a) => EditSelected(a));
        }

        #endregion

        #region Methods

        private void EditSelected(Audiobook audiobook)
        {
            EditLibraryViewModel editLibraryViewModel = ViewModelLocator.Instance.EditLibraryViewModel;
            editLibraryViewModel.Destination = audiobook.Metadata.Path;

            ViewModelLocator.Instance.MainViewModel.SwitchToEditLibraryPage();
        }

        public void ReloadLibrary()
        {
            Audiobooks = new ObservableCollection<Audiobook>();

            foreach (Audiobook audiobook in AudiobookManager.Instance.Audiobooks.Values)
            {
                Audiobooks.Add(audiobook);
            }
        }

        #endregion

    }
}
