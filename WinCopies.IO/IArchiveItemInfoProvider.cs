namespace WinCopies.IO
{
    public interface IArchiveItemInfoProvider : IBrowsableObjectInfo

    {

        // IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

        IArchiveItemInfoFactory Factory { get; }

        IShellObjectInfo ArchiveShellObject { get; }

    }
}
