using SevenZip;
using System;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo);

    }

    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        public ArchiveItemInfoFactory() : this(false) { }

        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, Func<ArchiveFileInfo?> archiveFileInfo) =>

            UseRecursively ? new ArchiveItemInfo(path, fileType, archiveShellObject, archiveFileInfo, (ArchiveItemInfoFactory)DeepClone(false)) : new ArchiveItemInfo(path, fileType, archiveShellObject, archiveFileInfo);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new ArchiveItemInfoFactory(UseRecursively);

    }
}
