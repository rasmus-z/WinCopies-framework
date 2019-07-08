using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using WinCopies.Collections;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides info to interact with file system browsable items.
    /// </summary>
    public interface IBrowsableObjectInfo : IFileSystemObject, IDisposable
    {

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        BitmapSource SmallBitmapSource { get; }

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        BitmapSource MediumBitmapSource { get; }

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        BitmapSource LargeBitmapSource { get; }

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        BitmapSource ExtraLargeBitmapSource { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is browsable.
        /// </summary>
        bool IsBrowsable { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is disposing.
        /// </summary>
        bool IsDisposing { get; }

        /// <summary>
        /// Gets the items of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; }

        // todo: really needed? :

        /// <summary>
        /// Gets a value that indicates whether the items of this <see cref="IBrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        bool AreItemsLoaded { get; }

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="IBrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        IBrowsableObjectInfo Parent { get; }

        ///// <summary>
        ///// Gets or sets the items loader for this <see cref="IBrowsableObjectInfo"/>.
        ///// </summary>
        //IBrowsableObjectInfoItemsLoader ItemsLoader { get; set; }

        // IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        void LoadItems();

        void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader"></param>
        void LoadItems(BrowsableObjectInfoItemsLoader itemsLoader);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously.
        /// </summary>
        void LoadItemsAsync();

        void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader"></param>
        void LoadItemsAsync(BrowsableObjectInfoItemsLoader itemsLoader);

        void Rename(string newValue);

        // string ToString();

        IBrowsableObjectInfo Clone();

    }

}
