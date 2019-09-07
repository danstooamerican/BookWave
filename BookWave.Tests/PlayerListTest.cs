using BookWave.Desktop.Models.AudiobookManagement;
using BookWave.Desktop.Models.AudiobookPlayer;
using System.Collections.Generic;
using Xunit;

namespace BookWave.Tests
{
    public class PlayerListTest
    {

        [Fact]
        public void AddRangeNull()
        {
            var list = new PlayerList<string>();
            list.AddRange(null);

            Assert.Null(list.Current);
            Assert.Null(list.Last);
        }

        [Fact]
        public void AddRangeCollection()
        {
            var collection = new List<string>();
            collection.Add("hello");
            collection.Add("bonjour");
            collection.Add("hallo");

            var list = new PlayerList<string>();
            list.AddRange(collection);

            Assert.Equal("hello", list.Current.Element);
            Assert.Equal("hallo", list.Last.Element);
        }

        [Fact]
        public void AddRangeCollectionWithNull()
        {
            var collection = new List<string>();
            collection.Add("hello");
            collection.Add(null);
            collection.Add("hallo");

            var list = new PlayerList<string>();
            list.AddRange(collection);

            Assert.Equal("hello", list.Current.Element);
            list.Forward();
            Assert.Equal("hallo", list.Current.Element);
        }

        [Fact]
        public void PushBackNull()
        {
            var list = new PlayerList<string>();

            list.PushBack(null);
            Assert.Null(list.Current);
            Assert.Null(list.Last);
            var str = "ik ben een vogelbekdier";
            list.PushBack(str);
            list.PushBack(null);

            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);
        }

        [Fact]
        public void PushBack()
        {
            var list = new PlayerList<string>();

            var str = "ik ben een banaan";
            list.PushBack(str);

            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);

            var str2 = "ik heb twee bananen";
            list.PushBack(str2);

            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str2, list.Last.Element);
        }

        [Fact]
        public void InsertAfterCurrent()
        {
            var list = new PlayerList<string>();
            var str = "ik_ihe";

            list.InsertAfterCurrent(str);
            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);
            Assert.Equal(list.Current, list.Last);

            var str2 = "ich_iel";

            list.InsertAfterCurrent(str2);

            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str2, list.Current.Next.Element);
            Assert.Equal(str2, list.Last.Element);

            var str3 = "me_irl";
            list.InsertAfterCurrent(str3);

            Assert.Equal(str, list.Current.Element);
            list.Forward();
            Assert.Equal(str3, list.Current.Element);
            list.Forward();
            Assert.Equal(str2, list.Current.Element);
            list.Forward();
            Assert.Equal(str2, list.Current.Element);
            Assert.Equal(list.Last, list.Current);
            list.Back();
            Assert.Equal(str3, list.Current.Element);
            list.Back();
            Assert.Equal(str, list.Current.Element);
            list.Back();
            Assert.Equal(str, list.Current.Element);
        }

        [Fact]
        public void InsertAfterCurrentNull()
        {
            var list = new PlayerList<string>();
            var str = "ik_ihe";

            list.InsertAfterCurrent(null);
            Assert.Null(list.Current);
            Assert.Null(list.Last);

            list.InsertAfterCurrent(str);
            list.InsertAfterCurrent(null);
            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);
            Assert.Equal(list.Current, list.Last);
        }

        [Fact]
        public void DeleteAfterCurrentNull()
        {
            var list = new PlayerList<string>();

            list.DeleteAfterCurrent(null);
            Assert.Null(list.Current);
            Assert.Null(list.Last);

            var str = "epsilon->infty";

            list.InsertAfterCurrent(str);
            list.DeleteAfterCurrent(null);
            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);
        }

        [Fact]
        public void DeleteAfterCurrentNotFound()
        {
            var list = new PlayerList<string>();
            var str = "epsilon->infty";
            var strNotFound = "epsilon>0";

            list.DeleteAfterCurrent(strNotFound);

            Assert.Null(list.Current);
            Assert.Null(list.Last);

            list.InsertAfterCurrent(str);
            list.DeleteAfterCurrent(strNotFound);

            Assert.Equal(str, list.Current.Element);
            Assert.Equal(str, list.Last.Element);
        }

        [Fact] 
        public void DeleteAfterCurrent()
        {
            var list = new PlayerList<string>();
            var str = "perry";
            var str2 = "doofenschmirtz-evil-incorporated";
            var str3 = "trapped by societal convention";

            list.InsertAfterCurrent(str);
            list.InsertAfterCurrent(str2);
            list.InsertAfterCurrent(str3);

            // str, str3, str2

            list.DeleteAfterCurrent(str);

            // str3, str2

            Assert.Equal(str3, list.Current.Element);
            Assert.Equal(str2, list.Last.Element);

            list.DeleteAfterCurrent(str2);

            Assert.Equal(str3, list.Current.Element);
            Assert.Equal(str3, list.Last.Element);

            list.InsertAfterCurrent(str);
            list.InsertAfterCurrent(str2);

            // str3, str2, str

            list.DeleteAfterCurrent(str2);

            Assert.Equal(str3, list.Current.Element);
            list.Forward();
            Assert.Equal(str, list.Current.Element);
            Assert.Equal(list.Last, list.Current);
            list.Forward();
            Assert.Equal(str, list.Current.Element);
            list.Back();
            Assert.Equal(str3, list.Current.Element);
            list.Back();
            Assert.Equal(str3, list.Current.Element);
        }
    }
}
