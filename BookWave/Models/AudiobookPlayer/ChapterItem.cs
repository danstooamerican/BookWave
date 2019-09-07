using BookWave.Desktop.Models.AudiobookManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWave.Desktop.Models.AudiobookPlayer
{
    public class ChapterItem
    {

        private Chapter mChapter;

        public Chapter Chapter
        {
            get { return mChapter; }
            private set { mChapter = value; }
        }


        public ChapterItem(Chapter chapter)
        {
            Chapter = chapter;
        }

    }
}
