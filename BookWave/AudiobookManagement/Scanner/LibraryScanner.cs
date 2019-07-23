using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    public abstract class LibraryScanner
    {
        public readonly string Title;

        public readonly string Description;

        public static LibraryScanner GetInstance(string qualifiedName)
        {
            if (qualifiedName == null)
            {
                throw new ArgumentNullException("class qualifier cannot be null");
            }

            Type type = Type.GetType(qualifiedName);

            if (type == null)
            {
                throw new ArgumentException("class qualifier not found");
            }

            return (LibraryScanner)Activator.CreateInstance(type);
        }

        public abstract ICollection<Audiobook> ScanLibrary(string path);

        public abstract ICollection<Chapter> ScanAudiobookFolder(string path);

        protected ICollection<string> GetAllAudioFilesFrom(string path, SearchOption searchOption)
        {
            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_extensions").Split(',');

            List<string> files = Directory
                    .GetFiles(path, "*.*", searchOption)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            return files;
        }

    }
}
