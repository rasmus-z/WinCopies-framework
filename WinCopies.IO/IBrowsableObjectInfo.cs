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

        IBrowsableObjectInfoItemsLoader ItemsLoader { get; }

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

        // IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        void LoadItems();

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        void LoadItems(IBrowsableObjectInfoItemsLoader itemsLoader);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously.
        /// </summary>
        void LoadItemsAsync();

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        void LoadItemsAsync(IBrowsableObjectInfoItemsLoader itemsLoader);

        /// <summary>
        /// Renames or move to a relative path, or both, the current <see cref="IBrowsableObjectInfo"/> with the specified name.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="IBrowsableObjectInfo"/>.</param>
        void Rename(string newValue);

        // string ToString();

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="IBrowsableObjectInfo"/>.</returns>
        IBrowsableObjectInfo Clone();

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposeItemsLoader">Whether to dispose the <see cref="ItemsLoader"/>s of the current path and its parent and items. If this parameter is set to <see langword="true"/>, the <see cref="ItemsLoader"/>s will also be disposed recursively.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        void Dispose(bool disposeItemsLoader);

    }

}
