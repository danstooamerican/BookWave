using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Util
{
    public class HistoryList<T>
    {
        private HistoryListElement<T> head;
        public HistoryList()
        {
            this.head = null;
        }

        private HistoryListElement<T> GetListElement(int index)
        {
            HistoryListElement<T> listElement = head;
            if (index > this.Size())
            {
                return null;
            }
            for (int i = 0; i < index; i++)
            {
                listElement = listElement.Next;
            }
            if (listElement == null)
            {
                return null;
            }
            return listElement;
        }

        public T Get(int index)
        {
            HistoryListElement<T> listElement = this.GetListElement(index);
            if (listElement == null)
            {
                return default(T);
            }
            return listElement.Element;
        }

        public void AddAtIndexDeleteBehind(T element, int index)
        {
            HistoryListElement<T> elementAtIndex = this.GetListElement(index);
            HistoryListElement<T> newElement = new HistoryListElement<T>(null, elementAtIndex, element);
            elementAtIndex.Next = newElement;
        }

        public void AddLast(T element)
        {
            int size = this.Size();
            HistoryListElement<T> lastElement = this.GetListElement(size - 1);
            HistoryListElement<T> newElement = new HistoryListElement<T>(null, lastElement, element);
            if (lastElement != null)
            {
                lastElement.Next = newElement;
            } else
            {
                this.head = newElement;
            }
        }
        
        public void AddFirst(T element)
        {
            HistoryListElement<T> firstElement = this.head;
            HistoryListElement<T> newElement = new HistoryListElement<T>(firstElement, null, element);
            this.head = newElement;
        }

        public int Size()
        {
            int i = 0;
            HistoryListElement<T> element = head;
            while (element != null)
            {
                i++;
                element = element.Next;
            }
            return i;
        }
    }
}
