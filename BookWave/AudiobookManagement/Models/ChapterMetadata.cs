using ATL;
using BookWave.Desktop.Util;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
{
    /// <summary>
    /// Metadata for chapters which adds a TrackNumber.
    /// </summary>
    public class ChapterMetadata : Metadata, XMLSaveObject
    {
        #region Public Properties

        private string mTrackNumber;
        public string TrackNumber
        {
            get { return mTrackNumber; }
            set
            {
                int trackNumber;
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out trackNumber))
                {
                    Set<string>(() => this.TrackNumber, ref mTrackNumber, value);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Metadata object off a track.
        /// </summary>
        /// <param name="track">Track to reference.</param>
        public ChapterMetadata(Track track = null)
        {
            Title = string.Empty;
            Description = string.Empty;
            TrackNumber = string.Empty;

            if (track != null)
            {
                Title = track.Title;
                TrackNumber = track.TrackNumber.ToString();
                Description = track.Description;
            }
        }

        public new XElement ToXML()
        {
            XElement metadataXML = base.ToXML();

            if (!string.IsNullOrEmpty(TrackNumber))
            {
                metadataXML.Add(new XElement("TrackNumber", TrackNumber));
            }

            return metadataXML;
        }

        public new void FromXML(XElement xmlElement)
        {
            base.FromXML(xmlElement);

            TrackNumber = XMLHelper.GetSingleIntValue(xmlElement, "TrackNumber");
        }

        public override object Clone()
        {
            ChapterMetadata copy = new ChapterMetadata();

            copy.Title = Title;
            copy.Description = Description;
            copy.TrackNumber = TrackNumber;
            copy.MetadataPath = MetadataPath;

            return copy;
        }

        #endregion

    }
}
