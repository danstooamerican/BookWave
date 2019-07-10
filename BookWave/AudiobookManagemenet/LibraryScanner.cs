namespace Commons.AudiobookManagemenet
{
    public abstract class LibraryScanner
    {
        public readonly string Title;

        public readonly string Description;

        public abstract void ScanLibrary(Library library);
    }
}
