using ATL;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Commons.Models
{
    public class Chapter : ObservableObject
    {

        #region Public Properties

        private ObservableCollection<AudioPath> mAudioPaths;
        public ObservableCollection<AudioPath> AudioPaths
        {
            get { return mAudioPaths; }
            set { Set<ObservableCollection<AudioPath>>(() => this.AudioPaths, ref mAudioPaths, value); }
        }

        private Metadata mMetadata;
        public Metadata Metadata
        {
            get { return mMetadata; }
            set { mMetadata = value; }
        }

        #endregion

        #region Constructors

        public Chapter(Track track)
        {
            AudioPaths = new ObservableCollection<AudioPath>();
            AudioPaths.Add(new AudioPath(track.Path, 0, -1));
            Metadata = new Metadata(track);
        }

        #endregion

        #region Methods

        public void ToXML(string path)
        {
            var metadataXML = new XElement("Metadata");
            var audioPaths = new XElement("AudioPaths");
            foreach (AudioPath audioPath in AudioPaths)
            {
                var pathXML = new XElement("AudioPath");
                pathXML.Add(new XElement("FilePath", audioPath.Path));
                pathXML.Add(new XElement("StartMark", audioPath.StartMark));
                pathXML.Add(new XElement("EndMark", audioPath.EndMark));
                audioPaths.Add(pathXML);
            }
            metadataXML.Add(audioPaths);

            if (!Metadata.Title.Equals(string.Empty)) //TODO maybe != null?
            {
                metadataXML.Add(new XElement("Title", Metadata.Title));
            } 
            if (Metadata.TrackNumber != 0)
            {
                metadataXML.Add(new XElement("TrackNumber", Metadata.TrackNumber));
            }
            if (!Metadata.Description.Equals(string.Empty))
            {
                metadataXML.Add(new XElement("Description", Metadata.Description));
            }
            if (Metadata.ReleaseYear != 0) //TODO what is standard value?
            {
                metadataXML.Add(new XElement("ReleaseYear", Metadata.ReleaseYear));
            }
            if (Metadata.Contributors.Authors.Count != 0)
            {
                XElement authors = new XElement("Authors");
                foreach (string author in Metadata.Contributors.Authors)
                {
                    authors.Add(new XElement("Author", author));
                }
                metadataXML.Add(authors);
            }
            if (Metadata.Contributors.Readers.Count != 0)
            {
                XElement readers = new XElement("Readers");
                foreach (string reader in Metadata.Contributors.Authors)
                {
                    readers.Add(new XElement("Reader", reader));
                }
                metadataXML.Add(readers);
            }
            XDocument xdoc = new XDocument();
            xdoc.Add(metadataXML);
            xdoc.Save(path);
        }

        #endregion

    }
}
