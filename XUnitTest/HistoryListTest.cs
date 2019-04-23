using System;
using System.Collections.Generic;
using System.Text;
using Commons.Util;
using Xunit;

namespace XUnitTest
{
    public class HistoryListTest
    {
        [Fact]
        public void CreateList()
        {
            HistoryList<int> list = new HistoryList<int>();
            list.AddLast(4);
            int item = list.Get(0);
            Assert.Equal(4, item);
        }

        [Fact]
        public void AddFirst()
        {
            HistoryList<int> list = new HistoryList<int>();
            list.AddLast(4);
            list.AddLast(2);
            list.AddFirst(-5);
            int item = list.Get(0);
            Assert.Equal(-5, item);
        }

        [Fact]
        public void CheckSize()
        {
            HistoryList<int> list = new HistoryList<int>();
            list.AddLast(4);
            list.AddLast(2);
            list.AddFirst(-5);
            int item = list.Size();
            Assert.Equal(3, item);
        }

        [Fact]
        public void IndexOutOfBounds()
        {
            HistoryList<string> list = new HistoryList<string>();
            list.AddLast("Hello world.");
            string item = list.Get(1);
            // default for int is 0
            Assert.Null(item);
        }

        [Fact]
        public void AddAtIndexAndDeleteBehindTest()
        {
            HistoryList<int> list = new HistoryList<int>();
            list.AddLast(4);
            list.AddLast(2);
            list.AddFirst(-5);
            list.AddFirst(13);
            // [13, -5, 4, 2]
            list.AddAtIndexDeleteBehind(25, 1);
            // [13, -5, 25]
            int size = list.Size();
            Assert.Equal(3, size);
            int item0 = list.Get(0);
            Assert.Equal(13, item0);
            int item1 = list.Get(1);
            Assert.Equal(-5, item1);
            int item2 = list.Get(2);
            Assert.Equal(25, item2);
        }
    }
}
