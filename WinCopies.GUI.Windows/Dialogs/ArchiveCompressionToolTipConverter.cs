using System;
using System.Globalization;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class ArchiveCompressionToolTipConverter : Util.Data.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool?)value == true ? Themes.Generic.GetResource<object>(string.Format("{0}Description", (string)parameter)) : null;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
