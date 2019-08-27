using BookWave.Desktop.Models.AudiobookManagement;
using Xunit;

namespace BookWave.Tests
{
    public class LibraryManagerTest
    {

        [Fact]
        public void LibraryManager_Creation()
        {
            Assert.NotNull(LibraryManager.Instance);
            Assert.NotNull(LibraryManager.Instance.Libraries);
            Assert.NotEqual(string.Empty, LibraryManager.Instance.MetadataPath);
        }

    }
}
