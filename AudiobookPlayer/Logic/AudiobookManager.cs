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
        private static readonly string UserData
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "Bookwave");

        private static readonly string FilePath = Path.Combine(UserData, "paths.txt");

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
        /// <param name="path">is the file where the different 
        /// repositories are stored in</param>
        public AudiobookManager(string path)
        {
            Audiobooks = new ObservableCollection<Audiobook>();
            path = UserData; //TODO
            Directory.CreateDirectory(path);
            if (File.Exists(FilePath))
            {
                List<string> files = new List<string>(File.ReadLines(FilePath));
            } else
            {
                File.Create(FilePath);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an AudioBook to the AudioBookManager 
        /// and saves the path where the AudioBook is 
        /// stored.
        /// </summary>
        /// <param name="audiobook">is the audiobook being added</param>
        public void AddAudioBook(Audiobook audiobook)
        {
            // saves the paths to file
            using (StreamWriter sw = File.AppendText(FilePath))
            {
                // gets all paths for the audiobook
                // usually just one path
                HashSet<string> audioPaths = new HashSet<string>();
                foreach (Chapter chapter in audiobook.Chapters)
                {
                    foreach (AudioPath path in chapter.AudioPaths)
                    {
                        audioPaths.Add(Path.GetDirectoryName(path.Path));
                    }
                }
                foreach (string path in audioPaths)
                {
                    sw.WriteLine(path);
                }
            }

            Audiobooks.Add(audiobook);
        }

        #endregion

    }
}
