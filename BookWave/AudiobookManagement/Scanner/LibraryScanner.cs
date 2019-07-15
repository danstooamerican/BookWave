using System;
using System.Collections.Generic;

namespace Commons.AudiobookManagement.Scanner
{
    public abstract class LibraryScanner
    {
        public readonly string Title;

        public readonly string Description;

        public static LibraryScanner GetInstance(string qualifiedName)
        {
            if (qualifiedName == null)
            {
                throw new ArgumentException("class qualifier cannot be null");
            }

            Type type = Type.GetType(qualifiedName);

            return (LibraryScanner)Activator.CreateInstance(type);
        }

        public abstract ICollection<Audiobook> ScanLibrary(string path);

        public abstract ICollection<Chapter> ScanAudiobookFolder(string path);

    }
}
