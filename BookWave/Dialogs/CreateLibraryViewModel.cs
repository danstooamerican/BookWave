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
using System.Windows.Input;

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
                    if (value != null && value.Equals(string.Empty))
                    {
                        Set<string>(() => this.Destination, ref mDestination, value);
                    }
                }
            }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
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

        #endregion

        #region Commands

        public ICommand AddLibraryCommand { private set; get; }

        #endregion

        #region Constructors

        public CreateLibraryViewModel()
        {
            AddLibraryCommand = new RelayCommand(AddLibrary);
            Scanner = LibraryScannerFactory.GetDefault();
        }

        #endregion

        #region Methods

        private void AddLibrary()
        {

        }

        #endregion

    }
}
