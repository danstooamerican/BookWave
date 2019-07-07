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

        public void ReloadLibrary()
        {
            Audiobooks = new ObservableCollection<Audiobook>();
            Random rnd = new Random();

            foreach (Audiobook audiobook in AudiobookManager.Instance.Audiobooks.Values)
            {
                var t = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(rnd.Next(200));
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Audiobooks.Add(audiobook);
                    }));
                });
            }
        }

    }
}
