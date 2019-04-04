using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util.DataConverters
{
    public class PercentConverter : MultiConverterBase 
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) parameter = 100; 

            return (int)values[0] / (int)values[1] * (int) parameter; 
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
