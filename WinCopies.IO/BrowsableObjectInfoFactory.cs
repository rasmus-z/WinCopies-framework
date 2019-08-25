using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides a base class for <see cref="BrowsableObjectInfo{T}"/> factories.
    /// </summary>
    public abstract class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory

    {

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> associated to this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        public IBrowsableObjectInfo Path { get; private set; }

        void IBrowsableObjectInfoFactory.RegisterPath(IBrowsableObjectInfo path)
        {

            if (object.ReferenceEquals(Path, path))

                throw new InvalidOperationException("This path is already registered.");

            Path = object.ReferenceEquals(path.Factory, this) ? path : throw new InvalidOperationException("Can not make a reference to the given path; the given path has to have registered the current factory before calling the RegisterPath method.");

        }

        void IBrowsableObjectInfoFactory.UnregisterPath()

        {

            if (object.ReferenceEquals(Path.Factory, this))

                throw new InvalidOperationException("Can not unregister the current path because it still references the current factory. You need to unregister the current factory from the current path before calling the UnregisterPath method.");

            Path = null;

        }

        private bool _useRecursively;

        /// <summary>
        /// Whether to add the current <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">On setting: The <see cref="Path"/>'s <see cref="IBrowsableObjectInfo.ItemsLoader"/> of the current <see cref="BrowsableObjectInfoFactory"/> is busy.</exception>
        public bool UseRecursively
        {
            get => _useRecursively; set

            {

                ThrowOnInvalidPropertySet(Path);

                _useRecursively = value;

            }
        }

        internal static void ThrowOnInvalidPropertySet(IBrowsableObjectInfo path)

        {

            if (path?.ItemsLoader?.IsBusy == true)

                throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class and sets the <see cref="UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        protected BrowsableObjectInfoFactory() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="BrowsableObjectInfoFactory"/>.</param>
        protected BrowsableObjectInfoFactory(bool useRecursively) => UseRecursively = useRecursively;

        protected virtual void OnDeepClone(BrowsableObjectInfoFactory factory, bool? preserveIds) { }

        protected abstract BrowsableObjectInfoFactory DeepCloneOverride(bool? preserveIds);

        public virtual object DeepClone(bool? preserveIds)

        {

            BrowsableObjectInfoFactory browsableObjectInfoFactory = DeepCloneOverride(preserveIds);

            OnDeepClone(browsableObjectInfoFactory, preserveIds);

            return browsableObjectInfoFactory;

        }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public virtual bool NeedsObjectsReconstruction => false;

    }

}
