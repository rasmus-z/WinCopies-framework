using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IFileSystemObjectLoader<T> : IBrowsableObjectInfoLoader<T> where T : IBrowsableObjectInfo
    {

        FileTypes FileTypes { get; set; }

    }

    public abstract class FileSystemObjectLoader<T> : BrowsableObjectInfoLoader<T>, IFileSystemObjectLoader<T> where T : class, IBrowsableObjectInfo
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<T>), true); }

        protected FileSystemObjectLoader(T path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IFileSystemObject>()) { }

        protected FileSystemObjectLoader(T path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IFileSystemObject> browsableObjectInfoComparer) : base(path, workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

    }
}
