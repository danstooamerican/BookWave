using BookWave.Desktop.Notifications.Templates;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace BookWave.Desktop.Notifications
{
    public class DecisionNotification : MessageBase<DecisionNotificationDisplayPart>, INotifyPropertyChanged
    {
        #region Properties

        private DecisionNotificationDisplayPart _displayPart;

        private Action mYesCallback;
        public Action YesCallback
        {
            get { return mYesCallback; }
            set { mYesCallback = value; }
        }

        private Action mNoCallback;
        public Action NoCallback
        {
            get { return mNoCallback; }
            set { mNoCallback = value; }
        }

        private string mDecisionText;
        public string DecisionText
        {
            get { return mDecisionText; }
            set { mDecisionText = value; }
        }


        #endregion

        #region Commands

        public ICommand YesCommand { private set; get; }

        public ICommand NoCommand { private set; get; }

        #endregion

        #region Constructors

        public DecisionNotification(string decisionText, Action yesCallback, Action noCallback, MessageOptions options) : base(string.Empty, options)
        {
            DecisionText = decisionText;
            YesCallback = yesCallback;
            NoCallback = noCallback;
            YesCommand = new RelayCommand(YesAction);
            NoCommand = new RelayCommand(NoAction);
        }

        public DecisionNotification(string decisionText, Action yesCallback, Action noCallback) : base(string.Empty, new MessageOptions())
        {
            DecisionText = decisionText;
            YesCallback = yesCallback;
            NoCallback = noCallback;
            YesCommand = new RelayCommand(YesAction);
            NoCommand = new RelayCommand(NoAction);
        }

        #endregion

        #region Notification

        public override NotificationDisplayPart DisplayPart => _displayPart ?? (_displayPart = new DecisionNotificationDisplayPart(this));

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void UpdateDisplayOptions(DecisionNotificationDisplayPart displayPart, MessageOptions options)
        {
            displayPart.CloseButton.Visibility = options.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override DecisionNotificationDisplayPart CreateDisplayPart()
        {
            return new DecisionNotificationDisplayPart(this);
        }

        #endregion

        #region Methods

        private void YesAction()
        {
            YesCallback?.Invoke();
            Close();
        }

        private void NoAction()
        {
            NoCallback?.Invoke();
            Close();
        }

        #endregion
    }
}
