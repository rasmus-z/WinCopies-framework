using Microsoft.Win32;
using System;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public RegistryItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="RegistryItemInfoFactory"/> to all the new objects created from the new <see cref="RegistryItemInfoFactory"/>.</param>
        public RegistryItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new RegistryItemInfoFactory(UseRecursively);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => GetBrowsableObjectInfo(UseRecursively ? (RegistryItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => GetBrowsableObjectInfo(registryKey, UseRecursively ? (RegistryItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => GetBrowsableObjectInfo(registryKeyPath, UseRecursively ? (RegistryItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => GetBrowsableObjectInfo(registryKey, valueName, UseRecursively ? (RegistryItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => GetBrowsableObjectInfo(registryKeyPath, valueName, UseRecursively ? (RegistryItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryItemInfoFactory factory) => new RegistryItemInfo(factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, RegistryItemInfoFactory factory) => new RegistryItemInfo(registryKey, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, RegistryItemInfoFactory factory) => new RegistryItemInfo(registryKeyPath, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName, RegistryItemInfoFactory factory) => new RegistryItemInfo(registryKey, valueName, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName, RegistryItemInfoFactory factory) => new RegistryItemInfo(registryKeyPath, valueName, factory);

    }

}
