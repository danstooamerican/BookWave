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

        public Repository(string folderPath, string fileName)
        {
            FolderPath = folderPath;            
            FilePath = Path.Combine(folderPath, fileName);
            this.Items = new HashSet<string>();
        }

        #endregion

        #region Methods

        public void SaveToFile()
        {
            Directory.CreateDirectory(FolderPath);

            File.WriteAllLines(FilePath, Items);
        }

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
