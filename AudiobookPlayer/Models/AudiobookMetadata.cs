namespace Commons.Models
{
    /// <summary>
    /// Metadata for audiobooks which adds a Genre Option.
    /// </summary>
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

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
