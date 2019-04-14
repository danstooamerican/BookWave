using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Commons.Styles.Converter
{
    public class SecondsToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int seconds = (int) double.Parse(value.ToString());

                if (seconds < TimeSpan.MaxValue.TotalSeconds)
                {
                    TimeSpan time = TimeSpan.FromSeconds(seconds);

                    if (seconds < 60 * 60)
                    {
                        if (seconds < 60 * 10)
                        {
                            return time.ToString(@"m\:ss");
                        } else
                        {
                            return time.ToString(@"mm\:ss");
                        }                        
                    } else
                    {
                        if (seconds < 60 * 60 * 10)
                        {
                            return time.ToString(@"h\:mm\:ss");
                        } else if (seconds < 60 * 60 * 24)
                        {
                            return time.ToString(@"hh\:mm\:ss");
                        } else
                        {
                            return time.ToString(@"d\d\:hh\:mm");
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
