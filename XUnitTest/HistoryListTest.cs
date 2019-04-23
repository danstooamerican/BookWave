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
        public void AddItemsToList()
        {
            HistoryList<int> list = new HistoryList<int>();
            list.AddLast(4);
            int item = list.Get(0);
            Assert.Equal(4, item);
        }

    }
}
