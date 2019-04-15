using System;
using System.Globalization;

namespace WinCopiesProcessesManager
{
    public class ObjectToTypeConverter : WinCopies.Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.GetType();

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
