using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides common properties for <see cref="IBrowsableObjectInfo"/> factories.
    /// </summary>
    public interface IBrowsableObjectInfoFactory : IDeepCloneable

    {

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> associated to this <see cref="IBrowsableObjectInfoFactory"/>.
        /// </summary>
        IBrowsableObjectInfo Path { get; }

        /// <summary>
        /// Whether to add the current <see cref="IBrowsableObjectInfoFactory"/> to all the new objects created from this <see cref="IBrowsableObjectInfoFactory"/>.
        /// </summary>
        bool UseRecursively { get; set; }

        /// <summary>
        /// This method should only be used for registering a reference to <paramref name="path"/> in this <see cref="IBrowsableObjectInfoFactory"/> and should NOT be called directly, except in interface implementation.
        /// </summary>
        /// <param name="path">The <see cref="IBrowsableObjectInfo"/> to register.</param>
        void RegisterPath(IBrowsableObjectInfo path);

        /// <summary>
        /// This method should only be used for unregistering the reference made by the <see cref="RegisterPath(IBrowsableObjectInfo)"/> method to the path associated to this <see cref="IBrowsableObjectInfoFactory"/> and should NOT be called directly, except in interface implementation.
        /// </summary>
        void UnregisterPath();

    }

}
