using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Omeopauta.converters
{
    class AddValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            double parameterValue;

            if (value != null && targetType == typeof(Double) &&
                double.TryParse((string)parameter,
                    NumberStyles.Integer, culture, 
                    out parameterValue))
            {
                result = (double)value + (double)parameterValue;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
