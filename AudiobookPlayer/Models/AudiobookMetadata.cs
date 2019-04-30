using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    public class AudiobookMetadata : Metadata
    {

        #region Public Properties

        private string mGenre;

        public string Genre
        {
            get { return mGenre; }
            set { mGenre = value; }
        }


        #endregion

    }
}
