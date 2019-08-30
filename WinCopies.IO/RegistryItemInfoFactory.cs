using Microsoft.Win32;
using System;

namespace WinCopies.IO
{

    public class RegistryItemInfoFactory<TParent, TItems> : BrowsableObjectInfoFactory, IRegistryItemInfoFactory where TParent : class, IRegistryItemInfo where TItems : class, IRegistryItemInfo
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

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new RegistryItemInfoFactory<TParent, TItems>(UseRecursively);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => GetBrowsableObjectInfo(UseRecursively ? (RegistryItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => GetBrowsableObjectInfo(registryKey, UseRecursively ? (RegistryItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => GetBrowsableObjectInfo(registryKeyPath, UseRecursively ? (RegistryItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => GetBrowsableObjectInfo(registryKey, valueName, UseRecursively ? (RegistryItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => GetBrowsableObjectInfo(registryKeyPath, valueName, UseRecursively ? (RegistryItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IRegistryItemInfoFactory factory) => new RegistryItemInfo< TParent, TItems,    IRegistryItemInfoFactory>(factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, IRegistryItemInfoFactory factory) => new RegistryItemInfo< TParent, TItems, IRegistryItemInfoFactory>(registryKey, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, IRegistryItemInfoFactory factory) => new RegistryItemInfo< TParent, TItems, IRegistryItemInfoFactory>(registryKeyPath, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName, IRegistryItemInfoFactory factory) => new RegistryItemInfo< TParent, TItems, IRegistryItemInfoFactory>(registryKey, valueName, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName, IRegistryItemInfoFactory factory) => new RegistryItemInfo< TParent, TItems, IRegistryItemInfoFactory>(registryKeyPath, valueName, factory);

    }

}
