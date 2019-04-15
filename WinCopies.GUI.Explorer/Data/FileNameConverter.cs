using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class FileNameConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == System.Windows.DependencyProperty.UnsetValue ? null : ((ShellObject)value).GetDisplayName(DisplayNameType.Default);
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
