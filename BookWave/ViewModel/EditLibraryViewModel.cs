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

        private string mDestination;
        public string Destination
        {
            get { return mDestination; }
            set
            {
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.Destination, ref mDestination, value.Trim());
                    AnalyzeFolder();
                }
                else
                {
                    if (value != null && value.Equals(string.Empty))
                    {
                        Set<string>(() => this.Destination, ref mDestination, value);
                    }
                }
            }
        }


        private Audiobook mAudiobook;
        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            set
            {
                Set<Audiobook>(() => this.Audiobook, ref mAudiobook, value);
                UpdateIsInLibrary();
            }
        }

        public List<Audiobook> AudiobookLibrary {
            get
            {
                return new List<Audiobook>(AudiobookManager.Instance.Audiobooks);
            }
        }

        private bool mIsInLibrary;
        public bool IsInLibrary
        {
            get { return mIsInLibrary; }
            set { Set<bool>(() => this.IsInLibrary, ref mIsInLibrary, value); }
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
        /// Checks whether the current audiobook is in the library and sets the IsInLibrary property.
        /// </summary>
        private void UpdateIsInLibrary()
        {
            IsInLibrary = Audiobook != null && AudiobookManager.Instance.AudiobookRepo.Items.Contains(Audiobook.Metadata.Path);
        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the FolderPath of the AudiobookFolder.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Destination = folderBrowserDialog.SelectedPath;
            }
        }

        public void AnalyzeFolder()
        {
            var tmpAudiobook = AudiobookManager.Instance.GetAudiobook(Destination);
            if (tmpAudiobook != null)
            {
                Audiobook = (Audiobook)tmpAudiobook.Clone();
            }
            else
            {
                Audiobook = AudiobookManager.Instance.LoadAudiobookFromFile(Destination);
            }

            Audiobook.Chapters = new ObservableCollection<Chapter>(AudiobookFolder.AnalyzeFolder(Audiobook.Metadata.Path));
            if (Audiobook.Metadata.Title.Equals(string.Empty))
            {
                Audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(Audiobook.Metadata.Path);
            }

            UpdateIsInLibrary();
        }

        private void SaveAudiobook()
        {
            AudiobookFolder.SaveAudiobookMetadata(Audiobook);

            if (!AudiobookManager.Instance.Audiobooks.Contains(Audiobook))
            {
                if (!Audiobook.Metadata.Title.Equals(string.Empty))
                {
                    AudiobookManager.Instance.UpdateAudioBook(Audiobook);
                } else
                {
                    throw new InvalidArgumentException("audiobook title is required");
                }                
            }

            UpdateIsInLibrary();
        }

        private bool CanSaveAudiobook()
        {
            return Audiobook.Chapters.Count > 0;
        }

        #endregion

    }
}
