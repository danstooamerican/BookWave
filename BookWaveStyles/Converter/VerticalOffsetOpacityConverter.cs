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
    public class VerticalOffsetOpacityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() == 2)
            {
                double sharedPageTitleHeight = double.Parse(values[0].ToString());
                double offset = double.Parse(values[1].ToString());

                return 1 - VerticalOffsetTopBarVisibilityConverter.CalcOpacityGradientOffset(offset, sharedPageTitleHeight);
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
