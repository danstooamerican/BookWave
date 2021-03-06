﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Controls
{
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        #region Dependency Properties

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register("ImageSize", typeof(double), typeof(ImageButton),
            new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public string NormalImage
        {
            get { return (string)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string HoverImage
        {
            get { return (string)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string PressedImage
        {
            get { return (string)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public string DisabledImage
        {
            get { return (string)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register("DisabledImage", typeof(string), typeof(ImageButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.NewValue))
            {
                Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
            }            
        }

        #endregion
    }
}
