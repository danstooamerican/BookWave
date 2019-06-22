using Commons.Models;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Takes an audiobook metadata file and creates an audiobook from it.
        /// If the file does not exist an empty Audiobook is returned.
        /// </summary>
        /// <param name="path">path to the xml file</param>
        /// <returns>a new audiobook</returns>
        public static Audiobook XMLToAudiobook(string path)
        {
            Audiobook audiobook = new Audiobook();

            if (File.Exists(path))
            {
                XDocument metadataDoc = XDocument.Load(path);

                var audiobookXML = metadataDoc.Descendants("Audiobook");
                if (audiobookXML.Count() > 0)
                {
                    audiobook.FromXML(audiobookXML.First());
                }
            }

            return audiobook;
        }

        /// <summary>
        /// Creates a new xml file at the given path with the toSave data.
        /// </summary>
        /// <param name="toSave">object to save</param>
        /// <param name="path">path to save the file at</param>
        public static void SaveToXML(XMLSaveObject toSave, string path)
        {
            var xml = new XDocument();
            xml.Add(toSave.ToXML());

            xml.Save(path);
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
