using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

        #region Commands

        #endregion

        #region Constructors

        public AddPageViewModel()
        {
            Chapters = new ObservableCollection<Chapter>();

            Chapter c = new Chapter(null);
            c.Metadata.Title = "TestTitle";
            c.Metadata.Description = "Description";
            c.Metadata.Contributors.Authors.Add("Author1");
            c.Metadata.Contributors.Authors.Add("Author2");

            Chapters.Add(c);
        }

        #endregion

        #region Methods


        #endregion

    }
}
