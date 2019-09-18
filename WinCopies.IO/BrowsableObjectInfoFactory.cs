using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides a base class for <see cref="BrowsableObjectInfo{TItems, TFactory}"/> factories.
    /// </summary>
    public abstract class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory

    {

        protected IList<IBrowsableObjectInfo> PathCollection { get; }

        public IReadOnlyList<IBrowsableObjectInfo> Paths { get; }

        private bool _useRecursively;

        /// <summary>
        /// Whether to add the current <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">On setting: At least one of the <see cref="Paths"/>'s <see cref="IBrowsableObjectInfo.ItemsLoader"/> of the current <see cref="BrowsableObjectInfoFactory"/> is busy.</exception>
        public bool UseRecursively
        {
            get => _useRecursively;

            set

            {

                ThrowOnInvalidPropertySet();

                _useRecursively = value;

            }
        }

        internal void ThrowOnInvalidPropertySet()

        {

            foreach (IBrowsableObjectInfo path in Paths)

                if (path?.ItemsLoader?.IsBusy == true)

                    throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

        }

        protected virtual IList<IBrowsableObjectInfo> GetNewPathCollection() => new Collection<IBrowsableObjectInfo>();

        protected virtual IReadOnlyList<IBrowsableObjectInfo> GetNewPathReadOnlyCollection() => new ReadOnlyCollection<IBrowsableObjectInfo>(PathCollection);

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class and sets the <see cref="UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        protected BrowsableObjectInfoFactory() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="BrowsableObjectInfoFactory"/>.</param>
        protected BrowsableObjectInfoFactory(bool useRecursively)
        {

            UseRecursively = useRecursively;

            PathCollection = GetNewPathCollection();

            Paths = GetNewPathReadOnlyCollection();

        }

        protected virtual void OnDeepClone(BrowsableObjectInfoFactory factory) { }

        protected abstract BrowsableObjectInfoFactory DeepCloneOverride();

        public virtual object DeepClone()

        {

            BrowsableObjectInfoFactory browsableObjectInfoFactory = DeepCloneOverride();

            OnDeepClone(browsableObjectInfoFactory);

            return browsableObjectInfoFactory;

        }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public virtual bool NeedsObjectsOrValuesReconstruction => false;

    }

}
