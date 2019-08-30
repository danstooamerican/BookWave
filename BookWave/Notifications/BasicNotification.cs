using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BookWave.Desktop.Notifications
{
    public class BasicNotification : BaseNotification
    {
        #region Properties

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }

        private string mFirstLine;
        public string FirstLine
        {
            get { return mFirstLine; }
            set { mFirstLine = value; }
        }

        private string mSecondLine;
        public string SecondLine
        {
            get { return mSecondLine; }
            set { mSecondLine = value; }
        }        

        protected string template;

        #endregion

        #region Constructor

        public BasicNotification(string tag, string group) : base(tag, group, 5)
        {
            Title = string.Empty;
            FirstLine = string.Empty;
            SecondLine = string.Empty;
        }

        #endregion

        public override string BuildMessage()
        {
            return 
                $@"<toast>
                    <visual>
                        <binding template=""ToastText04"">
                            <text id=""1"">{Title}</text>
                            <text id=""2"">{FirstLine}</text>
                            <text id=""3"">{SecondLine}</text>
                        </binding>
                    </visual>
                </toast>";
        }
    }
}
