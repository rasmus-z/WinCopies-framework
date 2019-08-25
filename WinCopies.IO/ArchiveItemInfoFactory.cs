using SevenZip;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ArchiveItemInfo{T}"/>s.
    /// </summary>
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo, IArchiveItemInfoFactory factory);

    }

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo{T}"/>s.
    /// </summary>
    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ArchiveItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{T}"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="ArchiveItemInfo{T}"/> to all the new objects created from the new <see cref="ArchiveItemInfoFactory"/>.</param>
        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo) => GetBrowsableObjectInfo(path, fileType, archiveShellObject, archiveFileInfo, UseRecursively ? (ArchiveItemInfoFactory)DeepClone(null) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo, IArchiveItemInfoFactory factory) => new ArchiveItemInfo<IArchiveItemInfoFactory>(path, fileType, archiveShellObject, archiveFileInfo, factory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool? preserveIds) => new ArchiveItemInfoFactory(UseRecursively);

    }

}
