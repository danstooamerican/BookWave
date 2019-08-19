using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWave.Desktop.AudiobookManagement
{
    /// <summary>
    /// Null Object for Audiobooks
    /// </summary>
    public class AudiobookDummy : Audiobook
    {
        public static readonly int DummyId = -1;
        public AudiobookDummy() : base(DummyId)
        {
            // initialize with invalid id so it can't be found
        }

    }
}
