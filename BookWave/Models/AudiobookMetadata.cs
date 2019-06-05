using Commons.Exceptions;
using Commons.Util;
using GalaSoft.MvvmLight;
using System.IO;
using System.Xml.Linq;

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
                if (Directory.Exists(value))
                {
                    Set<string>(() => this.Path, ref mPath, value);
                }
                else
                {
                    if (value != null && value.Equals(string.Empty))
                    {
                        Set<string>(() => this.Path, ref mPath, value);
                    }
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

        private Contributors mContributors;
        /// <summary>
        /// All contributors.
        /// </summary>
        public Contributors Contributors
        {
            get { return mContributors; }
            set { Set<Contributors>(() => this.Contributors, ref mContributors, value); }
        }

        #endregion

        #region Constructor
        public AudiobookMetadata() : base()
        {
            Path = string.Empty;
            Genre = string.Empty;
            Contributors = new Contributors();
        }

        public new XElement ToXML()
        {
            XElement metadataXML = base.ToXML();

            if (!Path.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Path", Path));
            }

            if (!Genre.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Genre", Genre));
            }

            if (Contributors != null)
            {
                XElement contributorsXML = Contributors.ToXML();
                if (contributorsXML != null)
                {
                    metadataXML.Add();
                }
            }

            return metadataXML;
        }

        public new void FromXML(XElement xmlElement)
        {
            base.FromXML(xmlElement);

            Path = XMLHelper.GetSingleElement(xmlElement, "Path");
            Genre = XMLHelper.GetSingleElement(xmlElement, "Genre");

            Contributors.FromXML(xmlElement);
            
        }
        #endregion

    }
}
