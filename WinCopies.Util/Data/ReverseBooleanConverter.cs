using System;
using System.Globalization;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBooleanConverter : ConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

    }

}
