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
    public class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory

    {

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> associated to this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        public IBrowsableObjectInfo Path { get; internal set; }

        private readonly bool _useCurrentFactoryRecursively;

        /// <summary>
        /// Whether to add the current <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from this <see cref="BrowsableObjectInfoFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">On setting: The <see cref="Path"/>'s <see cref="IBrowsableObjectInfo.ItemsLoader"/> of the current <see cref="BrowsableObjectInfoFactory"/> is busy.</exception>
        public bool UseCurrentFactoryRecursively
        {
            get => _useCurrentFactoryRecursively; set

            {

                if (Path?.ItemsLoader?.IsBusy == true)

                    throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class and sets the <see cref="UseCurrentFactoryRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public BrowsableObjectInfoFactory() : this(true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="useRecursively">Whether to add the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="IBrowsableObjectInfoFactory"/>.</param>
        public BrowsableObjectInfoFactory(bool useRecursively) => _useCurrentFactoryRecursively = useRecursively;

    }

}
