using BookWave.Desktop.Exceptions;
using BookWave.Desktop.Util;
using GalaSoft.MvvmLight;
using System;
using System.IO;
using System.Xml.Linq;

namespace BookWave.Desktop.AudiobookManagement
{
    /// <summary>
    /// Stores a path to an audio file with a startMark and an endMark.
    /// </summary>
    public class AudioPath : ObservableObject, XMLSaveObject, ICloneable
    {

        private static readonly int DefaultStartMark = 0;
        private static readonly int DefaultEndMark = -1;

        #region Public Properties

        private string mPath;
        /// <summary>
        /// Path to the audio file.
        /// </summary>
        public string Path
        {
            get { return mPath; }
            set
            {
                Set<string>(() => this.Path, ref mPath, value);
                RaisePropertyChanged(nameof(PathNotValid));
            }
        }

        public bool PathNotValid { get { return !File.Exists(Path); } }

        private int mStartMark;
        /// <summary>
        /// StartMark of the audio file in seconds.
        /// This allows to split files into multiple AudioPaths.
        /// </summary>
        public int StartMark
        {
            get { return mStartMark; }
            set
            {
                if (value >= 0)
                {
                    Set<int>(() => this.StartMark, ref mStartMark, value);
                }
                else
                {
                    throw new InvalidArgumentException(value, "is not a valid mark");
                }
            }
        }

        /// <summary>
        /// EndMark of the audio file in seconds.
        /// This allows to split files into multiple AudioPaths.
        /// -1 if EndMark marks that the audio path goes to the end of file. 
        /// 
        /// TODO: remove -1 and use the exact endMark of the set audio file to
        /// reduce unneccessary checks.
        /// </summary>
        private int mEndMark;
        public int EndMark
        {
            get { return mEndMark; }
            set
            {
                if (value >= -1)
                {
                    Set<int>(() => this.EndMark, ref mEndMark, value);
                }
                else
                {
                    throw new InvalidArgumentException(value, "is not a valid mark");
                }
            }
        }

        #endregion

        #region Constructors

        public AudioPath(string path)
        {
            Path = path;
            StartMark = DefaultStartMark;
            EndMark = DefaultEndMark;
        }
        public AudioPath()
        {
            Path = string.Empty;
            StartMark = DefaultStartMark;
            EndMark = DefaultEndMark;
        }

        #endregion

        #region Methods
        public XElement ToXML()
        {
            var pathXML = new XElement("AudioPath");

            if (!Path.Equals(string.Empty))
            {
                pathXML.Add(new XElement("FilePath", Path));
            }

            if (StartMark != DefaultStartMark)
            {
                pathXML.Add(new XElement("StartMark", StartMark));
            }

            if (EndMark != DefaultEndMark)
            {
                pathXML.Add(new XElement("EndMark", EndMark));
            }

            return pathXML;
        }

        public void FromXML(XElement xmlElement)
        {
            Path = XMLHelper.GetSingleValue(xmlElement, "FilePath");
            StartMark = int.Parse(XMLHelper.GetSingleValue(xmlElement, "StartMark", DefaultStartMark.ToString()));
            EndMark = int.Parse(XMLHelper.GetSingleValue(xmlElement, "EndMark", DefaultEndMark.ToString()));
        }

        public object Clone()
        {
            AudioPath copy = new AudioPath();

            copy.EndMark = EndMark;
            copy.StartMark = StartMark;
            copy.Path = Path;

            return copy;
        }

        #endregion
    }
}
