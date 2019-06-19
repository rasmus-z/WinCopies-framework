using System;
using System.Globalization;
using WinCopies.IO;
using ShellObjectInfo = WinCopies.GUI.Explorer.ShellObjectInfo;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class FolderBrowserDialogCommandButtonContentConverter : Util.Data.MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values[0] == System.Windows.DependencyProperty.UnsetValue) return null;



            Explorer.IBrowsableObjectInfo selectedItem = (Explorer.IBrowsableObjectInfo)values[0];



            if (selectedItem != null && (selectedItem.FileType == FileType.Folder || (selectedItem.FileType == FileType.SpecialFolder && selectedItem is IShellObjectInfo so && so.ShellObject.IsFileSystemObject) || selectedItem.FileType == FileType.Drive))

                return Explorer.Themes.Generic.Open;



            switch ((FolderBrowserDialogMode)values[1])
            {

                case FolderBrowserDialogMode.OpenFiles:

                    return Explorer.Themes. Generic.Open;

                case FolderBrowserDialogMode.OpenFolder:

                    return GUI.Themes.Generic.Cancel;

                case FolderBrowserDialogMode.Save:

                    return Explorer.Themes.Generic.Save;

                default:

                    return null;

            }

        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
