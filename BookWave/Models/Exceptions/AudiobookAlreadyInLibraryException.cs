using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    public class AudiobookAlreadyInLibraryException : Exception
    {
        public AudiobookAlreadyInLibraryException(string message) : base(message)
        {
        }

        public AudiobookAlreadyInLibraryException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
