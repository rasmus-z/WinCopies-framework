using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WinCopies.Util.Data;

namespace WinCopies.GUI.Explorer.Data
{
    public class ShowItemsCheckBoxConverter : MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue &&  (SelectionMode)values[0] != SelectionMode.Single && (bool)values[1];

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
