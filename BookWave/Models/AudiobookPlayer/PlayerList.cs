using BookWave.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookWave.Desktop.Models.AudiobookPlayer
{
    public class PlayerList<T>
    {

        #region Properties

        private ListElement<T> mCurrent;
        /// <summary>
        /// Element which is currently selected in the list.
        /// </summary>
        public ListElement<T> Current
        {
            get { return mCurrent; }
            private set
            {
                mCurrent = value;

                CurrentElementChangedEvent?.Invoke(this, null);
            }
        }

        private ListElement<T> mLast;
        public ListElement<T> Last
        {
            get { return mLast; }
            private set { mLast = value; }
        }


        #endregion

        #region Events

        /// <summary>
        /// Event which gets fired every time the current element is set.
        /// </summary>
        public event EventHandler CurrentElementChangedEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty PlayerList with CurrentElement set to null.
        /// </summary>
        public PlayerList()
        {
            Current = null;
            Last = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// If the CurrentElement has a predecessor the CurrentElement is set to it.
        /// </summary>
        public void Back()
        {
            if (Current != null && Current.Previous != null)
            {
                Current = Current.Previous;
            }
        }

        /// <summary>
        /// If the CurrentElement has a successor the CurrentElement is set to it.
        /// </summary>
        public void Forward()
        {
            if (Current != null && Current.Next != null)
            {
                Current = Current.Next;
            }
        }

        /// <summary>
        /// Adds a collection to the end of the list.
        /// </summary>
        /// <param name="collection">is the collection</param>
        public void AddRange(ICollection<T> collection)
        {
            if (collection == null)
            {
                return;
            }

            foreach (T element in collection)
            {
                PushBack(element);
            }
        }

        /// <summary>
        /// Adds an element to the end of the List.
        /// </summary>
        /// <param name="element"></param>
        public void PushBack(T element)
        {
            if (element == null)
            {
                return;
            }

            ListElement<T> newElement = new ListElement<T>(null, Last, element);
            if (Last != null)
            {
                Last.Next = newElement;
            } else
            {
                Current = newElement;
            }
            Last = newElement;
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        public void Clear()
        {
            Current = null;
            Last = null;
        }

        public void InsertAfterCurrent(T element)
        {
            if (element == null)
            {
                return;
            }

            if (Current == null)
            {
                Current = new ListElement<T>(null, null, element);
                Last = Current;
            } else
            {
                ListElement<T> newElement = new ListElement<T>(Current.Next, Current, element);
                Current.Next = newElement;
                if (newElement.Next != null)
                {
                    newElement.Next.Previous = newElement;
                }

                if (Current.Equals(Last))
                {
                    Last = newElement;
                }
            }
        }

        private void Delete(ListElement<T> element)
        {
            if (element == null)
            {
                return;
            }

            var prev = element.Previous;
            var next = element.Next;

            if (prev != null)
            {
                prev.Next = next;
            }
            if (next != null)
            {
                next.Previous = prev;
            }

            if (element.Equals(Current))
            {
                Current = next;
            }
            if (element.Equals(Last))
            {
                Last = prev;
            }
        }

        /// <summary>
        /// Deletes the first appearance of the element after the current pointer.
        /// Does nothing if the element doesn't exist.
        /// </summary>
        /// <param name="element">is the element being deleted</param>
        public void DeleteAfterCurrent(T element)
        {
            if (element == null)
            {
                return;
            }

            PushBack(element);

            ListElement<T> cur = Current;
            while (!cur.Element.Equals(element))
            {
                cur = cur.Next;
            }

            if (!cur.Equals(Last))
            {
                Delete(cur);
            }

            Delete(Last);
        }

        #endregion

    }
}
