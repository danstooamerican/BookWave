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

        public static Audiobook XMLToAudiobook(string path)
        {
            XDocument metadataDoc = XDocument.Load(path);

            Audiobook audiobook = null;
            var audiobookXML = metadataDoc.Descendants("Audiobook");
            if (audiobookXML.Count() > 0)
            {
                audiobook = new Audiobook();
                audiobook.FromXML(audiobookXML.First());
            }

            return audiobook;
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
        /// <param name="element">Element the property is searched in</param>
        /// <param name="name">Name of the property</param>
        /// <returns>the element. Returns string.empty if it doesn't exist.</returns>
        public static string GetSingleElement(XElement element, string name)
        {
            var descendents = element.Descendants(name);
            if (descendents.Count() > 0)
            {
                return descendents.First().Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns a single element of an XML document or the defaultValue if nothing was found.
        /// </summary>
        /// <param name="element">Element the property is searched in</param>
        /// <param name="name">Name of the property</param>
        /// <param name="defaultValue">Value which is returned if no element was found.</param>
        /// <returns>the element. Returns string.empty if it doesn't exist.</returns>
        public static string GetSingleElement(XElement element, string name, string defaultValue)
        {
            var descendents = element.Descendants(name);
            if (descendents.Count() > 0)
            {
                return descendents.First().Value;
            }

            return defaultValue;
        }

    }
}
