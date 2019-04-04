using System;
using System.Globalization;
using System.Windows.Controls;
using WinCopies.Util.DataConverters;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Explorer.Data
{
    public class BooleanToSelectionModeConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? SelectionMode.Extended : (object)SelectionMode.Single;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => If(ComparisonType.Or, Comparison.Equals, (SelectionMode)value, SelectionMode.Multiple, SelectionMode.Extended) ? true : false;
    }
}
