using System;
using System.Management;
using WinCopies.Util;

namespace WinCopies.IO
{

    public class WMIItemInfoFactory<TParent, TItems> : BrowsableObjectInfoFactory, IWMIItemInfoFactory where TParent : class, IWMIItemInfo where TItems : class, IWMIItemInfo
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsOrValuesReconstruction => !(Options is null);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new WMIItemInfoFactory<TParent, TItems>((WMIItemInfoFactoryOptions)Options?.DeepClone(), UseRecursively);

        private WMIItemInfoFactoryOptions _options;

        public WMIItemInfoFactoryOptions Options
        {

            get => _options; set
            {

                ThrowOnInvalidPropertySet(Path);

                _options.Factory = null;

                value.Factory = this;

                _options = value;

            }

        }

        IWMIItemInfoFactoryOptions IWMIItemInfoFactory.Options => _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory{TParent, TItems}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public WMIItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory{TParent, TItems}"/> class using custom options and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) : base() => _options = options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory{TParent, TItems}"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="WMIItemInfoFactory{TParent, TItems}"/> to all the new objects created from the new <see cref="WMIItemInfoFactory{TParent, TItems}"/>.</param>
        public WMIItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory{TParent, TItems}"/> class using custom options.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="WMIItemInfoFactory{TParent, TItems}"/> to all the new objects created from the new <see cref="WMIItemInfoFactory{TParent, TItems}"/>.</param>
        /// <param name="options">The options that this factory will use for creating new items.</param>
        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options, bool useRecursively) : base(useRecursively) => _options = options;

        /// <summary>
        /// Gets a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class.</returns>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => GetBrowsableObjectInfo(UseRecursively ? (WMIItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate) => GetBrowsableObjectInfo(path, wmiItemType, managementObject, managementObjectDelegate, UseRecursively ? (WMIItemInfoFactory<TParent, TItems>)DeepClone() : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(path, wmiItemType, UseRecursively ? (WMIItemInfoFactory<TParent, TItems>)DeepClone() : null);

        /// <summary>
        /// Gets a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class using a custom <see cref="IWMIItemInfoFactory"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class.</returns>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IWMIItemInfoFactory factory) => new WMIItemInfo<TParent, TItems, IWMIItemInfoFactory>(factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate, IWMIItemInfoFactory factory) => new WMIItemInfo<TParent, TItems, IWMIItemInfoFactory>(path, wmiItemType, managementObject, managementObjectDelegate, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, IWMIItemInfoFactory factory) => GetBrowsableObjectInfo(path, wmiItemType, new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions is null ? null : WMIItemInfo.DefaultConnectionOptionsDeepClone(_options?.ConnectionOptions, null)), new ManagementPath(path), _options?.ObjectGetOptions is null ? null : WMIItemInfo.DefaultObjectGetOptionsDeepClone(_options?.ObjectGetOptions)), _managementObject => _managementObject is ManagementClass managementClass ? WMIItemInfo.DefaultManagementClassDeepCloneDelegate(managementClass, null) : _managementObject is ManagementObject __managementObject ? WMIItemInfo.DefaultManagementObjectDeepClone(__managementObject, null) : throw new ArgumentException("The given object must be a ManagementClass or a ManagementObject.", "managementObject"), factory);
    }
}
