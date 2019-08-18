using System;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Controls
{

    public class ImageTextBox : TextBox
    {
        static ImageTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextBox), new FrameworkPropertyMetadata(typeof(ImageTextBox)));
        }

        #region Dependency Properties

        public string ImageToolTipText
        {
            get { return (string)GetValue(ImageToolTipTextProperty); }
            set { SetValue(ImageToolTipTextProperty, value); }
        }

        public static readonly DependencyProperty ImageToolTipTextProperty =
            DependencyProperty.Register("ImageToolTipText", typeof(string), typeof(ImageTextBox), new PropertyMetadata(string.Empty));

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageTextBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Visibility ImageVisibility
        {
            get { return (Visibility)GetValue(ImageVisibilityProperty); }
            set { SetValue(ImageVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ImageVisibilityProperty =
            DependencyProperty.Register("ImageVisibility", typeof(Visibility), typeof(ImageTextBox), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
        }

    }
}
