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
    public class VerticalOffsetTopBarVisibilityConverter : IMultiValueConverter
    {

        public static readonly double MAX_GRADIENT_OFFSET = 1;

        public static double CalcOpacityGradientOffset(double offset, double sharedPageTitleHeight)
        {
            double titleCoveredPercentage = offset / sharedPageTitleHeight / 0.75;

            return Math.Max(0, 1 - titleCoveredPercentage);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() == 2)
            {
                double sharedPageTitleHeight = double.Parse(values[0].ToString());
                double offset = double.Parse(values[1].ToString());

                return CalcOpacityGradientOffset(offset, sharedPageTitleHeight);
            }

            return 5;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
