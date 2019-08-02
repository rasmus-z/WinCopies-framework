using System;
using System.Globalization;
using System.IO;
using System.Windows;
using WinCopies.GUI.Explorer.Themes;
using WinCopies.Util.Data;
using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;

namespace WinCopies.GUI.Explorer.Data
{
    public class FileSystemInfoAttributesToVisibilityConverter : MultiConverterBase
    {

        private BooleanToVisibilityConverter booleanToVisibilityConverter = null;

        public BooleanToVisibilityConverter BooleanToVisibilityConverter

        {

            get => booleanToVisibilityConverter ?? (booleanToVisibilityConverter = Themes. Generic.BooleanToVisibilityConverter);

            set => booleanToVisibilityConverter = value;

        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => If(IfCT.Or, IfCM.Logical, IfComp.Equal, DependencyProperty.UnsetValue, values) ? null :

            BooleanToVisibilityConverter.Convert(values[0] is IO.ShellObjectInfo && ((ShellObjectInfo)values[0]).ShellObject.IsFileSystemObject
               ? !((FileAttributes)values[1]).HasFlag(FileAttributes.Hidden) && !((FileAttributes)values[1]).HasFlag(FileAttributes.System)
                   ? true
                   : ((FileAttributes)values[1]).HasFlag(FileAttributes.Hidden) && (bool)values[2]
                   ? (bool)values[2]
                       ? true
                       : ((IO.ShellObjectInfo)values[0]).FileType == IO.FileType.Drive
                       ? ((IO.ShellObjectInfo)values[0]).DriveInfoProperties.IsReady ? true : false
                       : false
                   : ((FileAttributes)values[1]).HasFlag(FileAttributes.System)
                   ? (bool)values[3]
                       ? true
                       : ((IO.ShellObjectInfo)values[0]).FileType == IO.FileType.Drive
                       ? ((IO.ShellObjectInfo)values[0]).DriveInfoProperties.IsReady ? true : false
                       : false
                   : false
               : true, targetType, parameter, culture);// else return Visibility.Visible;

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
