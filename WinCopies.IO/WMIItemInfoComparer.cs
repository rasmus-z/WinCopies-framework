using static WinCopies.Util.Util;
using WinCopies.Collections;
using WinCopies.Util;

namespace WinCopies.IO
{
    public class WMIItemInfoComparer : Comparer<IWMIItemInfo>

    {

        private readonly FileSystemObjectComparer _fileSystemObjectComparer;

        public FileSystemObjectComparer FileSystemObjectComparer { get => _fileSystemObjectComparer; set => this.SetField(nameof(_fileSystemObjectComparer), value, typeof(RegistryItemInfoComparer), paramName: nameof(value), setOnlyIfNotNull: true, throwIfNull: true); }

        public WMIItemInfoComparer() : this(BrowsableObjectInfo.GetDefaultComparer()) { }

        public WMIItemInfoComparer(FileSystemObjectComparer fileSystemObjectComparer) => FileSystemObjectComparer = fileSystemObjectComparer;

        protected override int CompareOverride(IWMIItemInfo x, IWMIItemInfo y)
        {

            int result = GetIf(x.WMIItemType, y.WMIItemType, (WMIItemType _x, WMIItemType _y) => _x.CompareTo(_y), () => -1, () => 1, () => 0);

            return result == 0 ? FileSystemObjectComparer.Compare(x, y) : result;

        }

    }

}
