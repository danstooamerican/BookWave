namespace BookWave.Desktop.Notifications
{
    public abstract class BaseNotification
    {
        public readonly string DefaultGroup = "BookWave.Notification";

        private string mTag;
        public string Tag
        {
            get { return mTag; }
            set { mTag = value; }
        }

        private string mGroup;
        public string Group
        {
            get { return mGroup; }
            set { mGroup = value; }
        }

        private int mDisplayTime;
        public int DisplayTime
        {
            get { return mDisplayTime; }
            set { mDisplayTime = value; }
        }

        public BaseNotification(string tag, string group, int displayTime)
        {
            Tag = tag;
            Group = string.IsNullOrEmpty(group) ? DefaultGroup : group;
            DisplayTime = displayTime;
        }

        public abstract string BuildMessage();

    }
}
