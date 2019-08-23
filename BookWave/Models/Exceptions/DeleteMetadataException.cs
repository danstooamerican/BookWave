using System;

namespace BookWave.Desktop.Exceptions
{
    [Serializable]
    public class DeleteMetadataException : Exception
    {
        public DeleteMetadataException(string message) : base(message)
        {
        }

        public DeleteMetadataException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
