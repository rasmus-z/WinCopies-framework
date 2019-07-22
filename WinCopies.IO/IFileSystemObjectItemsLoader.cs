using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IFileSystemObjectItemsLoader : IBrowsableObjectInfoItemsLoader
    {

        FileTypes FileTypes { get; set; }

    }
}
