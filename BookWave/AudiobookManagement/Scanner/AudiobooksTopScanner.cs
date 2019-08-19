using System.Collections.Generic;
using System.IO;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    class AudiobooksTopScanner : LibraryScanner
    {
        public override string GetDescription()
        {
            return "Default Scanner";
        }

        public override string GetIdentifier()
        {
            return "BookWave.Scanner.AudiobooksTopScanner";
        }

        public override string GetName()
        {
            return "Audiobook Top Scanner";
        }

        protected override ICollection<string> GetAudiobookFolders(string path)
        {
            return Directory.GetDirectories(path);
        }

        protected override void UpdateAudiobookInfo(Audiobook audiobook)
        {
            // NOP
        }
    }
}
