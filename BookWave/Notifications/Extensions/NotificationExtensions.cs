using ToastNotifications;

namespace BookWave.Desktop.Notifications.Extensions
{
    public static class NotificationExtensions
    {
        public static void ShowErrorNotification(this Notifier notifier, string message)
        {
            notifier.Notify<ErrorNotification>(() => new ErrorNotification(message));
        }

        public static void ShowInfoNotification(this Notifier notifier, string message)
        {
            notifier.Notify<InfoNotification>(() => new InfoNotification(message));
        }
    }
}
