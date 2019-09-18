using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{
    public interface IRegistryItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(IRegistryItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, IRegistryItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, IRegistryItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName, IRegistryItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName, IRegistryItemInfoFactory factory);

    }
}
