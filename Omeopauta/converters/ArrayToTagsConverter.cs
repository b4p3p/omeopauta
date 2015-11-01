using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Omeopauta.converters
{
    class ArrayToTagsConverter : IValueConverter
    {
        /// <summary>
        /// Converts from string to string[].
        /// </summary>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return String.Empty;
            string[] arrString = (string[])value;
            return string.Join(", ", arrString);
        }

        /// <summary>
        /// string[] -> string
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)value)) return new string[0];
            string[] res = ((string)value).Split(',');
            return (from string s in res
                    select s.Trim().ToUpper()).ToArray<string>();
        }
    }
}
