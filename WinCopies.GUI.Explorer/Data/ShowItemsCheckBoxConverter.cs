using System;
using System.Globalization;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class ShowItemsCheckBoxConverter : MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => (bool)values[0] && (bool)values[1] ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; 

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
