using System;
using System.Windows.Data;

namespace BookWave.Styles.Converter
{
    public class SecondsToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int seconds = (int)double.Parse(value.ToString());

                if (seconds < TimeSpan.MaxValue.TotalSeconds)
                {
                    TimeSpan time = TimeSpan.FromSeconds(seconds);

                    if (seconds < 60 * 60)
                    {
                        if (seconds < 60 * 10)
                        {
                            return time.ToString(@"m\:ss");
                        }
                        else
                        {
                            return time.ToString(@"mm\:ss");
                        }
                    }
                    else
                    {
                        if (seconds < 60 * 60 * 10)
                        {
                            return time.ToString(@"h\:mm\:ss");
                        }
                        else
                        {
                            return (int)time.TotalHours + time.ToString(@"\:mm\:ss");
                        }
                    }
                }
            }
            return "0:00";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
