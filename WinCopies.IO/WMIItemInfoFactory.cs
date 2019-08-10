using System;
using System.Management;

namespace WinCopies.IO
{

    public class WMIItemInfoFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
    {

        public override bool NeedsObjectsReconstruction => Options?.needs;

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new WMIItemInfoFactory(Options?.Clone());

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

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, Func<ManagementBaseObject> managementObject, WMIItemType wmiItemType) => new WMIItemInfo(path, managementObject, wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(managementScope, managementPath, _options?.ObjectGetOptions), wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions), new ManagementPath(path), _options?.ObjectGetOptions), wmiItemType);

    }
}
