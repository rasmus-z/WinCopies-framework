using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{
    public interface IWMIItemInfoFactoryOptions : IDeepCloneable
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

        private readonly DeepClone<ConnectionOptions> _connectionOptionsDelegate;

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        protected virtual ConnectionOptions ConnectionOptionsOverride { get => _connectionOptions; set => _connectionOptions = value; }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        public ConnectionOptions ConnectionOptions
        {
            get => ConnectionOptionsOverride; set
            {

                if (!(Factory is null))

                    BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory.Path);

                ConnectionOptionsOverride = value;

            }
        }

        private ObjectGetOptions _objectGetOptions;

        private readonly DeepClone<ObjectGetOptions> _objectGetOptionsDelegate;

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        protected virtual ObjectGetOptions ObjectGetOptionsOverride
        {
            get => _objectGetOptions; set
            {

                if (!(Factory is null))

                    BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory.Path);

                _objectGetOptions = value;

            }
        }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
        public ObjectGetOptions ObjectGetOptions

        {

            get => ObjectGetOptionsOverride; set
            {

                if (!(Factory is null))

                    BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory.Path);

                ObjectGetOptionsOverride = value;

            }

        }

        private EnumerationOptions _enumerationOptions;

        private readonly DeepClone<EnumerationOptions> _enumerationOptionsDelegate;

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

                if (!(Factory is null))

                    BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory.Path);

                EnumerationOptionsOverride = value;

            }
        }

        public bool NeedsObjectsReconstruction => true;

        public WMIItemInfoFactoryOptions() : this(null, null, null) { }

        public WMIItemInfoFactoryOptions(DeepClone<ConnectionOptions> connectionOptions, DeepClone<ObjectGetOptions> objectGetOptions, DeepClone<EnumerationOptions> enumerationOptions)

        {

            _connectionOptionsDelegate = connectionOptions;

            _connectionOptions = connectionOptions?.Invoke(null);

            _objectGetOptionsDelegate = objectGetOptions;

            _objectGetOptions = objectGetOptions?.Invoke(null);

            _enumerationOptionsDelegate = enumerationOptions;

            _enumerationOptions = enumerationOptions?.Invoke(null);

        }

        protected virtual void OnDeepClone(WMIItemInfoFactoryOptions wMIItemInfoFactoryOptions, bool? preserveIds) { }

        protected virtual WMIItemInfoFactoryOptions DeepCloneOverride(bool? preserveIds) => new WMIItemInfoFactoryOptions(_connectionOptionsDelegate, _objectGetOptionsDelegate, _enumerationOptionsDelegate);

        public object DeepClone(bool? preserveIds)

        {

            WMIItemInfoFactoryOptions options = DeepCloneOverride(preserveIds);

            OnDeepClone(options, preserveIds);

            return options;

        }

    }
}
