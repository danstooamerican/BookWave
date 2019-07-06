using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public void ReloadLibrary()
        {
            Audiobooks = new ObservableCollection<Audiobook>();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Audiobook audiobook in AudiobookManager.Instance.Audiobooks.Values)
                {
                    Audiobooks.Add(audiobook);
                }
            }));
        }

    }
}
