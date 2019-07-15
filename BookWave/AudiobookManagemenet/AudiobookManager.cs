using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Commons.AudiobookManagemenet
{
    /// <summary>
    /// AudiobookManager is a facade of the LibraryManager which simplifies the management of audiobooks
    /// accross different libraries.
    /// </summary>
    public class AudiobookManager : ObservableObject
    {
        #region Public Properties

        private static AudiobookManager mInstance;
        public static AudiobookManager Instance
        {
            get {
                if (mInstance == null)
                {
                    Instance = new AudiobookManager();
                }
                return mInstance;
            }
            private set { mInstance = value; }
        }

        private int IDCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AudiobookManager.
        /// </summary>
        private AudiobookManager()
        {
            IDCount = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new Library id. This method uses an auto increment method.
        /// </summary>
        /// <returns>unique runtime id for a library</returns>
        private int GetNewID()
        {
            var temp = IDCount;
            IDCount++;
            return temp;
        }

        public Audiobook GetAudiobook(string destination)
        {
            foreach (Library library in LibraryManager.Instance.GetLibraries())
            {
                Audiobook found = library.GetAudiobooks().FirstOrDefault(a => a.Metadata.Path.Equals(destination));

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        public ICollection<Audiobook> GetAllAudiobooks()
        {
            ICollection<Audiobook> audiobooks = new List<Audiobook>();

            foreach (Library library in LibraryManager.Instance.GetLibraries())
            {
                foreach (Audiobook audiobook in library.GetAudiobooks())
                {
                    audiobooks.Add(audiobook);
                }
            }

            return audiobooks;
        }

        public Audiobook CreateAudiobook()
        {
            return new Audiobook(GetNewID());
        }

        public Audiobook CreateAudiobook(string metadataPath)
        {
            Audiobook audiobook = CreateAudiobook();

            if (File.Exists(metadataPath))
            {
                XDocument metadataDoc = XDocument.Load(metadataPath);

                var audiobookRoot = XMLHelper.GetFirstXElement(metadataDoc, "Audiobook");

                if (audiobookRoot != null)
                {
                    audiobook.FromXML(audiobookRoot);
                }
            }

            return audiobook;
        }

        public void UpdateAudiobook(Library library, Audiobook audiobook)
        {
            if (library != null)
            {
                library.UpdateAudiobook(audiobook);
            }
        }

        public void UpdateAudiobook(Audiobook audiobook)
        {
            UpdateAudiobook(audiobook.Library, audiobook);
        }

        public void RemoveAudiobook(Audiobook audiobook)
        {
            if (audiobook != null && audiobook.Library != null)
            {
                audiobook.Library.RemoveAudiobook(audiobook);
            }            
        }

        #endregion

    }
}
