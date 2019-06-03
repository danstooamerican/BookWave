using Commons.Models;
using Commons.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

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

            LoadRepository();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the AudiobookRepo and creates a new audiobook for each
        /// path in the repository.
        /// </summary>
        private void LoadRepository()
        {
            AudiobookRepo.LoadFromFile();

            foreach (string path in AudiobookRepo.Items)
            {
                string metadataPath = Path.Combine(path, ConfigurationManager.AppSettings.Get("metadata_folder"));

                // only add Audiobook if metadata files exist in the metadata folder
                if (Directory.Exists(metadataPath) 
                    && Directory.GetFiles(Path.Combine(metadataPath, 
                    "*." + ConfigurationManager.AppSettings.Get("metadata_extension"))).Length > 0)
                {
                    Audiobooks.Add(new Audiobook(path));
                }                
            }
        }

        /// <summary>
        /// Adds an audiobook to the AudiobookManager 
        /// and adds the path to the AudiobookRepo.
        /// </summary>
        /// <param name="audiobook">the audiobook being added</param>
        public void AddAudioBook(Audiobook audiobook)
        {
            AudiobookRepo.Items.Add(audiobook.Metadata.Path);
            AudiobookRepo.SaveToFile();

            Audiobooks.Add(audiobook);
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
