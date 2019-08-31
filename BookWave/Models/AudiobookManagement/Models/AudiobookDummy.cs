using System.Linq;

namespace BookWave.Desktop.Models.AudiobookManagement
{
    /// <summary>
    /// Null Object for Audiobooks
    /// </summary>
    public class AudiobookDummy : Audiobook
    {
        public static readonly int DummyId = -1;

        // initialize with invalid id so it can't be found
        public AudiobookDummy() : base(DummyId)
        {
            Metadata = new AudiobookMetadataDummy();

            if (LibraryManager.Instance.GetLibraries().Count > 0)
            {
                Library = LibraryManager.Instance.GetLibraries().ElementAt(0);
            }
        }
    }

    public class AudiobookMetadataDummy : AudiobookMetadata
    {

        public override bool PathNotValid { get { return false; } }

        public override string Path { get { return "none"; } }

        public AudiobookMetadataDummy() : base()
        {

        }

    }

}
