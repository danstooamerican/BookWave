using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
{
    public abstract class Metadata : ObservableObject, XMLSaveObject, ICloneable
    {

        #region Public Properties

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { Set<string>(() => this.Title, ref mTitle, value); }
        }

        private string mDescription;
        /// <summary>
        /// Description of the file.
        /// </summary>
        public string Description
        {
            get { return mDescription; }
            set { Set<string>(() => this.Description, ref mDescription, value); }
        }

        private string mMetadataPath;
        public string MetadataPath
        {
            get { return mMetadataPath; }
            set { mMetadataPath = value; }
        }

        #endregion

        #region Constructor

        public Metadata()
        {
            Title = string.Empty;
            Description = string.Empty;
            MetadataPath = string.Empty;
        }

        #endregion

        #region Methods
        public XElement ToXML()
        {
            XElement metadataXML = new XElement("Metadata");

            if (!Title.Equals(string.Empty)) //TODO maybe != null?
            {
                metadataXML.Add(new XElement("Title", Title));
            }

            if (!Description.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Description", Description));
            }

            return metadataXML;
        }
        public void FromXML(XElement xmlElement)
        {
            Title = XMLHelper.GetSingleValue(xmlElement, "Title");

            Description = XMLHelper.GetSingleValue(xmlElement, "Description");
        }

        public abstract object Clone();

        #endregion
    }
}
