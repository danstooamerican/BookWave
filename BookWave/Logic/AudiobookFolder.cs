using ATL;
using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Commons.Logic
{
    /// <summary>
    /// Can analyze a folder and scan for audiofiles and metadata.
    /// </summary>
    public class AudiobookFolder
    {

        private AudiobookFolder()
        {

        }

        #region Methods

        /// <summary>
        /// Analyzes the folder of the AudiobookFolder.
        /// Searches for audiofiles and metadata and 
        /// creates metadata for the audiofiles with 
        /// no metadata.
        /// </summary>
        /// <returns>List of chapters in the folder.</returns>
        public static List<Chapter> AnalyzeFolder(string folderPath)
        {
            if (folderPath.Equals(string.Empty))
            {
                return new List<Chapter>();
            }
            var chapters = new List<Chapter>();
            string metadataDirectory = Path.Combine(folderPath, ConfigurationManager.AppSettings.Get("metadata_folder"));

            List<string> metadataFiles =
                Directory.Exists(metadataDirectory) 
                ? Directory.GetFiles(metadataDirectory, "*." + ConfigurationManager.AppSettings.Get("metadata_extensions")).ToList() 
                : new List<string>();

            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_extensions").Split(',');

            List<string> files = Directory
                    .GetFiles(folderPath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            foreach (string file in files)
            {
                // creates new XML file or reads existing XML file to chapter
                string fileName = Path.GetFileNameWithoutExtension(file);

                var correspondingXMLPath = GetFileFromFileName(metadataFiles, fileName);
                Chapter chapter;

                if (correspondingXMLPath != null)
                {
                    chapter = XMLHelper.XMLToChapter(correspondingXMLPath);
                } else
                {
                    chapter = new Chapter(new Track(file));
                }

                chapters.Add(chapter);
            }
            return chapters;
        }

        /// <summary>
        /// Loads all metadata files in the given directory and returns a list
        /// of chapters based on the metadata.
        /// </summary>
        /// <returns>List of chapters in the folder.</returns>
        public static List<Chapter> LoadAudiobookChapters(string metadataFolder)
        {
            List<Chapter> chapters = new List<Chapter>();

            if (!Directory.Exists(metadataFolder))
            {
                return chapters;
            }            

            List<string> metadataFiles = Directory.GetFiles(metadataFolder, "*." + ConfigurationManager.AppSettings.Get("metadata_extensions")).ToList();

            foreach (string file in metadataFiles)
            {
                Chapter chapter = XMLHelper.XMLToChapter(file);

                chapters.Add(chapter);
            }

            return chapters;
        }

        /// <summary>
        /// Saves metadata for each chapter in an audiobook
        /// </summary>
        /// <param name="chapters"></param>
        public static void SaveAudiobookMetadata(string folderPath, ObservableCollection<Chapter> chapters)
        {
            //TODO: change parameter to Audiobook

            string metadataDirectory = Path.Combine(folderPath, ConfigurationManager.AppSettings.Get("metadata_folder"));
            Directory.CreateDirectory(metadataDirectory);
            foreach (Chapter chapter in chapters)
            {
                foreach (AudioPath audioPath in chapter.AudioPaths)
                {
                    string fileName = Path.GetFileNameWithoutExtension(audioPath.Path);
                    XMLHelper.SaveChapterToXML(chapter, metadataDirectory + fileName + "."
                        + ConfigurationManager.AppSettings.Get("metadata_extensions"));
                }
            }
        }


        /// <summary>
        /// Searches in a List of file paths for a specific file name.
        /// </summary>
        /// <param name="files">is the List of file paths.</param>
        /// <param name="search">is the file name.</param>
        /// <returns></returns>
        private static string GetFileFromFileName(List<string> files, string search)
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

        #endregion
    }
}
