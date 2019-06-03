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

        private string repositoryPath;

        private HashSet<string> mItem;

        public HashSet<string> Items
        {
            get { return mItem; }
            set { mItem = value; }
        }


        #endregion

        #region Constructor

        public Repository(string path)
        {
            this.repositoryPath = path;
            this.Items = new HashSet<string>();
        }

        #endregion

        #region Methods

        public void SaveToFile()
        {
            if (File.Exists(repositoryPath))
            {
                File.WriteAllLines(repositoryPath, Items);
            }
        }

        public void LoadFromFile()
        {
            Items.Clear();

            if (File.Exists(repositoryPath))
            {
                foreach (string item in File.ReadLines(repositoryPath))
                {
                    Items.Add(item);
                }
            }
        }

        

        #endregion

    }
}
