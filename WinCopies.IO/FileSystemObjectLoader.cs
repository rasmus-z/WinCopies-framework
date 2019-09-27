using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IFileSystemObjectLoader : IBrowsableObjectInfoLoader
    {

        FileTypes FileTypes { get; set; }

    }

    public abstract class FileSystemObjectLoader<TPath, TItems, TSubItems, TFactory> : BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory>, IFileSystemObjectLoader where TPath : FileSystemObjectInfo where TItems : FileSystemObjectInfo where TSubItems : FileSystemObjectInfo where TFactory : BrowsableObjectInfoFactory
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<TPath, TItems, TSubItems, TFactory>), true); }

        protected FileSystemObjectLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IFileSystemObject>()) { }

        protected FileSystemObjectLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IFileSystemObject> browsableObjectInfoComparer) : base(path, workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

    }
}
