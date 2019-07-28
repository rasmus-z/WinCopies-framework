using Microsoft.Win32;
using System;

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

    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
    {

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => UseCurrentFactoryRecursively ? new RegistryItemInfo(this) : new RegistryItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => UseCurrentFactoryRecursively ? new RegistryItemInfo(registryKey, this) :    new RegistryItemInfo(registryKey);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => UseCurrentFactoryRecursively ? new RegistryItemInfo(registryKeyPath, this) : new RegistryItemInfo(registryKeyPath);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => UseCurrentFactoryRecursively ? new RegistryItemInfo(registryKey, valueName, this) : new RegistryItemInfo(registryKey, valueName);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => UseCurrentFactoryRecursively ? new RegistryItemInfo(registryKeyPath, valueName, this) : new RegistryItemInfo(registryKeyPath, valueName);

    }
}
