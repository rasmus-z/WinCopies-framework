using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IWMIItemInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

    }

    public interface IWMIItemInfoFactory<TOptions> : IWMIItemInfoFactory where TOptions : IWMIItemInfoFactoryOptions
    {

        TOptions Options { get; set; }

    }

    public class WMIItemInfoFactory : IWMIItemInfoFactory<WMIItemInfoFactoryOptions>
    {

        internal IBrowsableObjectInfoItemsLoader _wmiItemsLoader;

        private WMIItemInfoFactoryOptions _options;

        protected virtual WMIItemInfoFactoryOptions OptionsOverride { get => _options; set => _options = value; }

        public WMIItemInfoFactoryOptions Options
        {
            get => OptionsOverride; set
            {

                if (_wmiItemsLoader?.IsBusy == true)

                    throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo<IBrowsableObjectInfoItemsLoader>.ItemsLoader)} is busy.");

                OptionsOverride = value;

            }
        }

        public WMIItemInfoFactory() { }

        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) => OptionsOverride = options;

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new WMIItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType) => new WMIItemInfo(managementObject, wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(managementScope, managementPath, OptionsOverride?.ObjectGetOptions), wmiItemType);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(new ManagementObject(new ManagementScope(path, OptionsOverride?.ConnectionOptions), new ManagementPath(path), OptionsOverride?.ObjectGetOptions), wmiItemType);

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
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo<IBrowsableObjectInfoItemsLoader>.ItemsLoader)} is busy.");

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
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo<IBrowsableObjectInfoItemsLoader>.ItemsLoader)} is busy.");

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
                if (_wmiItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo<IBrowsableObjectInfoItemsLoader>.ItemsLoader)} is busy.");

                EnumerationOptionsOverride = value;
            }
        }

    }
}
