using System;
using System.Globalization;

namespace WinCopies.Util.DataConverters
{

    public class ReverseBooleanConverter : ConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return !(Boolean)value;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return !(Boolean)value;

        }

    }

}
