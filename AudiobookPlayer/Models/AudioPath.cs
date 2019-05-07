using Commons.Exceptions;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Models
{
    /// <summary>
    /// Stores a path to an audio file with a startMark and an endMark.
    /// </summary>
    public class AudioPath : ObservableObject
    {

        #region Public Properties

        private string mPath;
        /// <summary>
        /// Path to the audio file.
        /// </summary>
        public string Path {
            get { return mPath; }
            set {
                if (File.Exists(value))
                {
                    Set<string>(() => this.Path, ref mPath, value);
                } else
                {
                    throw new InvalidArgumentException(value, "is not a valid path.");
                }                
            }
        }

        private int mStartMark;
        /// <summary>
        /// StartMark of the audio file in seconds.
        /// This allows to split files into multiple AudioPaths.
        /// </summary>
        public int StartMark        {
            get { return mStartMark; }
            set {
                if (value >= 0)
                {
                    Set<int>(() => this.StartMark, ref mStartMark, value);
                } else
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
            set {
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

        public AudioPath(string path, int startMark, int endMark)
        {
            Path = path;
            StartMark = startMark;
            EndMark = endMark;
        }

        public AudioPath()
        {
        }

        #endregion

    }
}
