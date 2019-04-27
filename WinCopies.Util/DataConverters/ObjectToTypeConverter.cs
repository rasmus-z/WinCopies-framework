using System;
using System.Globalization;

namespace WinCopies.Util.DataConverters
{
    public class ObjectToTypeConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.GetType();

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
