using ATL;
using Commons.Models;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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
            if (Directory.Exists(FolderPath + @"\metadata"))
            {
                //TODO
                return null;
            } else
            {
                var allowedExtensions = new[] { ".mp3", ".aac" };
                List<string> files = Directory
                    .GetFiles(FolderPath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

                var chapters = new ObservableCollection<Chapter>();

                foreach (string file in files)
                {
                    Chapter chapter = new Chapter(new Track(file));
                    chapter.ToXML();
                    chapters.Add(chapter);
                }
                return chapters;
            }
        }
    }
}
