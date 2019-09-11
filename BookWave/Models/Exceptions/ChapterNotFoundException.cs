using BookWave.Desktop.Models.AudiobookManagement;
using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    class ChapterNotFoundException : Exception
    {
        private Chapter mChapter;

        public Chapter Chapter
        {
            get { return mChapter; }
            set { mChapter = value; }
        }


        public ChapterNotFoundException(string message) : base(message)
        {
        }

        public ChapterNotFoundException(Chapter chapter, string message) : base("'" + chapter.AudioPath.Path + "' " + message)
        {
            Chapter = chapter;
        }
    }
}
