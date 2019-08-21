using System;
using System.Globalization;
using WinCopies.Collections;
using WinCopies.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{
    public class FileSystemObjectComparer<T> : Comparer<T>, IFileSystemObjectComparer<T> where T : IFileSystemObject

    {

        public virtual bool NeedsObjectsReconstruction => true; // True because of the StirngComparer property.

        protected virtual void OnDeepClone(FileSystemObjectComparer<T> fileSystemObjectComparer, bool preserveIds) { }

        protected virtual FileSystemObjectComparer<T> DeepCloneOverride(bool preserveIds) => new FileSystemObjectComparer<T>(_stringComparerDelegate);

        public object DeepClone(bool preserveIds)

        {

            FileSystemObjectComparer<T> fileSystemObjectComparer = DeepCloneOverride(preserveIds);

            OnDeepClone(fileSystemObjectComparer, preserveIds);

            return fileSystemObjectComparer;

        }

        private readonly Func<StringComparer> _stringComparerDelegate;

        public StringComparer StringComparer { get; }

        public FileSystemObjectComparer() : this(() => StringComparer.Create(CultureInfo.CurrentCulture, true)) { }

        public FileSystemObjectComparer(Func<StringComparer> stringComparerDelegate)
        {

            _stringComparerDelegate = stringComparerDelegate;

            StringComparer = stringComparerDelegate();

        }

        protected override int CompareOverride(T x, T y) => x.FileType == y.FileType || (x.FileType == FileType.File && (y.FileType == FileType.Link || y.FileType == FileType.Archive)) || (y.FileType == FileType.File && (x.FileType == FileType.Link || x.FileType == FileType.Archive))
                ? StringComparer.Compare(x.LocalizedName.RemoveAccents(), y.LocalizedName.RemoveAccents())
                : (x.FileType == FileType.Folder || x.FileType == FileType.Drive) && (y.FileType == FileType.File || y.FileType == FileType.Archive || y.FileType == FileType.Link)
                ? -1
                : (x.FileType == FileType.File || x.FileType == FileType.Archive || x.FileType == FileType.Link) && (y.FileType == FileType.Folder || y.FileType == FileType.Drive)
                ? 1
                : 0;

    }
}
