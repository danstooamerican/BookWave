using Infralution.Localization.Wpf;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BookWave.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "LightTheme";
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "DarkTheme";
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "TestTheme";
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            CultureManager.UICulture = new CultureInfo("en-US");
        }

        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            CultureManager.UICulture = new CultureInfo("de-DE");
        }

        private void Button_Click_5(object sender, System.Windows.RoutedEventArgs e)
        {
            var xml = @"<toast>
              <visual>
                <binding template=""ToastText04"">
                  <text id=""1"">This is Notification Message</text>
                    <text id=""2"">This is second line Notification</text>
                    <text id=""3"">This is Third line Notification</text>
                  </binding>
                </visual>
              </toast>";
            var toastXml = new XmlDocument();
            toastXml.LoadXml(xml);
            var toast = new ToastNotification(toastXml);
            string tag = "ex" + DateTimeOffset.Now;
            toast.Tag = tag;
            toast.Group = "BookWave.Exception";

            ToastNotificationManager.CreateToastNotifier("Sample toast").Show(toast);

            Task.Delay(5000).ContinueWith(t =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ToastNotificationManager.History.Remove(tag, "BookWave.Exception", "Sample toast");
                }));
                
            });
        }
    }
}
