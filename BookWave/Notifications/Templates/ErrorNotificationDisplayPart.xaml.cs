using System.Windows;
using ToastNotifications.Core;

namespace BookWave.Desktop.Notifications.Templates
{
    /// <summary>
    /// Interaction logic for ErrorNotificationDisplayPart.xaml
    /// </summary>
    public partial class ErrorNotificationDisplayPart : NotificationDisplayPart
    {

        public ErrorNotificationDisplayPart(ErrorNotification customNotification)
        {
            InitializeComponent();
            Bind(customNotification);
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Notification.Close();
        }

    }
}
