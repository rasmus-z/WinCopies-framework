using Microsoft.Win32;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory<T> : BrowsableObjectInfoFactory, IRegistryItemInfoFactory where T : BrowsableObjectInfo, IRegistryItemInfo
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory{T}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public RegistryItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory{T}"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="RegistryItemInfoFactory{T}"/> to all the new objects created from the new <see cref="RegistryItemInfoFactory{T}"/>.</param>
        public RegistryItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new RegistryItemInfoFactory<T>(UseRecursively);

        // todo: xmldoc: invalidcast

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => GetBrowsableObjectInfo(UseRecursively ? (RegistryItemInfoFactory<T>)DeepClone() : default);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate) => GetBrowsableObjectInfo(registryKey, registryKeyDelegate, UseRecursively ? (RegistryItemInfoFactory<T>)DeepClone() : default);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => GetBrowsableObjectInfo(registryKeyPath, UseRecursively ? (RegistryItemInfoFactory<T>)DeepClone() : default);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName) => GetBrowsableObjectInfo(registryKey, registryKeyDelegate, valueName, UseRecursively ? (RegistryItemInfoFactory<T>)DeepClone() : default);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => GetBrowsableObjectInfo(registryKeyPath, valueName, UseRecursively ? (RegistryItemInfoFactory<T>)DeepClone() : default);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo( IRegistryItemInfoFactory factory) => new RegistryItemInfo<T, IRegistryItemInfoFactory>(factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, IRegistryItemInfoFactory factory) => new RegistryItemInfo<T, IRegistryItemInfoFactory>(registryKey, registryKeyDelegate, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, IRegistryItemInfoFactory factory) => new RegistryItemInfo<T, IRegistryItemInfoFactory>(registryKeyPath, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName, IRegistryItemInfoFactory factory) => new RegistryItemInfo<T, IRegistryItemInfoFactory>(registryKey, registryKeyDelegate, valueName, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName, IRegistryItemInfoFactory factory) => new RegistryItemInfo<T, IRegistryItemInfoFactory>(registryKeyPath, valueName, factory);

    }

}
