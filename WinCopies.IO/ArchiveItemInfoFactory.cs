using SevenZip;
using System;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo, string path, FileType fileType);

    }

    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        public ArchiveItemInfoFactory() : this(false) { }

        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo, string path, FileType fileType) =>

            UseRecursively ? new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType, (ArchiveItemInfoFactory)DeepClone(false)) : new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new ArchiveItemInfoFactory(UseRecursively);

    }
}
