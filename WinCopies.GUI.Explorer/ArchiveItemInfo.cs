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
    public class ArchiveItemInfo : IO.ArchiveItemInfo, IBrowsableObjectInfo, IBrowsableObjectInfoInternal, IBrowsableObjectInfoHelper
    {
        private readonly bool isSelected = false;

        public bool IsSelected { get => isSelected; set => OnPropertyChanged(nameof(IsSelected), nameof(isSelected), value, typeof(ArchiveItemInfo)); }

        private readonly IBrowsableObjectInfo selectedItem = null;

        public IBrowsableObjectInfo SelectedItem { get => selectedItem; set => OnPropertyChanged(nameof(SelectedItem), nameof(selectedItem), value, typeof(ArchiveItemInfo)); }

        public ReadOnlyObservableCollection<IBrowsableObjectInfo> SelectedItems { get; internal set; } = null;

        ReadOnlyObservableCollection<IBrowsableObjectInfo> IBrowsableObjectInfoHelper.SelectedItems { set => SelectedItems = value; }

        private readonly bool isCheckBoxEnabled = false;

        public bool IsCheckBoxEnabled { get => isCheckBoxEnabled; set => OnPropertyChanged(nameof(IsCheckBoxEnabled), nameof(isCheckBoxEnabled), value, typeof(ArchiveItemInfo)); }

        ObservableCollection<IBrowsableObjectInfo> IBrowsableObjectInfoInternal.SelectedItems { get; set; } = new ObservableCollection<IBrowsableObjectInfo>();

        public ArchiveItemInfo(IO.ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileType fileType) : base(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType) => BrowsableObjectInfoHelper.Init(this);

        public ArchiveItemInfo(IO.ArchiveItemInfo archiveItemInfo) : this(archiveItemInfo.ArchiveShellObject, archiveItemInfo.ArchiveFileInfo, archiveItemInfo.Path, archiveItemInfo.ArchiveItemRelativePath, archiveItemInfo.FileType) => BrowsableObjectInfoHelper.Init(this);

        public override IO.IBrowsableObjectInfo GetBrowsableObjectInfo(IO.ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileType fileType) => new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType);
    }
}
