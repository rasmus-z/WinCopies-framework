using System;
using System.Globalization;
using System.Windows;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class ShowItemsCheckBoxConverter : MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] == DependencyProperty.UnsetValue ? Visibility.Collapsed : (bool)values[0] && (bool)values[1] ? Visibility.Visible : Visibility.Collapsed;

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
