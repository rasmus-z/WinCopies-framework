using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static FileSystemObjectComparer GetDefaultComparer() => new FileSystemObjectComparer();

        public virtual int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

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
        /// The file type of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual FileType FileType { get; private set; } = FileType.Other;

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

        IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

        private BrowsableObjectInfoFactory _factory;

        /// <summary>
        /// Gets or sets the factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="ItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
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
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        [DefaultValue(false)]
        public bool AreItemsLoaded { get; internal set; }

        IBrowsableObjectInfoLoader<IBrowsableObjectInfo> IBrowsableObjectInfo.ItemsLoader => (IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)ItemsLoader;

        internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        /// <summary>
        /// Gets the items loader for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public BrowsableObjectInfoLoader<BrowsableObjectInfo> ItemsLoader { get; internal set; }

        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        internal ObservableCollection<IBrowsableObjectInfo> items;

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; internal set; }

#pragma warning disable IDE0069 // Disposed in the Dispose() method.
        private IBrowsableObjectInfo _parent = null;
#pragma warning restore IDE0069

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public virtual IBrowsableObjectInfo Parent { get => _parent ?? (_parent = GetParent()); internal set => _parent = value; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is disposing.
        /// </summary>
        public bool IsDisposing { get; private set; }

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

            SetItemsProperty();

        }

        private void SetItemsProperty()

        {

            items = new ObservableCollection<IBrowsableObjectInfo>();

            Items = new ReadOnlyObservableCollection<IBrowsableObjectInfo>(items);

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

            if (ItemsLoader?.IsBusy == true)

                throw new InvalidOperationException($"The {nameof(ItemsLoader)} is busy.");

            if (newFactory is null)

                throw new ArgumentNullException(paramName);

            if (!(newFactory.Path is null))

                throw new InvalidOperationException("The given factory has already been added to a BrowsableObjectInfo.");

        }

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
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItems(IBrowsableObjectInfoLoader<IBrowsableObjectInfo> itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = GetOrThrowIfNotType<BrowsableObjectInfoLoader<BrowsableObjectInfo>>(itemsLoader, nameof(itemsLoader));

            ItemsLoader.LoadItems();

        }

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
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItemsAsync(IBrowsableObjectInfoLoader<IBrowsableObjectInfo> itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = GetOrThrowIfNotType<BrowsableObjectInfoLoader<BrowsableObjectInfo>>(itemsLoader, nameof(itemsLoader));

            ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="BrowsableObjectInfo"/>.</returns>
        protected abstract IBrowsableObjectInfo GetParent();

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo"/> in memory.
        // /// </summary>

        public abstract bool IsRenamingSupported { get; }

        /// <summary>
        /// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo"/> with the specified name.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo"/>.</param>
        public abstract void Rename(string newValue);

        /// <summary>
        /// When overridden in a derived class, gets a new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>. How
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public virtual IBrowsableObjectInfo Clone()

        {

            if (IsDisposing)

                throw new InvalidOperationException("The current BrowsableObjectInfo is disposing.");

            if (IsDisposed)

                throw new ObjectDisposedException("The current BrowsableObjectInfo is disposed.");

            var browsableObjectInfo = (BrowsableObjectInfo)MemberwiseClone();

            browsableObjectInfo.AreItemsLoaded = false;

            browsableObjectInfo.ItemsLoaderInternal = null;

            browsableObjectInfo.SetItemsProperty();

            if (browsableObjectInfo.Factory.UseRecursively)

                browsableObjectInfo.Factory = (BrowsableObjectInfoFactory)browsableObjectInfo.Factory.Clone();

            else

                browsableObjectInfo._factory = null;

            browsableObjectInfo._parent = null;

            return browsableObjectInfo;

        }

        public virtual bool Equals(IFileSystemObject fileSystemObject) => Equals((object)fileSystemObject);

        /// <summary>
        /// Determines whether the specified object is equal to the current object by testing the following things, in order: whether <paramref name="obj"/> implements the <see cref="IBrowsableObjectInfo"/> interface and both objects references, and <see cref="FileType"/> and <see cref="Path"/> properties are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => ReferenceEquals(this, obj)
                ? true : obj is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
                : false;

        /// <summary>
        /// Gets an hash code for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The hash codes of the <see cref="FileType"/> and the <see cref="Path"/> property.</returns>
        public override int GetHashCode() => FileType.GetHashCode() ^ Path.ToLower().GetHashCode();

        /// <summary>
        /// Gets a string representation of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="BrowsableObjectInfo"/>.</returns>
        public override string ToString() => IsNullEmptyOrWhiteSpace(LocalizedName) ? Path : LocalizedName;

        /// <summary>
        /// Disposes the current <see cref="BrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public void Dispose() => Dispose(true, true, true, true);

        protected virtual void DisposeOverride(bool disposeItemsLoader, bool disposeItems, bool disposeParent, bool recursively)

        {

            if (ItemsLoader?.IsBusy == true)

                ItemsLoader.Cancel();

            if (disposeItemsLoader && ItemsLoader != null)

            {

                ItemsLoader.Dispose(false);

                ItemsLoader.Path = null;

            }

            if (disposeParent && Parent != null)

            {

                Parent.Dispose(disposeItemsLoader, disposeItems, recursively, recursively);

                Parent = null;

            }

            if (disposeItems)

                while (items.Count > 0)
                {

                    Items[0].Dispose(disposeItemsLoader, recursively, false, recursively);

                    items.RemoveAt(0);

                }

        }

        public void Dispose(bool disposeItemsLoader, bool disposeItems, bool disposeParent, bool recursively)

        {

            IsDisposing = true;

            DisposeOverride(disposeItemsLoader, disposeItems, disposeParent, recursively);

            IsDisposed = true;

            IsDisposing = false;

        }

        public bool IsDisposed { get; private set; }

    }

}
