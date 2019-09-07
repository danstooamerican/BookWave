namespace BookWave.Desktop.Util
{
    public class ListElement<T>
    {
        private ListElement<T> mNext;
        public ListElement<T> Next
        {
            get { return mNext; }
            set { mNext = value; }
        }

        private ListElement<T> mPrevious;
        public ListElement<T> Previous
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

        public ListElement(ListElement<T> next, ListElement<T> previous, T element)
        {
            Element = element;
            Previous = previous;
            Next = next;
        }
    }
}
