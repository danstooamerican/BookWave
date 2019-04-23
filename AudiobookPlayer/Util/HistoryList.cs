using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    public class HistoryList<T>
    {
        public HistoryList()
        {
            this.CurrentElement = null;
        }

        private HistoryListElement<T> mCurrentElement;

        public HistoryListElement<T> CurrentElement
        {
            get { return mCurrentElement; }
            set { mCurrentElement = value; }
        }

        public T Back()
        {
            T temp = CurrentElement.Element;
            CurrentElement = CurrentElement.Previous;
            return temp;
        }

        public T Forward()
        {
            T temp = CurrentElement.Element;
            CurrentElement = CurrentElement.Next;
            return temp;
        }

        public void AddAtCurrentElementDeleteBehind(T element)
        {
            if (CurrentElement == null)
            {
                CurrentElement = new HistoryListElement<T>(null, null, element);
            } else
            {
                CurrentElement.Next = new HistoryListElement<T>(null, CurrentElement, element);
                CurrentElement = CurrentElement.Next;
            }
        }
    }
}
