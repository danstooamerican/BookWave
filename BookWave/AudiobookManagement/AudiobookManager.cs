using ATL;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
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
            get
            {
                if (mInstance == null)
                {
                    Instance = new AudiobookManager();
                }
                return mInstance;
            }
            private set { mInstance = value; }
        }

        private static readonly object IDLock = new object(); // TODO
        private int IDCount;
        // private Semaphore IDsemaphore = new Semaphore(1, 1); TODO

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
        /// Creates a new audiobook id. This method uses an auto increment method.
        /// </summary>
        /// <returns>unique runtime id for an audiobook</returns>
        private int GetNewID()
        {
            //Console.WriteLine("Before " + IDCount);
            //int id = Interlocked.Increment(ref IDCount);
            //Console.WriteLine("After " + id + " count " + IDCount);
            //return id;

            lock(IDLock)
            {
                IDCount++;
                Console.WriteLine("During " + IDCount);
                return IDCount;
            }

            //IDsemaphore.WaitOne();
            //IDCount++;
            //Console.WriteLine("During " + IDCount);
            //var temp = IDCount;
            //IDsemaphore.Release();
            //return temp;
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

                audiobook.Metadata.MetadataPath = Directory.GetParent(metadataPath).FullName;
            }

            return audiobook;
        }

        public Chapter CreateChapter(Track track) {
            return new Chapter(track);
        }

        public Chapter CreateChapter(string metadataPath)
        {
            Chapter chapter = new Chapter();

            if (File.Exists(metadataPath))
            {
                XDocument metadataDoc = XDocument.Load(metadataPath);

                var chapterRoot = XMLHelper.GetFirstXElement(metadataDoc, "Chapter");

                if (chapterRoot != null)
                {
                    chapter.FromXML(chapterRoot);
                }

                chapter.Metadata.MetadataPath = metadataPath;
            }

            return chapter;
        }

        public void UpdateAudiobook(Library library, Audiobook audiobook)
        {
            if (library != null)
            {
                library.UpdateAudiobook(audiobook);
            }
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
