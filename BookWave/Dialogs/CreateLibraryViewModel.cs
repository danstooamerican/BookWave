using BookWave.Desktop.AudiobookManagement.Scanner;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
{
    public class CreateLibraryViewModel : ViewModelBase
    {

        #region Properties

        private string mDestination;
        public string Destination
        {
            get { return mDestination; }
            set
            {
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.Destination, ref mDestination, value.Trim());
                }
                else
                {
                    if (value != null && string.IsNullOrEmpty(value))
                    {
                        Set<string>(() => this.Destination, ref mDestination, string.Empty);
                    }
                }
            }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { Set<string>(() => this.Name, ref mName, value); }
        }

        public ICollection<LibraryScanner> LibraryScanners
        {
            get
            {
                return LibraryScannerFactory.GetAllScanners();
            }
        }

        private LibraryScanner mScanner;
        public LibraryScanner Scanner
        {
            get { return mScanner; }
            set { mScanner = value; }
        }

        private Library mLibrary;
        public Library Library
        {
            get { return mLibrary; }
            set { mLibrary = value; }
        }


        public delegate void LibraryCreated();

        public event LibraryCreated LibraryCreatedEvent;

        #endregion

        #region Commands

        public ICommand AddLibraryCommand { private set; get; }

        public ICommand SelectFolderCommand { private set; get; }

        #endregion

        #region Constructors

        public CreateLibraryViewModel()
        {
            AddLibraryCommand = new RelayCommand(AddLibrary, CanAddLibrary);
            SelectFolderCommand = new RelayCommand(SelectFolder);

            Scanner = LibraryScannerFactory.GetDefault();
        }

        #endregion

        #region Methods

        private void AddLibrary()
        {
            Library = LibraryManager.Instance.AddLibrary(Name, Destination, Scanner);
            LibraryCreatedEvent();          
        }

        private bool CanAddLibrary()
        {
            return !(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Destination));
        }

        /// <summary>
        /// Opens a FolderBrowserDialog and sets the LibraryPath.
        /// </summary>
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Destination = folderBrowserDialog.SelectedPath;

                if (string.IsNullOrEmpty(Name))
                {
                    Name = Path.GetFileName(Destination);
                }
            }
        }

        #endregion

    }
}
