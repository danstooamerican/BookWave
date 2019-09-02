using System.Windows;
using ToastNotifications.Core;

namespace BookWave.Desktop.Notifications.Templates
{
    /// <summary>
    /// Interaction logic for ErrorNotificationDisplayPart.xaml
    /// </summary>
    public partial class InfoNotificationDisplayPart : BookWaveNotificationDisplayPart
    {

        public InfoNotificationDisplayPart(InfoNotification customNotification)
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
