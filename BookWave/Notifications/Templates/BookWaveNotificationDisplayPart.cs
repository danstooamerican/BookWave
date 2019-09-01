using System;
using ToastNotifications.Core;
using ToastNotifications.Display;

namespace BookWave.Desktop.Notifications.Templates
{
    public class BookWaveNotificationDisplayPart : NotificationDisplayPart
    {
        protected BookWaveNotificationDisplayPart() : base()
        {
            Animator = new BookWaveNotificationAnimator(this, TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(300));
        }
    }
}
