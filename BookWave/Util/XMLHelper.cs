using Commons.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Commons.Util
{
    /// <summary>
    /// Contains static methods that analyze and write XML files.
    /// </summary>
    public class XMLHelper
    {
        /// <summary>
        /// Creates a chapter from an XML file.
        /// </summary>
        /// <param name="path">is the path to the XML file.</param>
        /// <returns></returns>
        public static Chapter XMLToChapter(string path)
        {
            XDocument metadataDoc = XDocument.Load(path);

            Chapter chapter = null;
            var chapterXML = metadataDoc.Descendants("Chapter");
            if (chapterXML.Count() > 0)
            {
                chapter = new Chapter();
                chapter.FromXML(chapterXML.First());
            }

            return chapter;
        }

        public static void SaveToXML(XMLSaveObject toSave, string path)
        {
            var metadataXML = new XDocument();
            metadataXML.Add(toSave.ToXML());

            metadataXML.Save(path);
        }

        /// <summary>
        /// Returns a single element of an XML document.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSingleElement(XElement element, string name)
        {
            var descendents = element.Descendants(name);
            if (descendents.Count() > 0)
            {
                return descendents.First().Value;
            }
            return string.Empty;
        }

    }
}
