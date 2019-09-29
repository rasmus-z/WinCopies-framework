using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //public static bool ValidatePropertySet(IBrowsableObjectInfoFactory factory) => factory.Loader?.IsBusy == false;

        //public static void ThrowOnInvalidPropertySet(IBrowsableObjectInfoFactory factory)

        //{

        //    if ( factory.Loader?.IsBusy == true)

        //        throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

        //}

        public IBrowsableObjectInfo Path { get; }

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
