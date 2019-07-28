using System;
using System.Drawing;
using System.Windows.Media.Imaging;

using TsudaKageyu;

using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides info to interact with any browsable items.
    /// </summary>
    public abstract class BrowsableObjectInfo : IBrowsableObjectInfo
    {

        internal static Icon TryGetIcon(int iconIndex, string dll, System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

        /// <summary>
        /// Gets the path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual string Path { get; }

        /// <summary>
        /// When overridden in a derived class, gets the localized path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string LocalizedName { get; }

        /// <summary>
        /// When overridden in a derived class, gets the name of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// When overridden in a derived class, gets the small <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource SmallBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the medium <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource MediumBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource LargeBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the extra large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource ExtraLargeBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is browsable.
        /// </summary>
        public abstract bool IsBrowsable { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is disposing.
        /// </summary>
        public bool IsDisposing { get; private set; }

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        public virtual bool AreItemsLoaded { get; internal set; }

        internal readonly ObservableCollection<IBrowsableObjectInfo> items = new ObservableCollection<IBrowsableObjectInfo>();

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; } = null;

        private IBrowsableObjectInfo _parent = null;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public virtual IBrowsableObjectInfo Parent
        {
            get => _parent ?? (_parent = GetParent());

            internal set => _parent = value;
        }

        /// <summary>
        /// The file type of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual FileType FileType { get; private set; } = FileType.Other;

        // private bool _considerAsPathRoot = false;

        // public bool ConsiderAsPathRoot { get => _considerAsPathRoot; set => OnPropertyChanged(nameof(ConsiderAsPathRoot), nameof(_considerAsPathRoot), value, typeof(BrowsableObjectInfo)); }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
        public BrowsableObjectInfo(string path, FileType fileType)

        {

            Path = path;

            FileType = fileType;

            Items = new ReadOnlyObservableCollection<IBrowsableObjectInfo>(items);

        }

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="BrowsableObjectInfo"/>.</returns>
        protected abstract IBrowsableObjectInfo GetParent();

        // protected abstract IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileTypes fileType);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual void LoadItems()

        {

            if (ItemsLoader == null)

                LoadItems(true, true);

            else

                ItemsLoader.LoadItems();

        }

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItems(IBrowsableObjectInfoItemsLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = GetOrThrowIfNotType<BrowsableObjectInfoItemsLoader>(itemsLoader, nameof(itemsLoader));

            ItemsLoader.LoadItems();

        }

        /// <summary>
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously.
        /// </summary>
        public virtual void LoadItemsAsync()

        {

            if (ItemsLoader == null)

                LoadItemsAsync(true, true);

            else

                ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItemsAsync(IBrowsableObjectInfoItemsLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = GetOrThrowIfNotType<BrowsableObjectInfoItemsLoader>(itemsLoader, nameof(itemsLoader));

            ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo"/> in memory.
        // /// </summary>

        /// <summary>
        /// Disposes the current <see cref="BrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public virtual void Dispose() => Dispose(false, true);

        /// <summary>
        /// Disposes the current <see cref="BrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposeItemsLoader">Whether to dispose the <see cref="ItemsLoader"/>s of the current path and its parent and items. If this parameter is set to <see langword="true"/>, the <see cref="ItemsLoader"/>s will also be disposed recursively.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public virtual void Dispose(bool disposeItemsLoader) => Dispose(disposeItemsLoader, true);

        private void Dispose(bool disposeItemsLoader, bool disposeParentBrowsableObjectInfo)

        {

            IsDisposing = true;

            if (ItemsLoader?.IsBusy == true)

                ItemsLoader.Cancel();

            if (disposeItemsLoader && ItemsLoader != null)

                ItemsLoader.Dispose(false);

            if (disposeParentBrowsableObjectInfo && Parent != null)

                Parent.Dispose(disposeItemsLoader);

            foreach (IBrowsableObjectInfo browsableObjectInfo in Items)

                browsableObjectInfo.Dispose(disposeItemsLoader);

            IsDisposing = false;

        }

        /// <summary>
        /// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo"/> with the specified name.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo"/>.</param>
        public abstract void Rename(string newValue);

        /// <summary>
        /// Gets a string representation of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="BrowsableObjectInfo"/>.</returns>
        public override string ToString() => IsNullEmptyOrWhiteSpace(LocalizedName) ? Path : LocalizedName;

        /// <summary>
        /// When overridden in a derived class, gets a new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public abstract IBrowsableObjectInfo Clone();

        /// <summary>
        /// Determines whether the specified object is equal to the current object by testing the following things, in order: whether <b>obj</b> implements the <see cref="IBrowsableObjectInfo"/> interface and both objects references, and <see cref="FileType"/> and <see cref="Path"/> properties are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is IBrowsableObjectInfo _obj
                ? ReferenceEquals(this, obj) ? true : FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
                : false;

        /// <summary>
        /// Gets an hash code for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The hash codes of the <see cref="FileType"/> and the <see cref="Path"/> property.</returns>
        public override int GetHashCode() => FileType.GetHashCode() ^ Path.ToLower().GetHashCode();

        IBrowsableObjectInfoItemsLoader IBrowsableObjectInfo.ItemsLoader => ItemsLoader;

        private BrowsableObjectInfoItemsLoader _itemsLoader;

        /// <summary>
        /// Gets or sets the items loader for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfoItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        public BrowsableObjectInfoItemsLoader ItemsLoader
        {
            get => _itemsLoader; set
            {

                if (ItemsLoader?.IsBusy == true)

                    throw new InvalidOperationException("The items loader is busy.");

                if (!(value.Path is null))

                    throw new InvalidOperationException("The given items loader has already been added to a BrowsableObjectInfo.");

                _itemsLoader.Path = null;

                value.Path = this;

                _itemsLoader = value;

            }

        }

        IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

        private BrowsableObjectInfoFactory _factory;

        /// <summary>
        /// Gets or sets the factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfoItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        protected BrowsableObjectInfoFactory Factory
        {

            get => _factory;

            set

            {

                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

                _factory.Path = null;

                value.Path = this;

                _factory = value;

            }

        }

        /// <summary>
        /// Checks if a <see cref="BrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo"/> and throw an exception if the validation failed.
        /// </summary>
        /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo"/> and in its associated <see cref="ItemsLoader"/>.</param>
        /// <param name="paramName">The parameter name to include in error messages.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newFactory"/> is null.</exception>
        protected virtual void ThrowOnInvalidFactoryUpdateOperation(BrowsableObjectInfoFactory newFactory, string paramName)

        {

            if (_itemsLoader?.IsBusy == true)

                throw new InvalidOperationException($"The {nameof(ItemsLoader)} is busy.");

            if (newFactory is null)

                throw new ArgumentNullException(paramName);

            if (!(newFactory.Path is null))

                throw new InvalidOperationException("The given factory has already been added to a BrowsableObjectInfo.");

        }

    }

}
