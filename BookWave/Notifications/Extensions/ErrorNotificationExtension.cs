using ToastNotifications;

namespace BookWave.Desktop.Notifications.Extensions
{
    public static class CustomMessageExtensions
    {
        public static void ShowErrorNotification(this Notifier notifier, string message)
        {
            notifier.Notify<ErrorNotification>(() => new ErrorNotification(message));
        }
    }
}
