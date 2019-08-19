using System;
using System.Windows;
using System.Windows.Data;

namespace BookWave.Styles.Converter
{
    public class ChaptersToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int val = int.Parse(value.ToString());

                return val == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
