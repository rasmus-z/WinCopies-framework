using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IArchiveItemInfo : IBrowsableObjectInfo, IArchiveItemInfoProvider
    {

        ArchiveFileInfo? ArchiveFileInfo { get; }

        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; set; }
    }
}
