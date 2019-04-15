using System;
using System.Globalization;
using WinCopies.IO;
using IBrowsableObjectInfo = WinCopies.GUI.Explorer.IBrowsableObjectInfo;
using ShellObjectInfo = WinCopies.GUI.Explorer.ShellObjectInfo;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class FoldersBrowserDialogCommandButtonContentConverter : Util.DataConverters.MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values[0] == System.Windows.DependencyProperty.UnsetValue) return null;

            IBrowsableObjectInfo selectedItem = (IBrowsableObjectInfo)values[0];



            if (selectedItem != null && (selectedItem.FileType == FileType.Folder || (selectedItem.FileType == FileType.SpecialFolder && selectedItem is ShellObjectInfo so && so.ShellObject.IsFileSystemObject) || selectedItem.FileType == FileType.Drive))

                return Explorer.Themes.Generic.Open;



            switch ((FoldersBrowserDialogMode)values[1])
            {

                case FoldersBrowserDialogMode.OpenFiles:

                    return Explorer.Themes.Generic.Open;

                case FoldersBrowserDialogMode.OpenFolder:

                    return ResourcesHelper.Cancel;

                case FoldersBrowserDialogMode.Save:

                    return Explorer.Themes.Generic.Save;

                default:

                    return null;

            }

        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
