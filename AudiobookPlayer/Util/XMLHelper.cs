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
        public static IEnumerable<AudioPath> AudioPaths { get; private set; }

        /// <summary>
        /// Creates a chapter from an XML file.
        /// </summary>
        /// <param name="path">is the path to the XML file.</param>
        /// <returns></returns>
        public static Chapter XMLToChapter(string path)
        {
            XDocument metadataDoc = XDocument.Load(path);

            IEnumerable<AudioPath> audioPath = from c in metadataDoc.Descendants("AudioPath")
                                               select new AudioPath()
                                               {
                                                   Path = (string)c.Element("FilePath"),
                                                   StartMark = (int)c.Element("StartMark"),
                                                   EndMark = (int)c.Element("EndMark")
                                               };

            IEnumerable<string> authors = from c in metadataDoc.Descendants("Author")
                                          select (string)c.Value;

            IEnumerable<string> readers = from c in metadataDoc.Descendants("Reader")
                                          select (string)c.Value;

            Metadata metadata = new Metadata();

            metadata.Contributors.Authors.AddRange(authors);
            metadata.Contributors.Readers.AddRange(readers);
            metadata.Title = GetSingleElement(metadataDoc, "Title");

            // TODO regex move to GetSingleElement
            string strTrackNumber = GetSingleElement(metadataDoc, "TrackNumber");
            if (Regex.IsMatch(strTrackNumber, "[0-9]+"))
            {
                metadata.TrackNumber = int.Parse(strTrackNumber);
            }
            string strReleaseYear = GetSingleElement(metadataDoc, "ReleaseYear");
            if (Regex.IsMatch(strTrackNumber, "[0-9]+"))
            {
                metadata.ReleaseYear = int.Parse(strReleaseYear);
            }

            return new Chapter(metadata);
        }

        /// <summary>
        /// Saves the metadata from a chapter to an XML file.
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="path"></param>
        public static void SaveChapterToXML(Chapter chapter, string path)
        {
            var metadataXML = ChapterToXML(chapter);
            //TODO: dont save before user finishes editing
            metadataXML.Save(path);
        }

        /// <summary>
        /// Converts the metadata of a chapter into an XDocument.
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        private static XDocument ChapterToXML(Chapter chapter)
        {
            var chapterXML = new XElement("Chapter");
            var audioPaths = new XElement("AudioPaths");
            foreach (AudioPath audioPath in AudioPaths)
            {
                var pathXML = new XElement("AudioPath");
                pathXML.Add(new XElement("FilePath", audioPath.Path));
                pathXML.Add(new XElement("StartMark", audioPath.StartMark));
                pathXML.Add(new XElement("EndMark", audioPath.EndMark));
                audioPaths.Add(pathXML);
            }
            chapterXML.Add(audioPaths);

            if (!chapter.Metadata.Title.Equals(string.Empty)) //TODO maybe != null?
            {
                chapterXML.Add(new XElement("Title", chapter.Metadata.Title));
            }
            if (chapter.Metadata.TrackNumber != 0)
            {
                chapterXML.Add(new XElement("TrackNumber", chapter.Metadata.TrackNumber));
            }
            if (!chapter.Metadata.Description.Equals(string.Empty))
            {
                chapterXML.Add(new XElement("Description", chapter.Metadata.Description));
            }
            if (chapter.Metadata.ReleaseYear != 0) //TODO what is standard value?
            {
                chapterXML.Add(new XElement("ReleaseYear", chapter.Metadata.ReleaseYear));
            }
            if (chapter.Metadata.Contributors.Authors.Count != 0)
            {
                XElement authors = new XElement("Authors");
                foreach (string author in chapter.Metadata.Contributors.Authors)
                {
                    authors.Add(new XElement("Author", author));
                }
                chapterXML.Add(authors);
            }
            if (chapter.Metadata.Contributors.Readers.Count != 0)
            {
                XElement readers = new XElement("Readers");
                foreach (string reader in chapter.Metadata.Contributors.Authors)
                {
                    readers.Add(new XElement("Reader", reader));
                }
                chapterXML.Add(readers);
            }
            var metadataXML = new XDocument();
            metadataXML.Add(chapterXML);
            return metadataXML;
        }

        /// <summary>
        /// Returns a single element of an XML document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetSingleElement(XDocument doc, string name)
        {
            var descendents = doc.Descendants(name);
            if (descendents.Count() > 0)
            {
                return descendents.First().Value;
            }
            return string.Empty;
        }

    }
}
