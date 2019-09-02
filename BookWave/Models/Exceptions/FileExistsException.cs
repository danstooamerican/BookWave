using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    public class FileExistsException : Exception
    {
        public FileExistsException(string message) : base(message)
        {
        }

        public FileExistsException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
