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
            Metadata = new Metadata(track);
        }

        #endregion

        #region Methods

        public void ToXML()
        {
            XElement metadataXML = new XElement("Metadata");
            metadataXML.Add(new XElement("Path", Metadata.Path));

            if (!Metadata.Title.Equals(string.Empty)) //TODO maybe != null?
            {
                metadataXML.Add(new XElement("Title", Metadata.Title));
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

            System.Console.WriteLine(metadataXML);
            
        }

        #endregion

    }
}
