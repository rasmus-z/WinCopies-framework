using Microsoft.Win32;
using System;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
    {

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => UseRecursively ? new RegistryItemInfo((RegistryItemInfoFactory)Clone()) : new RegistryItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => UseRecursively ? new RegistryItemInfo(registryKey, (RegistryItemInfoFactory)Clone()) : new RegistryItemInfo(registryKey);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => UseRecursively ? new RegistryItemInfo(registryKeyPath, (RegistryItemInfoFactory)Clone()) : new RegistryItemInfo(registryKeyPath);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => UseRecursively ? new RegistryItemInfo(registryKey, valueName, (RegistryItemInfoFactory)Clone()) : new RegistryItemInfo(registryKey, valueName);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => UseRecursively ? new RegistryItemInfo(registryKeyPath, valueName, (RegistryItemInfoFactory)Clone()) : new RegistryItemInfo(registryKeyPath, valueName);

    }

}
