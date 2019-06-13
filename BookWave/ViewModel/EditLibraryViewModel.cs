using Commons.Exceptions;
using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class EditLibraryViewModel : ViewModelBase
    {

        #region Public Properties

        private Audiobook mAudiobook;

        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            set { Set<Audiobook>(() => this.Audiobook, ref mAudiobook, value); }
        }

        public List<Audiobook> AudiobookLibrary {
            get
            {
                return new List<Audiobook>(AudiobookManager.Instance.Audiobooks);
            }
        }

        #endregion

        #region Commands
        public ICommand SelectFolderCommand { private set; get; }

        public ICommand SaveAudiobookCommand { private set; get; }

        #endregion

        #region Constructors
        
        public EditLibraryViewModel()
        {
            Audiobook = new Audiobook();

            SelectFolderCommand = new RelayCommand(SelectFolder);
            SaveAudiobookCommand = new RelayCommand(SaveAudiobook, CanSaveAudiobook);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the FolderPath of the AudiobookFolder.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Audiobook.Metadata.Path = folderBrowserDialog.SelectedPath;
            }
        }

        public void AnalyzeFolder()
        {
            var tmpAudiobook = AudiobookManager.Instance.GetAudiobook(Audiobook.Metadata.Path);
            if (tmpAudiobook != null)
            {
                Audiobook = tmpAudiobook;
            }

            Audiobook.Chapters = new ObservableCollection<Chapter>(AudiobookFolder.AnalyzeFolder(Audiobook.Metadata.Path));
            if (Audiobook.Metadata.Title.Equals(string.Empty))
            {
                Audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(Audiobook.Metadata.Path);
            }
            
        }

        private void SaveAudiobook()
        {
            AudiobookFolder.SaveAudiobookMetadata(Audiobook);

            if (!AudiobookManager.Instance.Audiobooks.Contains(Audiobook))
            {
                if (!Audiobook.Metadata.Title.Equals(string.Empty))
                {
                    AudiobookManager.Instance.AddAudioBook(Audiobook);
                } else
                {
                    throw new InvalidArgumentException("audiobook title is required");
                }                
            }                        
        }

        private bool CanSaveAudiobook()
        {
            return Audiobook.Chapters.Count > 0;
        }

        #endregion

    }
}
