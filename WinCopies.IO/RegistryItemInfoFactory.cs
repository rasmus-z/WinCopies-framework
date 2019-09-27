using Microsoft.Win32;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory"/> class.
        /// </summary>
        public RegistryItemInfoFactory() : base() { }

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new RegistryItemInfoFactory();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new RegistryItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate) => new RegistryItemInfo(registryKey, registryKeyDelegate);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => new RegistryItemInfo(registryKeyPath);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName) => new RegistryItemInfo(registryKey, registryKeyDelegate, valueName);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => new RegistryItemInfo(registryKeyPath, valueName);

    }

}
