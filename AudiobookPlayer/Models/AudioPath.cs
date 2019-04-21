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
    public class AudioPath : ObservableObject
    {

        #region Public Properties

        private string mPath;
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
        /// -1 if EndMark is end of file.
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

    }
}
