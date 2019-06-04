using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    /// <summary>
    /// Helper class which implements a simple linked list to keep track of elements
    /// which have been visited. This class also allows to go backwards and forwards
    /// through the list. There is no jumpTo option yet.
    /// </summary>
    /// <typeparam name="T">Type of element the HistoryList tracks.</typeparam>
    public class HistoryList<T>
    {

        #region Properties

        private HistoryListElement<T> mCurrentElement;
        /// <summary>
        /// Element which is currently selected in the list.
        /// </summary>
        public HistoryListElement<T> CurrentElement
        {
            get { return mCurrentElement; }
            private set
            {
                PreviousElement = mCurrentElement;
                mCurrentElement = value;

                CurrentElementChangedEvent?.Invoke();
            }
        }

        private HistoryListElement<T> mPreviousElement;
        /// <summary>
        /// Previously selected element. This allows to check whether an entry
        /// gets added twice in a row. 
        /// </summary>
        public HistoryListElement<T> PreviousElement
        {
            get { return mPreviousElement; }
            private set { mPreviousElement = value; }
        }

        #endregion

        #region Events

        public delegate void CurrentElementChanged();

        /// <summary>
        /// Event which gets fired every time the current element is set.
        /// </summary>
        public event CurrentElementChanged CurrentElementChangedEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty HistoryList with CurrentElement and PreviousElement set
        /// to null.
        /// </summary>
        public HistoryList()
        {
            CurrentElement = null;
            PreviousElement = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// If the CurrentElement has a predecessor the CurrentElement is set to it.
        /// </summary>
        public void Back()
        {
            if (CurrentElement != null && CurrentElement.Previous != null)
            {
                CurrentElement = CurrentElement.Previous;
            }
        }

        /// <summary>
        /// If the CurrentElement has a successor the CurrentElement is set to it.
        /// </summary>
        public void Forward()
        {
            if (CurrentElement != null && CurrentElement.Next != null)
            {
                CurrentElement = CurrentElement.Next;
            }
        }

        /// <summary>
        /// Checks if the passed element is equal to the CurrentElement. If the list is empty
        /// the check always returns true.
        /// </summary>
        /// <param name="element">Element to check</param>
        /// <returns>If the two elemens are equal.</returns>
        public bool IsNotCurrentElement(T element)
        {
            if (CurrentElement != null)
            {
                return !CurrentElement.Element.Equals(element);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the CurrentElement and the PreviousElement are equal.
        /// </summary>
        /// <returns>If the two elemens are equal.</returns>
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

        /// <summary>
        /// Inserts a new element behind the CurrentElement and moves the CurrentElement to the
        /// new element. 
        /// This method removes all elements which came after CurrentElement and leaves
        /// the inserted element as the new end of the list.
        /// </summary>
        /// <param name="element">Element to insert.</param>
        public void AddAtCurrentElementDeleteBehind(T element)
        {
            if (CurrentElement == null)
            {
                CurrentElement = new HistoryListElement<T>(null, null, element);
            }
            else
            {
                CurrentElement.Next = new HistoryListElement<T>(null, CurrentElement, element);
                this.Forward();
            }
        }

        #endregion

    }
}
