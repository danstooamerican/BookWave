using Commons.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        private static readonly string UserDataPath
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "Bookwave");

        private static readonly string FilePath = Path.Combine(UserDataPath, "paths.txt");

        #region Public Properties

        private ObservableCollection<Audiobook> mAudiobooks;
        public ObservableCollection<Audiobook> Audiobooks
        {
            get { return mAudiobooks; }
            set { Set<ObservableCollection<Audiobook>>(() => this.Audiobooks, ref mAudiobooks, value); }
        }


        private Dictionary<string, Audiobook> mAudiobookSet;
        public Dictionary<string, Audiobook> AudiobookSet
        {
            get { return mAudiobookSet; }
            set { Set<Dictionary<string, Audiobook>>(() => this.AudiobookSet, ref mAudiobookSet, value); }
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
            AudiobookSet = new Dictionary<string, Audiobook>();

            Audiobooks = new ObservableCollection<Audiobook>();
            Directory.CreateDirectory(UserDataPath);

            CreateMissingAudiobooksFromUserData();
            //TODO use files
        }

        #endregion

        #region Methods

        private void CreateMissingAudiobooksFromUserData()
        {
            if (File.Exists(FilePath))
            {
                foreach (string path in File.ReadLines(FilePath)) {
                    //TODO: folderHandler call
                }
            }
        }

        private void VerifyFilePathsData()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
                //TODO save current audiobooks
            }
            HashSet<string> audioPaths = new HashSet<string>(AudiobookSet.Keys);
            foreach (string path in File.ReadLines(FilePath))
            {
                audioPaths.Remove(path);
            }
            foreach (string path in audioPaths)
            {
                
            }
        }

        /// <summary>
        /// Adds an AudioBook to the AudioBookManager 
        /// and saves the path where the AudioBook is 
        /// stored.
        /// </summary>
        /// <param name="audiobook">is the audiobook being added</param>
        public void AddAudioBook(Audiobook audiobook, string audiobookPath)
        {
            VerifyFilePathsData();

            Audiobooks.Add(audiobook);
            
            // saves the paths to file

        }

        #endregion

    }
}
