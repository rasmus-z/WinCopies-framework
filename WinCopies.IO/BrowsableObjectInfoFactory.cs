using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides a base class for <see cref="BrowsableObjectInfo"/> factories.
    /// </summary>
    public abstract class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory

    {

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> associated to this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        public IBrowsableObjectInfo Path { get; internal set; }

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

        protected virtual void OnDeepClone(BrowsableObjectInfoFactory factory, bool preserveIds) { }

        protected abstract BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds);

        public virtual object DeepClone(bool preserveIds)

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
