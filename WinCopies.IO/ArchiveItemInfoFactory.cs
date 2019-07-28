using SevenZip;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

    }

    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

    }
}
