using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookWave.Desktop.Util
{
    /// <summary>
    /// Contains static methods that analyze and write XML files.
    /// </summary>
    public class XMLHelper
    {
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

        public static XElement GetFirstXElement(XDocument doc, string name)
        {
            var decendents = doc.Descendants(name);

            if (decendents.Count() > 0)
            {
                return decendents.First();
            }

            return null;
        }

        /// <summary>
        /// Returns a single element of an XML document.
        /// </summary>
        /// <param name="element">Element the property is searched in</param>
        /// <param name="name">Name of the property</param>
        /// <returns>the element. Returns string.empty if it doesn't exist.</returns>
        public static string GetSingleValue(XElement element, string name)
        {
            var descendents = element.Descendants(name);
            if (descendents.Count() > 0)
            {
                return descendents.First().Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns a single element of an XML document if it is an integer.
        /// </summary>
        /// <param name="element">Element the property is searched in</param>
        /// <param name="name">Name of the property</param>
        /// <returns>the element. Returns string.empty if it doesn't exist or isn't an integer.</returns>
        public static string GetSingleIntValue(XElement element, string name)
        {
            string value = GetSingleValue(element, name);
            int intValue;
            if (string.IsNullOrEmpty(value) || int.TryParse(value, out intValue))
            {
                return value;
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
        public static string GetSingleValue(XElement element, string name, string defaultValue)
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
