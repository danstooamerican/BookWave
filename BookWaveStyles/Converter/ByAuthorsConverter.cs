using System;
using System.Globalization;
using System.Windows.Data;

namespace BookWave.Styles.Converter
{
    public class ByAuthorsConverter : IValueConverter
    {
        /// <summary>
        /// If the author string is not empty "by" is added as a prefix.
        /// </summary>
        /// <param name="value">author string</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>"by author string" or an empty string</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string authorString = string.Empty;

            if (value != null)
            {
                if (!value.ToString().Equals(string.Empty))
                {
                    authorString = "by " + value.ToString();
                }
            }

            return authorString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
