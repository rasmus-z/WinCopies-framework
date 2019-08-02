using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IWMIItemInfoFactoryOptions
    {

        ConnectionOptions ConnectionOptions { get; set; }

        ObjectGetOptions ObjectGetOptions { get; set; }

        EnumerationOptions EnumerationOptions { get; set; }

    }

    public class WMIItemInfoFactoryOptions : IWMIItemInfoFactoryOptions
    {

        internal IBrowsableObjectInfoLoader<IWMIItemInfo> _wmiItemsLoader;

        private ConnectionOptions _connectionOptions;

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
        protected virtual ConnectionOptions ConnectionOptionsOverride
        {
            get => _connectionOptions; set =>

_connectionOptions = value;
        }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
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
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
        protected virtual ObjectGetOptions ObjectGetOptionsOverride { get => _objectGetOptions; set => _objectGetOptions = value; }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
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
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
        protected virtual EnumerationOptions EnumerationOptionsOverride { get => _enumerationOptions; set => _enumerationOptions = value; }

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader"/> is busy.</exception>
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
