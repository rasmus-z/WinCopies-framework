using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

    }

    public class ArchiveItemInfoFactory : IArchiveItemInfoFactory
    {

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

    }
}
