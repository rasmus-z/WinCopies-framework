using System;
using System.Globalization;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(object), typeof(bool))] 
    public class IsNullConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is null;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
