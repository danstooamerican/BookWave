using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using ATL;
using Commons.Models;

namespace Commons.AudiobookManagemenet.Scanner
{
    public class AudiobooksTopScanner : LibraryScanner
    {
        public override ICollection<Chapter> ScanAudiobookFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException("audiobook folder does not exist");
            }

            ICollection<Chapter> chapters = new List<Chapter>();

            foreach (string file in GetAllAudioFilesFrom(path))
            {
                Chapter chapter = new Chapter(new Track(file));
                if (chapter != null)
                {
                    chapters.Add(chapter);
                }
            }

            return chapters;
        }

        private List<string> GetAllAudioFilesFrom(string path)
        {
            var allowedExtensions = ConfigurationManager.AppSettings.Get("allowed_extensions").Split(',');

            List<string> files = Directory
                    .GetFiles(path)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

            foreach (string dir in Directory.GetDirectories(path))
            {
                files.AddRange(GetAllAudioFilesFrom(dir));
            }

            return files;
        }

        public override ICollection<Audiobook> ScanLibrary(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException("library folder does not exist");
            }

            ICollection<Audiobook> audiobooks = new List<Audiobook>();

            foreach (string dir in Directory.GetDirectories(path))
            {
                Audiobook audiobook = AudiobookManager.Instance.CreateAudiobook();

                audiobook.SetChapters(ScanAudiobookFolder(dir));
                audiobook.Metadata.Title = Path.GetFileNameWithoutExtension(dir);
                audiobook.Metadata.Path = dir;

                audiobooks.Add(audiobook);
            }

            return audiobooks;
        }

    }
}
