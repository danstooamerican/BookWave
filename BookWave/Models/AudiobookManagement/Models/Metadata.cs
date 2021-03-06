﻿using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Xml.Linq;

namespace BookWave.Desktop.Models.AudiobookManagement
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

        /// <summary>
        /// For audiobooks this property represents the full path to the metadata folder.
        /// For chapters this property only contains the chapter metadata file name with extension.
        /// </summary>
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

            if (!string.IsNullOrEmpty(Title))
            {
                metadataXML.Add(new XElement("Title", Title));
            }

            if (!string.IsNullOrEmpty(Description))
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
