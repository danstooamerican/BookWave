using System;

namespace BookWave.Desktop.Exceptions
{
    class ScannerNotFoundException : Exception
    {
        public ScannerNotFoundException(string message) : base(message)
        {
        }

        public ScannerNotFoundException(object argument, string message) : base("'" + argument + "' " + message)
        {
        }
    }
}
