using BookWave.Desktop.Notifications.Templates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace BookWave.Desktop.Notifications
{
    public class ErrorNotification : MessageBase<ErrorNotificationDisplayPart>, INotifyPropertyChanged
    {
        private ErrorNotificationDisplayPart _displayPart;

        public override NotificationDisplayPart DisplayPart => _displayPart ?? (_displayPart = new ErrorNotificationDisplayPart(this));

        public ErrorNotification(string message, MessageOptions options) : base(message, options)
        {
            
        }

        public ErrorNotification(string message) : base(message, new MessageOptions())
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

        protected override void UpdateDisplayOptions(ErrorNotificationDisplayPart displayPart, MessageOptions options)
        {
            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override ErrorNotificationDisplayPart CreateDisplayPart()
        {
            return new ErrorNotificationDisplayPart(this);
        }
    }
}
