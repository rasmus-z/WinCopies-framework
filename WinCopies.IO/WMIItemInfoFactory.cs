using System;
using System.Management;

namespace WinCopies.IO
{
    

    public class WMIItemInfoFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
    {

        private WMIItemInfoFactoryOptions _options;

        public WMIItemInfoFactoryOptions Options
        {
            get => _options; set
            {

                if (Path?.ItemsLoader?.IsBusy == true)

                    throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                _options = value;

            }

        }

        IWMIItemInfoFactoryOptions IWMIItemInfoFactory.Options => _options;

        public WMIItemInfoFactory() { }

        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) => _options = options;

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new WMIItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType) => new WMIItemInfo(managementObject, wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(managementScope, managementPath, _options?.ObjectGetOptions), wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions), new ManagementPath(path), _options?.ObjectGetOptions), wmiItemType);

    }
}
