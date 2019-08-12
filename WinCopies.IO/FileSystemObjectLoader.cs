using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IFileSystemObjectLoader<TPath> : IBrowsableObjectInfoLoader<TPath> where TPath : IBrowsableObjectInfo
    {

        FileTypes FileTypes { get; set; }

    }

    public abstract class FileSystemObjectLoader<TPath> : BrowsableObjectInfoLoader<TPath>, IFileSystemObjectLoader<TPath> where TPath : BrowsableObjectInfo
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<TPath>), true); }

        protected FileSystemObjectLoader(TPath path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer()) { }

        protected FileSystemObjectLoader(TPath path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer browsableObjectInfoComparer) : base(path, workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

    }
}
