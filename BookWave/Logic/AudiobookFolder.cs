using ATL;
using Commons.AudiobookManagemenet;
using Commons.Models;
using Commons.Util;
using System.Collections.Generic;
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

        #region Methods

        /// <summary>
        /// Searches for audiofiles and metadata and 
        /// gets metadata from the audio files for audio files with no metadata.
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

            List<string> metadataFiles;
            
            if (Directory.Exists(metadataDirectory))
            {
                metadataFiles = Directory.GetFiles(metadataDirectory, "*." + ConfigurationManager.AppSettings.Get("metadata_extensions")).ToList();
            } else
            {
                metadataFiles = new List<string>(); 
            }

            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_extensions").Split(',');

            List<string> files = Directory
                    .GetFiles(folderPath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            // creates new XML file or reads existing XML file to chapter
            foreach (string file in files)
            {                
                string fileName = Path.GetFileNameWithoutExtension(file);

                var chapterMetadataPath = GetMetadataFilePath(metadataFiles, fileName);
                Chapter chapter;

                if (chapterMetadataPath != null)
                {
                    chapter = XMLHelper.XMLToChapter(chapterMetadataPath);
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
                if (chapter != null)
                {
                    chapters.Add(chapter);
                }
            }

            return chapters;
        }

        /// <summary>
        /// Saves metadata for each chapter in an audiobook
        /// </summary>
        /// <param name="chapters"></param>
        public static void SaveAudiobookMetadata(Audiobook audiobook)
        {
            string metadataDirectory = Path.Combine(audiobook.Metadata.Path, ConfigurationManager.AppSettings.Get("metadata_folder"));
            Directory.CreateDirectory(metadataDirectory);
            foreach (Chapter chapter in audiobook.Chapters)
            {
                string fileName = Path.GetFileNameWithoutExtension(chapter.AudioPath.Path);
                XMLHelper.SaveToXML(chapter, Path.Combine(metadataDirectory, fileName + "."
                    + ConfigurationManager.AppSettings.Get("metadata_extensions")));
            }
            XMLHelper.SaveToXML(audiobook, Path.Combine(metadataDirectory, "audiobook."
                        + ConfigurationManager.AppSettings.Get("metadata_extensions")));
        }


        /// <summary>
        /// Searches in a List of file paths for a specific file name.
        /// </summary>
        /// <param name="files">List of all file paths.</param>
        /// <param name="search">File name.</param>
        /// <returns>Path of the metadata file or null if nothing was found.</returns>
        private static string GetMetadataFilePath(List<string> files, string search)
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
