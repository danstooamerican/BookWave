﻿using Commons.Logic;
using Commons.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Commons.ViewModel
{
    public class BrowseViewModel : ViewModelBase
    {
        #region Properties

        private Audiobook mSelected;
        public Audiobook Selected
        {
            get { return mSelected; }
            set { Set<Audiobook>(() => this.Selected, ref mSelected, value); }
        }

        private ICollectionView mAudiobooks;
        public ICollectionView Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ICollectionView>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        #endregion

        #region Commands

        public ICommand EditSelectedCommand { private set; get; }

        #endregion

        #region Constructor

        public BrowseViewModel()
        {
            EditSelectedCommand = new RelayCommand<Audiobook>((a) => EditSelected(a));

            var t = Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Audiobooks = CollectionViewSource.GetDefaultView(AudiobookManager.Instance.Audiobooks.Values);
                    Audiobooks.SortDescriptions.Add(new SortDescription("Metadata.Title", ListSortDirection.Ascending));
                }));
            });            
        }

        #endregion

        #region Methods

        private void EditSelected(Audiobook audiobook)
        {
            ViewModelLocator.Instance.MainViewModel.SwitchToEditLibraryPage(audiobook);
        }

        #endregion

    }
}
