using ATL;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    class AudiobooksTopScanner : LibraryScanner
    {
        public override ICollection<Chapter> ScanAudiobookFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException("audiobook folder does not exist");
            }

            ICollection<Chapter> chapters = new List<Chapter>();

            foreach (string file in GetAllAudioFilesFrom(path, SearchOption.AllDirectories))
            {
                Chapter chapter = AudiobookManager.Instance.CreateChapter(new Track(file));
                if (chapter != null)
                {
                    chapters.Add(chapter);
                }
            }

            return chapters;
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
