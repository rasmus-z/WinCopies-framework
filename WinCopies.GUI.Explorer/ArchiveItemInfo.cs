using SevenZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.IO;

namespace WinCopies.GUI.Explorer
{
    public class ArchiveItemInfo : IO.ArchiveItemInfo, IBrowsableObjectInfo
    {
        public bool IsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBrowsableObjectInfo SelectedItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReadOnlyObservableCollection<IBrowsableObjectInfo> SelectedItems => throw new NotImplementedException();

        public bool IsCheckBoxEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ArchiveItemInfo(IO.ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType) : base(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType) { }

        public ArchiveItemInfo(IO.ArchiveItemInfo archiveItemInfo) : this(archiveItemInfo.ArchiveShellObject, archiveItemInfo.ArchiveFileInfo, archiveItemInfo.Path, archiveItemInfo.ArchiveItemRelativePath, archiveItemInfo.FileType) { }

        public override IO.IBrowsableObjectInfo GetBrowsableObjectInfo(IO.ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType) => new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType);
    }
}
