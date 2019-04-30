using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    public class HistoryList<T>
    {        

        #region Properties

        private HistoryListElement<T> mCurrentElement;
        public HistoryListElement<T> CurrentElement
        {
            get { return mCurrentElement; }
            set {
                PreviousElement = mCurrentElement;
                mCurrentElement = value;

                CurrentElementChangedEvent?.Invoke();
            }
        }

        private HistoryListElement<T> mPreviousElement;
        public HistoryListElement<T> PreviousElement
        {
            get { return mPreviousElement; }
            set { mPreviousElement = value; }
        }

        #endregion

        #region Events

        public delegate void CurrentElementChanged();

        public event CurrentElementChanged CurrentElementChangedEvent;

        #endregion

        #region Constructor

        public HistoryList()
        {
            CurrentElement = null;
            PreviousElement = null;
        }

        #endregion

        #region Methods

        public void Back()
        {
            CurrentElement = CurrentElement.Previous;
        }

        public void Forward()
        {
            CurrentElement = CurrentElement.Next;
        }

        public bool IsNotCurrentElement(T element)
        {
            if (CurrentElement != null)
            {
                return !CurrentElement.Element.Equals(element);
            } else
            {
                return true;
            }
        }

        public bool IsRepeatedElement()
        {
            if (CurrentElement != null && PreviousElement != null)
            {
                return CurrentElement.Element.Equals(PreviousElement.Element);
            }
            else
            {
                return false;
            }
        }

        public void AddAtCurrentElementDeleteBehind(T element)
        {
            if (CurrentElement == null)
            {
                CurrentElement = new HistoryListElement<T>(null, null, element);
            } else
            {
                CurrentElement.Next = new HistoryListElement<T>(null, CurrentElement, element);
                this.Forward();
            }
        }

        #endregion

    }
}
