using Commons.Exceptions;
using System.IO;

namespace Commons.Models
{
    /// <summary>
    /// Metadata for audiobooks which adds a Genre Option.
    /// </summary>
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

        private string mPath;
        /// <summary>
        /// Path to the audiobook folder.
        /// </summary>
        public string Path
        {
            get { return mPath; }
            set
            {
                if (File.Exists(value))
                {
                    Set<string>(() => this.Path, ref mPath, value);
                }
                else
                {
                    throw new InvalidArgumentException(value, "is not a valid path.");
                }
            }
        }

        private string mGenre;
        /// <summary>
        /// Genre of the audiobook.
        /// </summary>
        public string Genre
        {
            get { return mGenre; }
            set { mGenre = value; }
        }


        #endregion

    }
}
