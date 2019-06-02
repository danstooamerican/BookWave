﻿using Commons.Logic;
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

        private Audiobook mAudiobook;

        public Audiobook Audiobook
        {
            get { return mAudiobook; }
            set { Set<Audiobook>(() => this.Audiobook, ref mAudiobook, value); }
        }


        public FolderHandler FolderHandler { get; set; }

        #endregion

        #region Commands
        public ICommand SelectFolderCommand { private set; get; }

        public ICommand SaveAudiobookCommand { private set; get; }

        #endregion

        #region Constructors
        
        public AddPageViewModel()
        {
            FolderHandler = new FolderHandler();
            Audiobook = new Audiobook();
            FolderHandler.FolderPathClearedEvent += Audiobook.ClearChapters;
            FolderHandler.FolderPathSetEvent += AnalyzeFolder;

            SelectFolderCommand = new RelayCommand(SelectFolder);
            SaveAudiobookCommand = new RelayCommand(SaveAudiobook, CanSaveAudiobook);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the FolderPath of the FolderHandler.
        /// </summary>
        private void SelectFolder()
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
        private void AnalyzeFolder()
        {
            Audiobook.Chapters =
                new ObservableCollection<Chapter>(FolderHandler.AnalyzeFolder());
        }


        private void SaveAudiobook()
        {
            FolderHandler.SaveAudiobook(Audiobook.Chapters);
            // TODO add audiobook to audiobookmanager
        }

        private bool CanSaveAudiobook()
        {
            return Audiobook.Chapters.Count > 0;
        }

        #endregion

    }
}
