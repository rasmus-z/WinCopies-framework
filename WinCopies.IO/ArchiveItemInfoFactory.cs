using SevenZip;
using System;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ArchiveItemInfo"/>s.
    /// </summary>
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo);

    }

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>s.
    /// </summary>
    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ArchiveItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="ArchiveItemInfo"/> to all the new objects created from the new <see cref="ArchiveItemInfoFactory"/>.</param>
        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo) => GetBrowsableObjectInfo(path, fileType, archiveShellObject, archiveFileInfo, UseRecursively ? (ArchiveItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo, ArchiveItemInfoFactory factory) => new ArchiveItemInfo(path, fileType, archiveShellObject, archiveFileInfo, factory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new ArchiveItemInfoFactory(UseRecursively);

    }

}
