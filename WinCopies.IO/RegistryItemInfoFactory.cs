using Microsoft.Win32;
using System;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
    {

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new RegistryItemInfoFactory();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => UseRecursively ? new RegistryItemInfo((RegistryItemInfoFactory)DeepClone(false)) : new RegistryItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => UseRecursively ? new RegistryItemInfo(registryKey, (RegistryItemInfoFactory)DeepClone(false)) : new RegistryItemInfo(registryKey);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => UseRecursively ? new RegistryItemInfo(registryKeyPath, (RegistryItemInfoFactory)DeepClone(false)) : new RegistryItemInfo(registryKeyPath);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => UseRecursively ? new RegistryItemInfo(registryKey, valueName, (RegistryItemInfoFactory)DeepClone(false)) : new RegistryItemInfo(registryKey, valueName);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => UseRecursively ? new RegistryItemInfo(registryKeyPath, valueName, (RegistryItemInfoFactory)DeepClone(false)) : new RegistryItemInfo(registryKeyPath, valueName);

    }

}
