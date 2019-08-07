using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IWMIItemInfoFactoryOptions : ICloneable
    {

        IWMIItemInfoFactory Factory { get; }

        ConnectionOptions ConnectionOptions { get; set; }

        ObjectGetOptions ObjectGetOptions { get; set; }

        EnumerationOptions EnumerationOptions { get; set; }

    }

    public class WMIItemInfoFactoryOptions : IWMIItemInfoFactoryOptions
    {

        public IWMIItemInfoFactory Factory { get; internal set; }

        private ConnectionOptions _connectionOptions;

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        protected virtual ConnectionOptions ConnectionOptionsOverride
        {
            get => _connectionOptions; set =>

_connectionOptions = value;
        }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        public ConnectionOptions ConnectionOptions
        {
            get => ConnectionOptionsOverride; set
            {
                if (Factory?.Path?.ItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                ConnectionOptionsOverride = value;
            }
        }

        private ObjectGetOptions _objectGetOptions;

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        protected virtual ObjectGetOptions ObjectGetOptionsOverride { get => _objectGetOptions; set => _objectGetOptions = value; }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        public ObjectGetOptions ObjectGetOptions

        {

            get => ObjectGetOptionsOverride; set
            {
                if (Factory?.Path?.ItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                ObjectGetOptionsOverride = value;
            }

        }

        EnumerationOptions _enumerationOptions;

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        protected virtual EnumerationOptions EnumerationOptionsOverride { get => _enumerationOptions; set => _enumerationOptions = value; }

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        public virtual EnumerationOptions EnumerationOptions
        {
            get => EnumerationOptionsOverride; set
            {
                if (Factory?.Path?.ItemsLoader?.IsBusy == true) throw new InvalidOperationException($"The parent {nameof(IBrowsableObjectInfo.ItemsLoader)} is busy.");

                EnumerationOptionsOverride = value;
            }
        }

        public virtual object Clone()

        {

            var options = (WMIItemInfoFactoryOptions)MemberwiseClone();

            options.Factory = null;

            options._connectionOptions = (ConnectionOptions) options.ConnectionOptions.Clone();

            options._enumerationOptions = (EnumerationOptions) options.EnumerationOptions.Clone();

            options._objectGetOptions = (ObjectGetOptions) options.ObjectGetOptions.Clone();

            return options;

        }

    }
}
