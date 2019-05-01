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
            set { mChapters = value; }
        }

        private FolderHandler mFolderHandler;
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
            SelectFolderCommand = new RelayCommand(SelectFolder);
            AnalyzeFolderCommand = new RelayCommand(AnalyzeFolder);
            Chapters = new ObservableCollection<Chapter>();

            Chapter c = new Chapter(null);
            c.Metadata.Title = "TestTitle";
            c.Metadata.Description = "Description";
            c.Metadata.Contributors.Authors.Add("Author1");
            c.Metadata.Contributors.Authors.Add("Author2");

            Chapters.Add(c);

            Chapter c1 = new Chapter(null);
            c1.Metadata.Title = "TestTitle2";
            c1.Metadata.Description = "Description2";
            c1.Metadata.Contributors.Authors.Add("Author11");
            c1.Metadata.Contributors.Authors.Add("Author22");

            Chapters.Add(c1);
        }

        #endregion

        #region Methods

        public void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderHandler.FolderPath = folderBrowserDialog.SelectedPath;
            }
        }

        public void AnalyzeFolder()
        {
            Chapters = FolderHandler.AnalyzeFolder();
        }

        #endregion

    }
}
