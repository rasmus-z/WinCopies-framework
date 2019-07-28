using SevenZip;

namespace WinCopies.IO
{
    public interface IArchiveItemInfo : IBrowsableObjectInfo, IArchiveItemInfoProvider
    {

        ArchiveFileInfo? ArchiveFileInfo { get; }

    }
}
