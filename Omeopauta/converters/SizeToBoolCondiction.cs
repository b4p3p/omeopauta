using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Omeopauta.converters
{
    class SizeToBoolCondiction : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cond = (string)parameter;
            int limit = int.Parse(cond.Substring(1));
            int v = (int)value;

            if( cond.StartsWith(">"))
                return v > limit;
            else if (cond.StartsWith("="))
                return v = limit;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
