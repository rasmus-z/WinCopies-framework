using SevenZip;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ArchiveItemInfo{TItems, TFactory}"/>s.
    /// </summary>
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate, IArchiveItemInfoFactory factory);

    }

    /// <summary>
    /// A factory for creating new <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>s.
    /// </summary>
    public class ArchiveItemInfoFactory<TItems> : BrowsableObjectInfoFactory, IArchiveItemInfoFactory where TItems : BrowsableObjectInfo , IArchiveItemInfo
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory{TItems}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ArchiveItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="ArchiveItemInfo{TItems, TFactory}"/> to all the new objects created from the new <see cref="ArchiveItemInfoFactory{TItems}"/>.</param>
        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) => GetBrowsableObjectInfo(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate, UseRecursively ? (ArchiveItemInfoFactory<TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate, IArchiveItemInfoFactory factory) => new ArchiveItemInfo<TItems, IArchiveItemInfoFactory>(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate, factory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ArchiveItemInfoFactory<TItems>(UseRecursively);

    }

}
