using SevenZip;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

    }

    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        public ArchiveItemInfoFactory() : this(false) { }

        public ArchiveItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) =>

            UseRecursively ? new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType, (ArchiveItemInfoFactory) Clone()) : new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

        public override object Clone() => new ArchiveItemInfoFactory(UseRecursively);

    }
}
