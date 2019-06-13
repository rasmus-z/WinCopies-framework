using System;
using System.Globalization;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    public class EnumToReversedBooleanConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !value.Equals(parameter);
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value ? parameter : Binding.DoNothing;
    }
}
