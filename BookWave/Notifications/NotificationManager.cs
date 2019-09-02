using BookWave.Desktop.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BookWave.Desktop.Notifications
{
    public class NotificationManager
    {
        public static readonly string NotificationIdentifier = "BookWave";

        private static Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 5,
                offsetY: 100);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(4),
                maximumNotificationCount: MaximumNotificationCount.FromCount(4));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


        private static IDictionary<string, CancellationTokenSource> activeNotifications = new Dictionary<string, CancellationTokenSource>();

        public static void DisplayWindowsNotification(BaseNotification notification)
        {
            string notificationID = notification.Group + "/" + notification.Tag;

            lock (activeNotifications)
            {
                if (activeNotifications.ContainsKey(notificationID))
                {
                    activeNotifications[notificationID].Cancel();
                    activeNotifications.Remove(notificationID);
                    ToastNotificationManager.History.Remove(notification.Tag, notification.Group, NotificationIdentifier);
                }
            }

            var tokenSource = new CancellationTokenSource();
            activeNotifications.Add(notificationID, tokenSource);

            var toastXml = new XmlDocument();
            toastXml.LoadXml(notification.BuildMessage());
            var toast = new ToastNotification(toastXml);

            toast.Tag = notification.Tag;
            toast.Group = notification.Group;

            ToastNotificationManager.CreateToastNotifier(NotificationIdentifier).Show(toast);

            var cancelToken = tokenSource.Token;

            if (notification.DisplayTime > 0)
            {
                Task.Delay(notification.DisplayTime).ContinueWith(t =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lock (activeNotifications)
                        {
                            if (!cancelToken.IsCancellationRequested)
                            {
                                ToastNotificationManager.History.Remove(notification.Tag, notification.Group, NotificationIdentifier);
                                activeNotifications.Remove(notificationID);
                            }
                        }
                    }));
                }, tokenSource.Token);
            }
        }

        public static void DisplayException(string message)
        {
            notifier.ShowErrorNotification(message);
        }

        public static void DisplayInfo(string message)
        {
            notifier.ShowInfoNotification(message);
        }

        public static void Dispose()
        {
            notifier.ClearMessages(new ClearAll());
            notifier.Dispose();
        }

    }
}
