using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IFileSystemObjectLoader : IBrowsableObjectInfoLoader
    {

        FileTypes FileTypes { get; set; }

    }

    public abstract class FileSystemObjectLoader<TPath, TItems, TFactory> : BrowsableObjectInfoLoader<TPath, TItems, TFactory>, IFileSystemObjectLoader where TPath : BrowsableObjectInfo<TItems, TFactory>, IFileSystemObjectInfo where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TFactory : BrowsableObjectInfoFactory
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<TPath, TItems, TFactory>), true); }

        protected FileSystemObjectLoader(TPath path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IFileSystemObject>()) { }

        protected FileSystemObjectLoader(TPath path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IFileSystemObject> browsableObjectInfoComparer) : base(path, workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

    }
}
