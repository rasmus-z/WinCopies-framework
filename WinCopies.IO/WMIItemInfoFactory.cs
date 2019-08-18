using System;
using System.Management;

namespace WinCopies.IO
{

    public class WMIItemInfoFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsReconstruction => !(Options is null);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new WMIItemInfoFactory((WMIItemInfoFactoryOptions)Options?.DeepClone(false), UseRecursively);

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
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public WMIItemInfoFactory() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class using custom options and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) : base() => _options = options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="WMIItemInfoFactory"/> to all the new objects created from the new <see cref="WMIItemInfoFactory"/>.</param>
        public WMIItemInfoFactory(bool useRecursively) : base(useRecursively) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class using custom options.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="WMIItemInfoFactory"/> to all the new objects created from the new <see cref="WMIItemInfoFactory"/>.</param>
        /// <param name="options">The options that this factory will use for creating new items.</param>
        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options, bool useRecursively) : base(useRecursively) => _options = options;

        /// <summary>
        /// Gets a new instance of the <see cref="WMIItemInfo"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="WMIItemInfo"/> class.</returns>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => GetBrowsableObjectInfo(UseRecursively ? (WMIItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementBaseObject> managementObject) => GetBrowsableObjectInfo(path, wmiItemType, managementObject, UseRecursively ? (WMIItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementScope> managementScope, Func<ManagementPath> managementPath) => GetBrowsableObjectInfo(path, wmiItemType, managementScope, managementPath, UseRecursively ? (WMIItemInfoFactory)DeepClone(false) : null);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(path, wmiItemType, UseRecursively ? (WMIItemInfoFactory)DeepClone(false) : null);

        /// <summary>
        /// Gets a new instance of the <see cref="WMIItemInfo"/> class using a custom <see cref="WMIItemInfoFactory"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="WMIItemInfo"/> class.</returns>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(WMIItemInfoFactory factory) => new WMIItemInfo(factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementBaseObject> managementObject, WMIItemInfoFactory factory) => new WMIItemInfo(path, wmiItemType, managementObject, null, factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementScope> managementScope, Func<ManagementPath> managementPath, WMIItemInfoFactory factory) => GetBrowsableObjectInfo(path, wmiItemType, () => new ManagementObject(managementScope(), managementPath(), _options?.ObjectGetOptions), factory);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, WMIItemInfoFactory factory) => GetBrowsableObjectInfo(path, wmiItemType, () => new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions), new ManagementPath(path), _options?.ObjectGetOptions), factory);

    }
}
