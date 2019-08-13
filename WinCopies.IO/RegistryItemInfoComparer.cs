using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public class RegistryItemInfoComparer<T> : FileSystemObjectComparer<T> where T : IRegistryItemInfo

    {

#pragma warning disable CS0649 // Set up using reflection
        private readonly IFileSystemObjectComparer<IFileSystemObject> _fileSystemObjectComparer;
#pragma warning restore CS0649

        public IFileSystemObjectComparer<IFileSystemObject> FileSystemObjectComparer { get => _fileSystemObjectComparer; set => this.SetField(nameof(_fileSystemObjectComparer), value, typeof(RegistryItemInfoComparer<T>), paramName: nameof(value), setOnlyIfNotNull: true, throwIfNull: true); }

        public RegistryItemInfoComparer() : this(BrowsableObjectInfo.GetDefaultComparer()) { }

        public RegistryItemInfoComparer(IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer) => FileSystemObjectComparer = fileSystemObjectComparer;

        protected override int CompareOverride( T x, T y)
        {

            int result = GetIf(x.RegistryItemType, y.RegistryItemType, (RegistryItemType _x, RegistryItemType _y) => _x.CompareTo(_y), () => -1, () => 1, () => 0);

            return result == 0 ? FileSystemObjectComparer.Compare(x, y) : result;

        }

    }
}
