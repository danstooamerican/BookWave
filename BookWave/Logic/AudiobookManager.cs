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

        private Repository mAudiobookRepo;
        public Repository AudiobookRepo
        {
            get { return mAudiobookRepo; }
            set { mAudiobookRepo = value; }
        }

        #region Public Properties

        private ObservableCollection<Audiobook> mAudiobooks;
        public ObservableCollection<Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ObservableCollection<Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
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

            Audiobooks = new ObservableCollection<Audiobook>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the AudiobookRepo and creates a new audiobook for each
        /// path in the repository if metadata files exists in the metadata folder.
        /// </summary>
        public void LoadRepository()
        {
            AudiobookRepo.LoadFromFile();

            foreach (string path in AudiobookRepo.Items)
            {
                Audiobook audiobook = LoadAudiobookFromFile(path);

                // only add Audiobook if metadata files exist in the metadata folder
                if (audiobook.Chapters.Count > 0)
                {
                    Audiobooks.Add(audiobook);
                }
            }
        }

        /// <summary>
        /// Loads an audiobook from a folder path.
        /// </summary>
        /// <param name="path">path to the audiobook folder</param>
        /// <returns>audiobook created from the folder</returns>
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

            audiobook.LoadChapters();

            return audiobook;
        }

        /// <summary>
        /// Searches for an audiobook with the given path.
        /// </summary>
        /// <param name="path">is the path of the audiobook</param>
        /// <returns>audiobook</returns>
        public Audiobook GetAudiobook(string path)
        {
            if (AudiobookRepo.Items.Contains(path))
            {
                 return Audiobooks.FirstOrDefault(audiobook => audiobook.Metadata.Path.Equals(path));
            }
            return null;
        }

        /// <summary>
        /// Adds an audiobook to the AudiobookManager and adds the path to the AudiobookRepo if it is not added already. 
        /// If it is already added the audiobook is updated.
        /// </summary>
        /// <param name="audiobook">the audiobook being added</param>
        public void UpdateAudioBook(Audiobook toAdd)
        {
            Audiobook audiobook = GetAudiobook(toAdd.Metadata.Path);

            if (audiobook != null)
            {
                Audiobooks.Remove(audiobook);
            } else
            {
                AudiobookRepo.Items.Add(toAdd.Metadata.Path);
                AudiobookRepo.SaveToFile();
            }            

            
            Audiobooks.Add(toAdd);
        }

        /// <summary>
        /// Removes an audiobook from the AudiobookManager 
        /// and removes the path from the AudiobookRepo.
        /// </summary>
        /// <param name="audiobook">the audiobook being removed</param>
        public void RemoveAudioBook(Audiobook audiobook)
        {
            AudiobookRepo.Items.Remove(audiobook.Metadata.Path);
            AudiobookRepo.SaveToFile();

            Audiobooks.Remove(audiobook);
        }

        #endregion

    }
}
