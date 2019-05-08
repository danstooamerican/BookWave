using Commons.Models;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Commons.Logic
{
    public class AudiobookManager : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<Audiobook> mAudiobooks;
        public ObservableCollection<Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ObservableCollection<Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        #endregion

        #region Constructors

        public AudiobookManager()
        {
            Audiobooks = new ObservableCollection<Audiobook>();
        }

        #endregion

        #region Methods

        public void AddAudiobook(string path)
        {

        }

        #endregion

    }
}
