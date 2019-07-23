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

        private int mTrackNumber;
        public int TrackNumber
        {
            get { return mTrackNumber; }
            set { Set<int>(() => this.TrackNumber, ref mTrackNumber, value); }
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
            TrackNumber = -1;

            if (track != null)
            {
                Title = track.Title;
                TrackNumber = track.TrackNumber;
                Description = track.Description;
            }
        }

        public new XElement ToXML()
        {
            XElement metadataXML = base.ToXML();

            if (TrackNumber != 0)
            {
                metadataXML.Add(new XElement("TrackNumber", TrackNumber));
            }

            return metadataXML;
        }

        public new void FromXML(XElement xmlElement)
        {
            base.FromXML(xmlElement);

            // TODO regex move to GetSingleElement
            string strTrackNumber = XMLHelper.GetSingleValue(xmlElement, "TrackNumber");
            if (Regex.IsMatch(strTrackNumber, "[0-9]+"))
            {
                TrackNumber = int.Parse(strTrackNumber);
            }
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
