using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    public class HistoryListElement<T>
    {
        private HistoryListElement<T> mNext;
        public HistoryListElement<T> Next
        {
            get { return mNext; }
            set { mNext = value; }
        }

        private HistoryListElement<T> mPrevious;
        public HistoryListElement<T> Previous
        {
            get { return mPrevious; }
            set { mPrevious = value; }
        }

        private T mElement;
        public T Element
        {
            get { return mElement; }
            set { mElement = value; }
        }

        public HistoryListElement(HistoryListElement<T> next, HistoryListElement<T> previous, T element)
        {
            Element = element;
            Previous = previous;
            Next = next;
        }
    }
}
