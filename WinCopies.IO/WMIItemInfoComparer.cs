using static WinCopies.Util.Util;
using WinCopies.Collections;
using WinCopies.Util;

namespace WinCopies.IO
{
    public class WMIItemInfoComparer<T> : FileSystemObjectComparer<T> where T : IWMIItemInfo

    {

        private readonly FileSystemObjectComparer<IFileSystemObject> _fileSystemObjectComparer;

        public FileSystemObjectComparer<IFileSystemObject> FileSystemObjectComparer { get => _fileSystemObjectComparer; set => this.SetField(nameof(_fileSystemObjectComparer), value, typeof(WMIItemInfoComparer<T>), paramName: nameof(value), setOnlyIfNotNull: true, throwIfNull: true); }

        public WMIItemInfoComparer() : this(BrowsableObjectInfo.GetDefaultComparer()) { }

        public WMIItemInfoComparer(FileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer) => FileSystemObjectComparer = fileSystemObjectComparer;

        protected override int CompareOverride(T x, T y)
        {

            int result = GetIf(x.WMIItemType, y.WMIItemType, (WMIItemType _x, WMIItemType _y) => _x.CompareTo(_y), () => -1, () => 1, () => 0);

            return result == 0 ? FileSystemObjectComparer.Compare(x, y) : result;

        }

    }

}
