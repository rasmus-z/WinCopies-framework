using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public class RegistryItemInfoComparer : Comparer<IRegistryItemInfo>

    {

        private readonly FileSystemObjectComparer _fileSystemObjectComparer;

        public FileSystemObjectComparer FileSystemObjectComparer { get => _fileSystemObjectComparer; set => this.SetField(nameof(_fileSystemObjectComparer), value, typeof(RegistryItemInfoComparer), paramName: nameof(value), setOnlyIfNotNull: true, throwIfNull: true); }

        public RegistryItemInfoComparer() : this(BrowsableObjectInfo.GetDefaultComparer()) { }

        public RegistryItemInfoComparer(FileSystemObjectComparer fileSystemObjectComparer) => FileSystemObjectComparer = fileSystemObjectComparer;

        protected override int CompareOverride(IRegistryItemInfo x, IRegistryItemInfo y)
        {

            int result = GetIf(x.RegistryItemType, y.RegistryItemType, (RegistryItemType _x, RegistryItemType _y) => _x.CompareTo(_y), () => -1, () => 1, () => 0);

            return result == 0 ? FileSystemObjectComparer.Compare(x, y) : result;

        }

    }
}
