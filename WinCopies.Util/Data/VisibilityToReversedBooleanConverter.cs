using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(Visibility), typeof(bool), ParameterType = typeof(Visibility))]
    public class VisibilityToReversedBooleanConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility)value == Visibility.Visible;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter != null && !(parameter is Visibility))

                // todo:

                throw new ArgumentException("parameter must be a value of the System.Windows.Visibility enum.");

            return (bool)value ? parameter ?? Visibility.Collapsed : Visibility.Visible;

        }
        
    }
}
