using BookWave.Desktop.Util;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
{
    /// <summary>
    /// Metadata for audiobooks which adds a Genre, Contributors, ReleaseYear and a CoverPath.
    /// </summary>
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

        public static readonly string StandardCover = @"/BookWave.Styles;component/Resources/Player/sampleCover.png";

        private string mPath;
        /// <summary>
        /// Path to the audiobook folder.
        /// </summary>
        public string Path
        {
            get { return mPath; }
            set
            {
                Set<string>(() => this.Path, ref mPath, value.Trim());
                RaisePropertyChanged(nameof(PathNotValid));
            }
        }

        public bool PathNotValid
        {
            get { return !Directory.Exists(Path); }
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

        private string mReleaseYear;
        /// <summary>
        /// Release Year.
        /// </summary>
        public string ReleaseYear
        {
            get { return mReleaseYear; }
            set {
                int year;
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out year))
                {
                    Set<string>(() => this.ReleaseYear, ref mReleaseYear, value);
                }        
            }
        }

        /// <summary>
        /// The path for the cover image.
        /// </summary>
        public string CoverPath
        {
            get
            {
                string coverPath = System.IO.Path.Combine(MetadataPath, "cover.jpg");

                return File.Exists(coverPath) ? coverPath : StandardCover;
            }
        }

        /// <summary>
        /// Cover Image as an ImageSource to be bound to in the view so the file is not locked.
        /// </summary>
        public ImageSource CoverSource
        {
            get
            {
                if (HasCoverPath)
                {
                    using (var fs = new FileStream(CoverPath, FileMode.Open, FileAccess.Read))
                    {
                        return BitmapFrame.Create(
                            fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
                else
                {
                    var uriSource = new Uri(CoverPath, UriKind.Relative);
                    return new BitmapImage(uriSource);
                }
            }
        }

        public bool HasCoverPath
        {
            get
            {
                return CoverPath.Equals(StandardCover) ? false : File.Exists(CoverPath);
            }
        }

        #endregion

        #region Constructor
        public AudiobookMetadata() : base()
        {
            Path = string.Empty;
            MetadataPath = string.Empty;
            Genre = string.Empty;
            Contributors = new Contributors();
            ReleaseYear = string.Empty;
        }

        #endregion

        #region Methods

        public void RaiseCoverChanged()
        {
            RaisePropertyChanged(nameof(CoverPath));
            RaisePropertyChanged(nameof(CoverSource));
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
                    metadataXML.Add(contributorsXML);
                }
            }

            if (!ReleaseYear.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("ReleaseYear", ReleaseYear));
            }

            if (!CoverPath.Equals(StandardCover))
            {
                metadataXML.Add(new XElement("Cover", CoverPath));
            }

            return metadataXML;
        }

        public new void FromXML(XElement xmlElement)
        {
            base.FromXML(xmlElement);

            Path = XMLHelper.GetSingleValue(xmlElement, "Path");
            Genre = XMLHelper.GetSingleValue(xmlElement, "Genre");

            Contributors.FromXML(xmlElement);

            ReleaseYear = XMLHelper.GetSingleValue(xmlElement, "ReleaseYear");
        }

        public override object Clone()
        {
            AudiobookMetadata copy = new AudiobookMetadata();

            copy.Title = Title;
            copy.Description = Description;
            copy.Genre = Genre;
            copy.Path = Path;
            copy.MetadataPath = MetadataPath;
            copy.ReleaseYear = ReleaseYear;
            copy.Contributors = (Contributors)Contributors.Clone();

            return copy;
        }

        #endregion

    }
}
