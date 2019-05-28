using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using WinCopies.Util.Data;

namespace WinCopies.GUI.Explorer.Data
{

    public class FileSystemInfoConverter : ConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>

            //Debug.WriteLine("fsic: " + value.GetType().ToString());

            //Debug.WriteLine("fsic: " + (value is IO.ShellObjectInfo).ToString());

            value is IO.ShellObjectInfo;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
