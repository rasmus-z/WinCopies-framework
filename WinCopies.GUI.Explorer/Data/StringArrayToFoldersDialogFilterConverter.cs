using System;
using System.Globalization;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Windows.Dialogs.Data
{
    public class StringArrayToFoldersDialogFilterConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? (object)null : new FoldersBrowserDialogFilter() { Filter = ((string[])value)[0] };

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? null : new string[] { ((FoldersBrowserDialogFilter)value).Filter };
    }
}
