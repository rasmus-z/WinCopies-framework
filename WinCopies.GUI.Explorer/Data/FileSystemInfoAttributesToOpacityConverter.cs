using System;
using System.Globalization;
using System.IO;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class FileSystemInfoAttributesToOpacityConverter : MultiConverterBase
    {

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] is IO.ShellObjectInfo && ((ShellObjectInfo)values[0]).ShellObject.IsFileSystemObject
                ? ((FileAttributes)values[1]).HasFlag(FileAttributes.Hidden) && (bool)values[2]
                    ? ((IO.ShellObjectInfo)values[0]).FileType == IO.FileType.Drive
                        ? ((IO.ShellObjectInfo)values[0]).DriveInfoProperties.IsReady ? 1.0 : (object)0.5
                        : 0.5
                    : ((FileAttributes)values[1]).HasFlag(FileAttributes.System) && (bool)values[3]
                    ? ((IO.ShellObjectInfo)values[0]).FileType == IO.FileType.Drive
                        ? ((IO.ShellObjectInfo)values[0]).DriveInfoProperties.IsReady ? 1.0 : (object)0.5
                        : 0.5
                    : 1.0
                : 1.0;

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
