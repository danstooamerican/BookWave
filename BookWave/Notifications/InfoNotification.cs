using BookWave.Desktop.Notifications.Templates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace BookWave.Desktop.Notifications
{
    public class InfoNotification : MessageBase<InfoNotificationDisplayPart>, INotifyPropertyChanged
    {
        private InfoNotificationDisplayPart _displayPart;

        public override NotificationDisplayPart DisplayPart => _displayPart ?? (_displayPart = new InfoNotificationDisplayPart(this));

        public InfoNotification(string message, MessageOptions options) : base(message, options)
        {
            Message = message;
        }

        public InfoNotification(string message) : base(message, new MessageOptions())
        {
            Message = message;
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void UpdateDisplayOptions(InfoNotificationDisplayPart displayPart, MessageOptions options)
        {
            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override InfoNotificationDisplayPart CreateDisplayPart()
        {
            return new InfoNotificationDisplayPart(this);
        }
    }
}
