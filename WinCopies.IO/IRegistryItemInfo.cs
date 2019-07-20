using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IRegistryItemInfo : IBrowsableObjectInfo
    {

        RegistryItemType RegistryItemType { get; }

        RegistryKey RegistryKey { get; }

        IRegistryItemInfoFactory RegistryItemInfoFactory { get; set; }

    }
}
