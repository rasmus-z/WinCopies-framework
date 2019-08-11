using System;
using System.Management;

namespace WinCopies.IO
{

    public class WMIItemInfoFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
    {

        public override bool NeedsObjectsReconstruction => !(Options is null);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new WMIItemInfoFactory((WMIItemInfoFactoryOptions)Options?.DeepClone(false));

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

        public WMIItemInfoFactory() { }

        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) => _options = options;

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new WMIItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementBaseObject> managementObject) => new WMIItemInfo(path, wmiItemType, managementObject);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, Func<ManagementScope> managementScope, Func<ManagementPath> managementPath) => GetBrowsableObjectInfo(path, wmiItemType, () => new ManagementObject(managementScope(), managementPath(), _options?.ObjectGetOptions));

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(path, wmiItemType, () => new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions), new ManagementPath(path), _options?.ObjectGetOptions));

    }
}
