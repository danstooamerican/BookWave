using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message)
        {
        }

        public InvalidArgumentException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
