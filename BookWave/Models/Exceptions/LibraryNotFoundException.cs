using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    class LibraryNotFoundException : Exception
    {
        public LibraryNotFoundException(string message) : base(message)
        {
        }

        public LibraryNotFoundException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
