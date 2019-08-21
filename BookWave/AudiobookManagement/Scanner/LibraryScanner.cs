using ATL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    public abstract class LibraryScanner
    {
        public string Name { get { return GetName(); } }

        public string Description { get { return GetDescription(); } }

        /// <summary>
        /// The display name of the scanner to be displayed in the ui.
        /// </summary>
        /// <returns>name of the scanner</returns>
        public abstract string GetName();

        /// <summary>
        /// Description of the scanner to be displayed in the ui.
        /// </summary>
        /// <returns>description of the scanner</returns>
        public abstract string GetDescription();

        /// <summary>
        /// Unique identifier of the scanner. This identifier is saved with the library
        /// metadata.
        /// </summary>
        /// <returns>identifier of the scanner</returns>
        public abstract string GetIdentifier();

        /// <summary>
        /// Scans a library folder and returns a list of all audiobooks found.
        /// </summary>
        /// <param name="libraryPath">path of the library folder</param>
        /// <returns>list of all found audiobooks</returns>
        public ICollection<Audiobook> ScanLibrary(string libraryPath)
        {
            if (!Directory.Exists(libraryPath))
            {
                throw new FileNotFoundException("library folder does not exist");
            }

            ICollection<string> audiobookFolders = GetAudiobookFolders(libraryPath);

            ICollection<Audiobook> audiobooks = new List<Audiobook>();
            foreach (string path in audiobookFolders)
            {
                ICollection<Chapter> chapters = ScanAudiobookFolder(path);

                Audiobook audiobook = AudiobookManager.Instance.CreateAudiobook();
                audiobook.SetChapters(chapters);                
                audiobook.Metadata.Path = path;
                audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(audiobook.Metadata.Path);

                UpdateAudiobookInfo(audiobook);

                audiobooks.Add(audiobook);
            }

            return audiobooks;
        }

        /// <summary>
        /// Change relevant information of the audiobook. Path and title are set
        /// by default. Title is the name of the audio file without extension.
        /// </summary>
        /// <param name="audiobook">audiobook to be modified</param>
        protected abstract void UpdateAudiobookInfo(Audiobook audiobook);

        /// <summary>
        /// Finds all folders in a library folder which contain an audiobook for
        /// further analysis.
        /// </summary>
        /// <param name="path">library folder path</param>
        /// <returns>list of paths to all audiobook folders</returns>
        protected abstract ICollection<string> GetAudiobookFolders(string path);

        /// <summary>
        /// Scans an audiobook folder for all audio files and creates chapters for them.
        /// This implementation assumes that audio files are all in the top folder level.
        /// </summary>
        /// <param name="folderPath">path to an audiobook folder</param>
        /// <returns>list of chapters contained in the audiobook folder</returns>
        public ICollection<Chapter> ScanAudiobookFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new FileNotFoundException("audiobook folder does not exist");
            }

            ICollection<Chapter> chapters = new List<Chapter>();

            foreach (string file in GetAllAudioFilesFrom(folderPath, SearchOption.AllDirectories))
            {
                Chapter chapter = AudiobookManager.Instance.CreateChapter(new Track(file));
                if (chapter != null)
                {
                    chapters.Add(chapter);
                }
            }

            return chapters;
        }        

        /// <summary>
        /// Retrieves all audio files with allowed extensions in the given folder.
        /// </summary>
        /// <param name="path">path to a folder</param>
        /// <param name="searchOption">search algorithm for files</param>
        /// <returns>list of paths to all audio files in the folder</returns>
        protected ICollection<string> GetAllAudioFilesFrom(string path, SearchOption searchOption)
        {
            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_audio_extensions").Split(',');

            List<string> files = Directory
                    .GetFiles(path, "*.*", searchOption)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            return files;
        }

        /// <summary>
        /// Returns the name of the scanner.
        /// </summary>
        /// <returns>name of the scanner</returns>
        public override string ToString()
        {
            return GetName();
        }

    }
}
