using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Commons.Logic
{
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

        private Repository mAudiobookRepo;
        public Repository AudiobookRepo
        {
            get { return mAudiobookRepo; }
            set { mAudiobookRepo = value; }
        }        

        private Dictionary<int, Audiobook> mAudiobooks;
        public Dictionary<int, Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<Dictionary<int, Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AudiobookManager.
        /// Reads the Path where the files are stored and 
        /// then reads the audiobooks.
        /// </summary>
        private AudiobookManager()
        {
            AudiobookRepo = new Repository(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BookWave"), "audiobookPaths.bw");

            Audiobooks = new Dictionary<int, Audiobook>();
            IDCount = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the AudiobookRepo and creates a new audiobook for each
        /// path in the repository if metadata files exists in the metadata folder.
        /// If no metadata files exist the path is removed from the repository.
        /// </summary>
        public void LoadRepository()
        {
            AudiobookRepo.LoadFromFile();

            List<string> toRemove = new List<string>();
            foreach (string path in AudiobookRepo.Items)
            {
                Audiobook audiobook = LoadAudiobookFromFile(path);
                audiobook.LoadChapters();

                // only add Audiobook if metadata files exist in the metadata folder
                if (audiobook.Chapters.Count > 0)
                {
                    Audiobooks.Add(audiobook.ID, audiobook);
                } else
                {
                    toRemove.Add(path);
                }
            }

            toRemove.ForEach(path => AudiobookRepo.Items.Remove(path));

            AudiobookRepo.SaveToFile();
        }

        /// <summary>
        /// Parses an audio book from a xml file. No chapters are loaded.
        /// </summary>
        /// <param name="path">path to the audio book folder</param>
        /// <returns>audio book created from the folder</returns>
        public Audiobook LoadAudiobookFromFile(string path)
        {
            // parse audiobook from xml file
            string audiobookMetadataPath = Path.Combine(path, ConfigurationManager.AppSettings.Get("metadata_folder"),
                ConfigurationManager.AppSettings.Get("audiobook_metadata_filename") + "."
                + ConfigurationManager.AppSettings.Get("metadata_extensions"));

            Audiobook audiobook = XMLHelper.XMLToAudiobook(audiobookMetadataPath);

            if (audiobook.Metadata.Path.Equals(string.Empty))
            {
                audiobook.Metadata.Path = path;
            }

            return audiobook;
        }

        /// <summary>
        /// Searches for an audio book with the given path.
        /// </summary>
        /// <param name="path">is the path of the audiobook</param>
        /// <returns>audiobook</returns>
        public Audiobook GetAudiobook(int id)
        {
            if (Contains(id))
            {
                return Audiobooks[id];
            }
            return null;
        }

        public Audiobook GetAudiobook(string path)
        {
            return Audiobooks.Values.FirstOrDefault(audiobook => audiobook.Metadata.Path.Equals(path));
        }

        public bool Contains(int id)
        {
            return Audiobooks.ContainsKey(id);
        }

        /// <summary>
        /// Adds an audiobook to the AudiobookManager and adds the path to the AudiobookRepo if it is not added already. 
        /// If it is already added the audiobook is updated.
        /// </summary>
        /// <param name="audiobook">the audiobook being added</param>
        public void UpdateAudioBook(Audiobook toAdd)
        {
            if (!Contains(toAdd.ID))
            {
                AudiobookRepo.Items.Add(toAdd.Metadata.Path);
                AudiobookRepo.SaveToFile();
            } else
            {
                Audiobooks.Remove(toAdd.ID);
            }
            Audiobooks.Add(toAdd.ID, toAdd);
        }

        /// <summary>
        /// Removes an audiobook from the AudiobookManager 
        /// and removes the path from the AudiobookRepo.
        /// </summary>
        /// <param name="audiobook">the audiobook being removed</param>
        public void RemoveAudioBook(int id)
        {
            if (Contains(id))
            {
                Audiobook toRemove = Audiobooks[id];
                AudiobookRepo.Items.Remove(toRemove.Metadata.Path);
                AudiobookRepo.SaveToFile();

                Audiobooks.Remove(id);
            }
        }

        public int GetNewID()
        {
            var temp = IDCount;
            IDCount++;
            return temp;
        }

        #endregion

    }
}
