using ATL;
using Commons.Models;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Commons.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class FolderHandler : ObservableObject
    {
        private string mFolderPath;
        public string FolderPath
        {
            get { return mFolderPath; }
            set
            {
                if (value != null && (Directory.Exists(value) || value.Equals(string.Empty)))
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
            mFolderPath = string.Empty;
        }

        public ObservableCollection<Chapter> AnalyzeFolder()
        {
            var chapters = new ObservableCollection<Chapter>();
            string metadataDirectory = FolderPath + ConfigurationManager.AppSettings.Get("metadata_folder");

            Directory.CreateDirectory(metadataDirectory);

            List<string> metadataFiles = Directory.GetFiles(metadataDirectory, "*." + ConfigurationManager.AppSettings.Get("metadata_extensions")).ToList();

            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_extensions").Split(',');

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

                    //TODO: dont save before user finishes editing
                    metadataXML.Save(metadataDirectory + fileName + "." + ConfigurationManager.AppSettings.Get("metadata_extensions"));
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
