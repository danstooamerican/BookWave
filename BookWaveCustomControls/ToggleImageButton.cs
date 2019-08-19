using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace BookWave.Controls
{
    public class ToggleImageButton : ToggleButton
    {
        static ToggleImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleImageButton), new FrameworkPropertyMetadata(typeof(ToggleImageButton)));
        }

        #region DependencyProperties

        // copied from ImageButton
        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register("ImageSize", typeof(double), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public string NormalImage
        {
            get { return (string)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string HoverImage
        {
            get { return (string)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string PressedImage
        {
            get { return (string)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string DisabledImage
        {
            get { return (string)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register("DisabledImage", typeof(string), typeof(ToggleImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
        }


        // toggled code

        public string NormalImageToggled
        {
            get { return (string)GetValue(NormalImageToggledProperty); }
            set { SetValue(NormalImageToggledProperty, value); }
        }

        public static readonly DependencyProperty NormalImageToggledProperty =
            DependencyProperty.Register("NormalImageToggled", typeof(string), typeof(ToggleImageButton), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string HoverImageToggled
        {
            get { return (string)GetValue(HoverImageToggledProperty); }
            set { SetValue(HoverImageToggledProperty, value); }
        }

        public static readonly DependencyProperty HoverImageToggledProperty =
            DependencyProperty.Register("HoverImageToggled", typeof(string), typeof(ToggleImageButton), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string PressedImageToggled
        {
            get { return (string)GetValue(PressedImageToggledProperty); }
            set { SetValue(PressedImageToggledProperty, value); }
        }

        public static readonly DependencyProperty PressedImageToggledProperty =
            DependencyProperty.Register("PressedImageToggled", typeof(string), typeof(ToggleImageButton), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        #endregion

    }
}
