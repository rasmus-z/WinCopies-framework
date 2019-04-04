using System;
using System.Globalization;
using System.Windows.Data;

namespace WinCopies.GUI.Windows.Dialogs
{
    [ValueConversion(typeof(string), typeof(System.Windows.Visibility))]
    public class ErrorLabelVisibilityConverter : Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            // string _value = (string)value;

            string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value)
                ? System.Windows.Visibility.Collapsed
                : (object)System.Windows.Visibility.Visible;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
