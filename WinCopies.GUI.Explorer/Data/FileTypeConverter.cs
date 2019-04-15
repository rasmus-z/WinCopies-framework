using System;
using System.Globalization;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class FileTypeConverter : MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] == System.Windows.DependencyProperty.UnsetValue ? null : (string)values[0] + " / " + (string)values[1];
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
