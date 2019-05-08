using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class AddPageViewModel : ViewModelBase
    {

        #region Public Properties

        private ObservableCollection<Chapter> mChapters;

        public ObservableCollection<Chapter> Chapters
        {
            get { return mChapters; }
            set { Set<ObservableCollection<Chapter>>(() => this.Chapters, ref mChapters, value); }
        }

        public FolderHandler FolderHandler { get; set; }

        #endregion

        #region Commands
        public ICommand SelectFolderCommand { private set; get; }

        public ICommand AnalyzeFolderCommand { private set; get; }

        #endregion

        #region Constructors
        
        public AddPageViewModel()
        {
            FolderHandler = new FolderHandler();
            FolderHandler.FolderPathClearedEvent += ClearChapters;

            SelectFolderCommand = new RelayCommand(SelectFolder);
            AnalyzeFolderCommand = new RelayCommand(AnalyzeFolder);

            Chapters = new ObservableCollection<Chapter>();
        }

        #endregion

        #region Methods

        private void ClearChapters()
        {
            Chapters.Clear();
        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the FolderPath of the FolderHandler.
        /// </summary>
        public void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderHandler.FolderPath = folderBrowserDialog.SelectedPath;                
            }
        }

        /// <summary>
        /// Analyzes the selected folder and updates the chapter list.
        /// </summary>
        public void AnalyzeFolder()
        {
            Chapters = new ObservableCollection<Chapter>(FolderHandler.AnalyzeFolder());
        }

        #endregion

    }
}
