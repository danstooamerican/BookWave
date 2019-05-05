using ATL;
using Commons.Models;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Commons.Logic
{
    public class FolderHandler : ObservableObject
    {
        private string mFolderPath;
        public string FolderPath
        {
            get { return mFolderPath; }
            set
            {
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.FolderPath, ref mFolderPath, value);
                } else
                {
                    Set<string>(() => this.FolderPath, ref mFolderPath, mFolderPath);
                }
            }
        }

        public FolderHandler()
        {
            FolderPath = string.Empty;
        }

        public ObservableCollection<Chapter> AnalyzeFolder()
        {
            var chapters = new ObservableCollection<Chapter>();
            string metadataDirectory = FolderPath + @"\metadata\";
            if (!Directory.Exists(metadataDirectory))
            {
                Directory.CreateDirectory(metadataDirectory);
            }
            List<string> metadataFiles = Directory.GetFiles(metadataDirectory, "*.nfo").ToList();
            var allowedExtensions = new[] { ".mp3", ".aac" };
            List<string> files = Directory
                    .GetFiles(FolderPath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            foreach (string file in files)
            {
                // creates new XML file or reads existing XML file to chapter
                string fileName = Path.GetFileNameWithoutExtension(file);
                var correspondingXMLPath = getFileFromFileName(metadataFiles, fileName);
                XDocument metadataXML;
                Chapter chapter;
                if (correspondingXMLPath != null)
                {
                    metadataXML = XDocument.Load(correspondingXMLPath);
                    chapter = new Chapter(metadataXML);
                } else
                {
                    chapter = new Chapter(new Track(file));
                    metadataXML = chapter.ToXML();
                    metadataXML.Save(metadataDirectory + fileName + ".nfo");
                }
                chapters.Add(chapter);
            }
            return chapters;
        }

        private static string getFileFromFileName(List<string> files, string search)
        {
            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).Equals(search))
                {
                    return file;
                }
            }
            return null;
        }
    }
}
