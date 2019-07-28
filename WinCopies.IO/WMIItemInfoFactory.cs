using System;
using System.Management;

namespace WinCopies.IO
{
    public interface IWMIItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IWMIItemInfoFactoryOptions Options { get; }

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

    }

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

    public interface IWMIItemInfoFactoryOptions
    {

        ConnectionOptions ConnectionOptions { get; set; }

        ObjectGetOptions ObjectGetOptions { get; set; }

        EnumerationOptions EnumerationOptions { get; set; }

    }

    public class WMIItemInfoFactoryOptions : IWMIItemInfoFactoryOptions
    {

        internal IBrowsableObjectInfoItemsLoader _wmiItemsLoader;

        private ConnectionOptions _connectionOptions;

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        protected virtual ConnectionOptions ConnectionOptionsOverride
        {
            get => _connectionOptions; set =>

_connectionOptions = value;
        }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        public ConnectionOptions ConnectionOptions
        {
            get => ConnectionOptionsOverride; set
            {
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                ConnectionOptionsOverride = value;
            }
        }

        private ObjectGetOptions _objectGetOptions;

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        protected virtual ObjectGetOptions ObjectGetOptionsOverride { get => _objectGetOptions; set => _objectGetOptions = value; }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        public ObjectGetOptions ObjectGetOptions

        {

            get => ObjectGetOptionsOverride; set
            {
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                ObjectGetOptionsOverride = value;
            }

        }

        EnumerationOptions _enumerationOptions;

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        protected virtual EnumerationOptions EnumerationOptionsOverride { get => _enumerationOptions; set => _enumerationOptions = value; }

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoItemsLoader"/> is busy.</exception>
        public virtual EnumerationOptions EnumerationOptions
        {
            get => EnumerationOptionsOverride; set
            {
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                EnumerationOptionsOverride = value;
            }
        }

    }
}
