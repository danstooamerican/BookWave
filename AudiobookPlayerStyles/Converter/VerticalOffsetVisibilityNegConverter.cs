using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Commons.Styles.Converter
{
    public class VerticalOffsetVisibilityNegConverter : IValueConverter
    {
        public object Convert(object offset, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (offset != null)
            {
                var sharedPageTitle = parameter as TextBlock;
                if (sharedPageTitle != null)
                {
                    double height = double.Parse(sharedPageTitle.Text, CultureInfo.InvariantCulture);
                    double threshhold = height * 0.6;
                    double offsetDouble = double.Parse(offset.ToString());
                    if (offsetDouble > threshhold)
                    {
                        return Visibility.Visible;
                    }
                }

            }
            return Visibility.Hidden;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
