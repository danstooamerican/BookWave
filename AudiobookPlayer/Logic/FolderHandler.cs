using Commons.Models;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Commons.Logic
{
    public class FolderHandler : ObservableObject
    {
        private string mFolderPath;
        public string FolderPath
        {
            get { return mFolderPath; }
            set { Set<string>(() => this.FolderPath, ref mFolderPath, value); }
        }

        public FolderHandler()
        {
            FolderPath = string.Empty;
        }

        public ObservableCollection<Chapter> AnalyzeFolder()
        {
            return null; // TODO
        }

    }
}
