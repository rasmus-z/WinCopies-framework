using SevenZip;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ArchiveItemInfo"/>s.
    /// </summary>
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate);

    }

    /// <summary>
    /// A factory for creating new <see cref="ShellObjectInfo"/>s.
    /// </summary>
    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory"/> class.
        /// </summary>
        public ArchiveItemInfoFactory() : base() { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) => new ArchiveItemInfo(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ArchiveItemInfoFactory();

    }

}
