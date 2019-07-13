using Commons.Models;
using System.Collections.Generic;

namespace Commons.AudiobookManagemenet
{
    public abstract class LibraryScanner
    {
        public readonly string Title;

        public readonly string Description;

        public abstract void ScanLibrary(Library library);

        public abstract ICollection<Chapter> ScanAudiobookFolder(string path);
    }
}
