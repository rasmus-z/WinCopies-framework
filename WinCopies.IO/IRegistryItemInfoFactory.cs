using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IRegistryItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName);

    }
}
