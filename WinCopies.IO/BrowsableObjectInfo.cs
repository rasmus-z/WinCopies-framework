using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;

using TsudaKageyu;

using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{

    /// <summary>
    /// The base class for all browsable items of the WinCopies framework.
    /// </summary>
    public abstract class BrowsableObjectInfo : FileSystemObject, IBrowsableObjectInfo
    {

        internal static Icon TryGetIcon(int iconIndex, string dll, System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

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
        /// <exception cref="InvalidOperationException">The old <see cref="ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
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
        public bool AreItemsLoaded { get; internal set; }

        IBrowsableObjectInfoLoader<IBrowsableObjectInfo> IBrowsableObjectInfo.ItemsLoader => (IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)ItemsLoader;

        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        /// <summary>
        /// Gets the items loader for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public BrowsableObjectInfoLoader<BrowsableObjectInfo> ItemsLoader { get; internal set; }

        internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        internal ObservableCollection<IBrowsableObjectInfo> items;

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; internal set; }

        private IBrowsableObjectInfo _parent = null;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public virtual IBrowsableObjectInfo Parent { get => _parent ?? (_parent = GetParent()); internal set => _parent = value; }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposing.
        /// </summary>
        public bool IsDisposing { get; private set; }

        // private bool _considerAsPathRoot = false;

        // public bool ConsiderAsPathRoot { get => _considerAsPathRoot; set => OnPropertyChanged(nameof(ConsiderAsPathRoot), nameof(_considerAsPathRoot), value, typeof(BrowsableObjectInfo)); }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="factory">The factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="ItemsLoader"/>.</param>
        /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        protected BrowsableObjectInfo(string path, FileType fileType, BrowsableObjectInfoFactory factory) : base(path, fileType)

        {

            Factory = factory;

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

        // public abstract bool IsRenamingSupported { get; }

        ///// <summary>
        ///// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo"/> with the specified name.
        ///// </summary>
        ///// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo"/>.</param>
        //public abstract void Rename(string newValue);

        /// <summary>
        /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool)"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        protected virtual void OnDeepClone(BrowsableObjectInfo browsableObjectInfo, bool preserveIds)

        {

            // browsableObjectInfo.AreItemsLoaded = false;

            browsableObjectInfo.ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)ItemsLoader.DeepClone(preserveIds);

            // browsableObjectInfo.SetItemsProperty();

            //if (Factory.UseRecursively)

            browsableObjectInfo.Factory = (BrowsableObjectInfoFactory)browsableObjectInfo.Factory.DeepClone(preserveIds);

            // else

            // browsableObjectInfo._factory = null;

            // browsableObjectInfo._parent = null;

        }

        /// <summary>
        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo, bool)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool)"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        protected abstract BrowsableObjectInfo DeepCloneOverride(bool preserveIds);

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public object DeepClone(bool preserveIds)

        {

            //var callee = new StackFrame(0).GetMethod();

            //var caller = new StackFrame(1).GetMethod();

            //if (callee.DeclaringType.Equals(caller.DeclaringType) || (caller.IsConstructor && caller.DeclaringType.BaseType.Equals(this.GetType())))

            //{

            ((IDisposable)this).ThrowIfDisposingOrDisposed();

            BrowsableObjectInfo browsableObjectInfo = DeepCloneOverride(preserveIds);

            OnDeepClone(browsableObjectInfo, preserveIds);

            return browsableObjectInfo;

            //}

            //    else

            //        throw new InvalidOperationException("The type of the caller of the current constructor is not the same as the type of this constructor.");

        }

        /// <summary>
        /// Disposes the current <see cref="BrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        public void Dispose() => Dispose(true, true, true, true);

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        /// <param name="disposeItemsLoader">Whether to dispose the items loader of the current path.</param>
        /// <param name="disposeParent">Whether to dispose the parent of the current path.</param>
        /// <param name="disposeItems">Whether to dispose the items of the current path.</param>
        /// <param name="recursively">Whether to dispose recursively.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected virtual void DisposeOverride(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)

        {

            if (ItemsLoader != null)

            {

                if (ItemsLoader.IsBusy)

                    ItemsLoader.Cancel();

                if (disposeItemsLoader)

                {

                    ItemsLoader.Dispose(false);

                    ItemsLoader.Path = null;

                }

            }

            if (disposeParent && Parent != null)

            {

                Parent.Dispose(disposeItemsLoader && recursively, recursively, disposeItems && recursively, recursively);

                Parent = null;

            }

            if (disposeItems)

                while (items.Count > 0)
                {

                    Items[0].Dispose(disposeItemsLoader && recursively, false, recursively, recursively);

                    items.RemoveAt(0);

                }

        }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposeItemsLoader">Whether to dispose the items loader of the current path.</param>
        /// <param name="disposeParent">Whether to dispose the parent of the current path.</param>
        /// <param name="disposeItems">Whether to dispose the items of the current path.</param>
        /// <param name="recursively">Whether to dispose recursively.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        public void Dispose(bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)

        {

            IsDisposing = true;

            DisposeOverride(true, disposeItemsLoader, disposeParent, disposeItems, recursively);

            GC.SuppressFinalize(this);

            IsDisposed = true;

            IsDisposing = false;

        }

#pragma warning disable CS1591
        ~BrowsableObjectInfo()
#pragma warning restore CS1591

        {

            DisposeOverride(false, false, false, false, false);

        }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public virtual bool NeedsObjectsReconstruction => (!(ItemsLoader is null) && ItemsLoader.NeedsObjectsReconstruction) || Factory.NeedsObjectsReconstruction;
    }

}
