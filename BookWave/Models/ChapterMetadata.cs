using System.Text.RegularExpressions;
using System.Xml.Linq;
using ATL;
using Commons.Util;
using GalaSoft.MvvmLight;

namespace Commons.Models
{
    /// <summary>
    /// Basis metadata for audio files.
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
            ReleaseYear = -1;

            if (track != null)
            {
                Title = track.Title;
                TrackNumber = track.TrackNumber;
                Description = track.Description;
                ReleaseYear = track.Year;
            }
        }
        
        public override XElement ToXML()
        {
            XElement metadataXML = new XElement("Metadata");

            if (!Title.Equals(string.Empty)) //TODO maybe != null?
            {
                metadataXML.Add(new XElement("Title", Title));
            }
            if (TrackNumber != 0)
            {
                metadataXML.Add(new XElement("TrackNumber", TrackNumber));
            }
            if (!Description.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Description", Description));
            }
            if (ReleaseYear != 0) //TODO what is standard value?
            {
                metadataXML.Add(new XElement("ReleaseYear", ReleaseYear));
            }

            return metadataXML;
        }

        public override void FromXML(XElement xmlElement)
        {
            Title = XMLHelper.GetSingleElement(xmlElement, "Title");

            Description = XMLHelper.GetSingleElement(xmlElement, "Description");

            // TODO regex move to GetSingleElement
            string strTrackNumber = XMLHelper.GetSingleElement(xmlElement, "TrackNumber");
            if (Regex.IsMatch(strTrackNumber, "[0-9]+"))
            {
                TrackNumber = int.Parse(strTrackNumber);
            }
            string strReleaseYear = XMLHelper.GetSingleElement(xmlElement, "ReleaseYear");
            if (Regex.IsMatch(strReleaseYear, "[0-9]+"))
            {
                ReleaseYear = int.Parse(strReleaseYear);
            }
        }

        #endregion

    }
}
