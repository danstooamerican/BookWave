using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml.Linq;

namespace Commons.Models
{
    /// <summary>
    /// Includes lists of people who contribute to an audiobook.
    /// </summary>
    public class Contributors : ObservableObject, XMLSaveObject, ICloneable
    {

        #region Public Properties

        private List<string> mAuthors;
        /// <summary>
        /// List of authors.
        /// </summary>
        public List<string> Authors
        {
            get { return mAuthors; }
            set { Set<List<string>>(() => this.Authors, ref mAuthors, value); }
        }

        private List<string> mReaders;
        /// <summary>
        /// List of readers.
        /// </summary>
        public List<string>  Readers
        {
            get { return mReaders; }
            set { Set<List<string>>(() => this.Readers, ref mReaders, value); }
        }

        /// <summary>
        /// String representation of all authors. If this Property is set
        /// in the right format (name1, name2, ..., name n) then the current 
        /// author list is cleared and new elements are added.
        /// </summary>
        public string AuthorString {
            get
            {
                return BuildNameListString(Authors);
            }
            set
            {
                Authors = ParseNameList(value);
            }
        }

        public string ReaderString
        {
            get
            {
                return BuildNameListString(Readers);
            }
            set
            {
                Readers = ParseNameList(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Contributors class with empty lists of
        /// authors and readers.
        /// </summary>
        public Contributors()
        {
            Authors = new List<string>();
            Readers = new List<string>();
        }

        #endregion

        #region Helper Methods

        //TODO: move this logic to the view model or a converter

        /// <summary>
        /// Turns a string which is seperated by the DELIMITER into a
        /// List of strings.
        /// </summary>
        /// <param name="value">string with a DELIMITER seperated list of values.</param>
        /// <returns></returns>
        private List<string> ParseNameList(string value)
        {
            string[] split = value.Split(char.Parse(ConfigurationManager.AppSettings.Get("text_delimiter")));

            List<string> list = new List<string>();
            foreach (string name in split)
            {
                if (!name.Trim().Equals(string.Empty)
                    && !list.Contains(name.Trim()))
                {
                    list.Add(name.Trim());
                }
            }

            return list;
        }

        /// <summary>
        /// Creates a string representation of a List with strings.
        /// The values of DELIMITER is used to seperate the values.
        /// </summary>
        /// <param name="list">List with the elements to be included in the string.</param>
        /// <returns>string seperated by DELIMITER with the values from the list.</returns>
        private string BuildNameListString(List<string> list)
        {
            string DELIM_TEXT = ConfigurationManager.AppSettings.Get("text_delimiter") + " ";

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item).Append(DELIM_TEXT);
            }

            sb.Length = Math.Max(sb.Length - DELIM_TEXT.Length, 0);

            return sb.ToString();
        }

        public XElement ToXML()
        {
            var contributorsXML = new XElement("Contributors");

            if (Authors.Count > 0)
            {
                var authorsXML = new XElement("Authors");
                foreach (string author in Authors)
                {
                    var authorXML = new XElement("Author");
                    authorXML.Add(author);
                    authorsXML.Add(authorXML);
                }
                contributorsXML.Add(authorsXML);
            }

            if (Readers.Count > 0)
            {
                var readersXML = new XElement("Readers");
                foreach (string reader in Readers)
                {
                    var readerXML = new XElement("Reader");
                    readerXML.Add(reader);
                    readersXML.Add(readerXML);
                }
                contributorsXML.Add(readersXML);
            }
            
            if (Authors.Count > 0 || Readers.Count > 0)
            {
                return contributorsXML;
            }
            return null;
        }

        public void FromXML(XElement xmlElement)
        {
            foreach (var element in xmlElement.Descendants("Author"))
            {
                Authors.Add((string) element.Value);
            }
            foreach (var element in xmlElement.Descendants("Reader"))
            {
                Readers.Add((string) element.Value);
            }
        }

        public object Clone()
        {
            Contributors copy = new Contributors();

            List<string> authorCopy = new List<string>();
            foreach (string author in Authors)
            {
                authorCopy.Add(author);
            }
            copy.Authors = authorCopy;

            List<string> readersCopy = new List<string>();
            foreach (string reader in Readers)
            {
                readersCopy.Add(reader);
            }
            copy.Readers = readersCopy;

            return copy;
        }

        #endregion

    }
}
