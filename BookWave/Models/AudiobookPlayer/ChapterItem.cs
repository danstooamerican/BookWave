using BookWave.Desktop.Models.AudiobookManagement;
using System;

namespace BookWave.Desktop.Models.AudiobookPlayer
{
    public class ChapterItem : IComparable<ChapterItem>
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

        public int CompareTo(ChapterItem other)
        {
            if (other == null)
            {
                return 1;
            }

            if (other.Chapter == null)
            {
                return 1;
            }

            return other.Chapter.CompareTo(Chapter);
        }
    }
}
