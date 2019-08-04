using System.Collections.Generic;
using System.IO;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    class AudiobooksTopScanner : LibraryScanner
    {
        public override string GetDescription()
        {
            return "Makker Scanner Description";
        }

        public override string GetIdentifier()
        {
            return "Other.Scanner.Makker";
        }

        public override string GetName()
        {
            return "Makker Scanner";
        }

        protected override ICollection<string> GetAudiobookFolders(string path)
        {
            return Directory.GetDirectories(path);
        }

        protected override void UpdateAudiobookInfo(Audiobook audiobook)
        {
            audiobook.Metadata.Title = "Makker " + audiobook.Metadata.Title;
        }
    }
}
