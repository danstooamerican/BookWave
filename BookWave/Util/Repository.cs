using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    /// <summary>
    /// Holds a unique set of elements and saves them to a file.
    /// </summary>
    public class Repository
    {

        #region Private Properties

        public string FolderPath { get; private set; }

        public string FilePath { get; private set; }

        private HashSet<string> mItem;
        public HashSet<string> Items
        {
            get { return mItem; }
            set { mItem = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Repository but doesn't load anything.
        /// </summary>
        /// <param name="folderPath">Path to the folder the repository file is in.</param>
        /// <param name="fileName">Name of the repository file</param>
        public Repository(string folderPath, string fileName)
        {
            FolderPath = folderPath;            
            FilePath = Path.Combine(folderPath, fileName);
            this.Items = new HashSet<string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves all paths currently stored to the repository file.
        /// </summary>
        public void SaveToFile()
        {
            Directory.CreateDirectory(FolderPath);

            File.WriteAllLines(FilePath, Items);
        }

        /// <summary>
        /// Loads all paths saved in the repository file.
        /// </summary>
        public void LoadFromFile()
        {
            Items.Clear();

            if (File.Exists(FilePath))
            {
                foreach (string item in File.ReadLines(FilePath))
                {
                    Items.Add(item);
                }
            }
        }

        

        #endregion

    }
}
