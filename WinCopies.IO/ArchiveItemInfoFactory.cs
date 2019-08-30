using SevenZip;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ArchiveItemInfo{TParent, TItems, TFactory}"/>s.
    /// </summary>
    public interface IArchiveItemInfoFactory : IFileSystemObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo, IArchiveItemInfoFactory factory);

    }

    /// <summary>
    /// A factory for creating new <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/>s.
    /// </summary>
    public class ArchiveItemInfoFactory<TParent, TItems> : BrowsableObjectInfoFactory, IArchiveItemInfoFactory where TParent : class, IArchiveItemInfoProvider where TItems : class, IArchiveItemInfo
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory{TParent, TItems}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ArchiveItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TParent, TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="ArchiveItemInfo{TParent, TItems, TFactory}"/> to all the new objects created from the new <see cref="ArchiveItemInfoFactory{TParent, TItems}"/>.</param>
        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo) => GetBrowsableObjectInfo(path, fileType, archiveShellObject, archiveFileInfo, UseRecursively ? (ArchiveItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfo, IArchiveItemInfoFactory factory) => new ArchiveItemInfo< TParent, TItems, IArchiveItemInfoFactory>(path, fileType, archiveShellObject, archiveFileInfo, factory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ArchiveItemInfoFactory<TParent, TItems>(UseRecursively);

    }

}
