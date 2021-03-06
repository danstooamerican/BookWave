﻿using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookWave.Styles.Converter
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string imagePath = value as string;
                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
