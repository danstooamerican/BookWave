using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BookWave.Styles.Converter
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
