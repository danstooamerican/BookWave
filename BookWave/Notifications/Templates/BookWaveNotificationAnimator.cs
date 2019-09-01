using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ToastNotifications.Core;

namespace BookWave.Desktop.Notifications.Templates
{
    public class BookWaveNotificationAnimator : INotificationAnimator
    {

        private readonly NotificationDisplayPart _displayPart;
        private readonly TimeSpan _showAnimationTime;
        private readonly TimeSpan _hideAnimationTime;

        public BookWaveNotificationAnimator(NotificationDisplayPart displayPart, TimeSpan showAnimationTime, TimeSpan hideAnimationTime)
        {
            _displayPart = displayPart;
            _showAnimationTime = showAnimationTime;
            _hideAnimationTime = hideAnimationTime;
        }

        public void Setup()
        {
        }

        public void PlayShowAnimation()
        {
            Storyboard storyboard = new Storyboard();

            SetFadeInAnimation(storyboard);
            
            storyboard.Begin();
        }

        private void SetFadeInAnimation(Storyboard storyboard)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                Duration = _showAnimationTime,
                From = 0,
                To = 1
            };
            storyboard.Children.Add(fadeInAnimation);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(fadeInAnimation, _displayPart);

            DoubleAnimation growYAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.Zero,
                From = 0,
                To = 1
            };
            storyboard.Children.Add(growYAnimation);

            Storyboard.SetTargetProperty(growYAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(growYAnimation, _displayPart);
        }

        public void PlayHideAnimation()
        {
            Storyboard storyboard = new Storyboard();

            SetFadeoutAnimation(storyboard);

            storyboard.Begin();
        }

        private void SetFadeoutAnimation(Storyboard storyboard)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                Duration = _hideAnimationTime,
                From = 1,
                To = 0
            };

            storyboard.Children.Add(fadeOutAnimation);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(fadeOutAnimation, _displayPart);

            DoubleAnimation growYAnimation = new DoubleAnimation
            {
                Duration = _hideAnimationTime,
                From = 1,
                To = 0
            };
            storyboard.Children.Add(growYAnimation);

            _displayPart.RenderTransformOrigin = new Point(1, 0.5);

            Storyboard.SetTargetProperty(growYAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growYAnimation, _displayPart);
        }
    }
}
