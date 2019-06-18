using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Commons.Styles.Converter
{
    public class AddUpdateConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean to "Add" or "Update"
        /// </summary>
        /// <param name="value">is true when "Update", false when "Add"</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>"Update" if true, "Add" if false</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool b = (bool)value;
                if (b)
                {
                    return "Update";
                } else
                {
                    return "Add";
                }
            }
            return "Add";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
