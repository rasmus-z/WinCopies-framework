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

    public abstract class BrowsableObjectInfo : FileSystemObject, IBrowsableObjectInfo
    {

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo{TItems, TFactory}"/> class.
        /// </summary>
        protected BrowsableObjectInfo(string path) : base(path) { }

        public abstract bool NeedsObjectsOrValuesReconstruction { get; }

        /// <summary>
        /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
        protected virtual void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)

        {

            // browsableObjectInfo.AreItemsLoaded = false;

            if (!(ItemsLoader is null))

                browsableObjectInfo.ItemsLoader = (IBrowsableObjectInfoLoader)ItemsLoader.DeepClone();

            // browsableObjectInfo.SetItemsProperty();

            //if (Factory.UseRecursively)

            // else

            // browsableObjectInfo._factory = null;

            // browsableObjectInfo._parent = null;

        }

        /// <summary>
        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        protected abstract BrowsableObjectInfo DeepCloneOverride();

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public object DeepClone()

        {

            //var callee = new StackFrame(0).GetMethod();

            //var caller = new StackFrame(1).GetMethod();

            //if (callee.DeclaringType.Equals(caller.DeclaringType) || (caller.IsConstructor && caller.DeclaringType.BaseType.Equals(this.GetType())))

            //{

            ((IDisposable)this).ThrowIfDisposingOrDisposed();

            BrowsableObjectInfo browsableObjectInfo = DeepCloneOverride();

            OnDeepClone(browsableObjectInfo);

            return browsableObjectInfo;

            //}

            //    else

            //        throw new InvalidOperationException("The type of the caller of the current constructor is not the same as the type of this constructor.");

        }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected virtual void Dispose(bool disposing)

        {

            if (ItemsLoader != null)

            {

                if (ItemsLoader.IsBusy)

                    ItemsLoader.Cancel();

                // ItemsLoader.Path = null;

            }

            if (disposing)

                Parent = null;

        }

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

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        public bool AreItemsLoaded { get; internal set; }

        /// <summary>
        /// Gets the items loader for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public IBrowsableObjectInfoLoader ItemsLoader { get; internal set; }

        public bool HasParent { get; internal set; }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposing.
        /// </summary>
        public bool IsDisposing { get; internal set; }

        private IBrowsableObjectInfo _parent = default;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public IBrowsableObjectInfo Parent { get { if (HasParent) return _parent; else _parent = GetParent(); HasParent = true; return _parent; } internal set => _parent = value; }

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
        public virtual void LoadItems(IBrowsableObjectInfoLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject));

            ItemsLoader = itemsLoader;

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
        public virtual void LoadItemsAsync(IBrowsableObjectInfoLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject));

            ItemsLoader = itemsLoader;

            ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="BrowsableObjectInfo"/>.</returns>
        protected abstract IBrowsableObjectInfo GetParent();

        /// <summary>
        /// Gets a value that indicates whether the current object is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        public void Dispose()

        {

            IsDisposing = true;

            Dispose(true);

            GC.SuppressFinalize(this);

            IsDisposed = true;

            IsDisposing = false;

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~BrowsableObjectInfo()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        {

            Dispose(false);

        }

    }

    ///// <summary>
    ///// The base class for all browsable items of the WinCopies framework.
    ///// </summary>
    //public abstract class BrowsableObjectInfo<TItems, TFactory> : BrowsableObjectInfo, IBrowsableObjectInfo<TItems, TFactory> where TItems : BrowsableObjectInfo where TFactory : BrowsableObjectInfoFactory
    //{

    //    // IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

    //    private TFactory _factory;

    //    /// <summary>
    //    /// Gets or sets the factory for this <see cref="BrowsableObjectInfo{TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo{TItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.
    //    /// </summary>
    //    /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo.ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</exception>
    //    /// <exception cref="ArgumentNullException">value is null.</exception>
    //    public TFactory Factory
    //    {

    //        get => _factory;

    //        set

    //        {

    //            ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

    //            _factory = value;

    //        }

    //    }

    //    // IBrowsableObjectInfoLoader<IBrowsableObjectInfo> IBrowsableObjectInfo.ItemsLoader => (IBrowsableObjectInfoLoader)ItemsLoader;

    //    // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

    //    //IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo> IBrowsableObjectInfo.RegisterLoader(IBrowsableObjectInfoLoader itemsLoader)
    //    //{

    //    //    if (object.ReferenceEquals(ItemsLoader, itemsLoader))

    //    //        throw new InvalidOperationException("This items loader is already registered.");

    //    //    ItemsLoader = object.ReferenceEquals(itemsLoader.Path, this) ? itemsLoader : throw new InvalidOperationException("Can not make a reference to the given items loader; the given items loader has to have registered the current path before calling the RegisterLoader method.");

    //    //    return (IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo>) new PathModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor< BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this));

    //    //}

    //    //IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems> IBrowsableObjectInfo<TItems, TFactory, TParentItems>.RegisterLoader(IBrowsableObjectInfoLoader itemsLoader)
    //    //{

    //    //    if (object.ReferenceEquals(ItemsLoader, itemsLoader))

    //    //        throw new InvalidOperationException("This items loader is already registered.");

    //    //    ItemsLoader = object.ReferenceEquals(itemsLoader.Path, this) ? itemsLoader : throw new InvalidOperationException("Can not make a reference to the given items loader; the given items loader has to have registered the current path before calling the RegisterLoader method.");

    //    //    return (IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems>)(IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo>)new PathModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this));

    //    //}

    //    //void IBrowsableObjectInfo.UnregisterLoader()

    //    //{

    //    //    if (object.ReferenceEquals(ItemsLoader.Path, this))

    //    //        throw new InvalidOperationException("Can not unregister the current items loader because it still references the current path. You need to unregister the current path from the current items loader before calling the UnregisterLoader method.");

    //    //    ItemsLoader = null;

    //    //}

    //    // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

    //    protected internal BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems> ItemCollection { get; }

    //    /// <summary>
    //    /// Gets the items of this <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.
    //    /// </summary>
    //    public ReadOnlyBrowsableObjectInfoCollection<TItems> Items { get; }

    //    IReadOnlyBrowsableObjectInfoCollection<TItems> IBrowsableObjectInfo<TItems, TFactory>.Items => Items;

    //    /// <summary>
    //    /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo{TItems, TFactory}"/> class.
    //    /// </summary>
    //    /// <param name="path">The path of this <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</param>
    //    /// <param name="factory">The factory for this <see cref="BrowsableObjectInfo{TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo{TItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
    //    /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</exception>
    //    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
    //    // /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</param>
    //    protected BrowsableObjectInfo(string path, TFactory factory) : base(path)

    //    {

    //        ThrowOnInvalidFactoryUpdateOperation(factory, nameof(factory));

    //        // factory.UnregisterPath();

    //        _factory = factory;

    //        ItemCollection = GetNewItemCollection();

    //        // ItemCollection.RegisterOwner((IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems>)new PathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this)));

    //        Items = GetNewItemReadOnlyCollection() ; 

    //    }

    //    protected virtual BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems> GetNewItemCollection() => (BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems>)new BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems>(this);

    //    protected virtual ReadOnlyBrowsableObjectInfoCollection<TItems> GetNewItemReadOnlyCollection() => new ReadOnlyBrowsableObjectInfoCollection<TItems>(ItemCollection);

    //    /// <summary>
    //    /// Checks if an <see cref="IBrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo{TItems, TFactory}"/> and throw an exception if the validation failed.
    //    /// </summary>
    //    /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo{TItems, TFactory}"/> and in its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
    //    /// <param name="paramName">The parameter name to include in error messages.</param>
    //    /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</exception>
    //    /// <exception cref="ArgumentNullException"><paramref name="newFactory"/> is null.</exception>
    //    protected virtual void ThrowOnInvalidFactoryUpdateOperation(IBrowsableObjectInfoFactory newFactory, string paramName)

    //    {

    //        if (ItemsLoader?.IsBusy == true)

    //            throw new InvalidOperationException($"The {nameof(ItemsLoader)} is busy.");

    //        if (newFactory is null)

    //            throw new ArgumentNullException(paramName);

    //    }

    //    // protected abstract IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileTypes fileType);

    //    // /// <summary>
    //    // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> in memory.
    //    // /// </summary>

    //    // public abstract bool IsRenamingSupported { get; }

    //    ///// <summary>
    //    ///// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> with the specified name.
    //    ///// </summary>
    //    ///// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
    //    //public abstract void Rename(string newValue);

    //    protected override void Dispose(bool disposing)
    //    {

    //        base.Dispose(disposing);

    //        if (disposing)

    //            ItemCollection.Clear();

    //    }

    //    /// <summary>
    //    /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
    //    /// </summary>
    //    public override bool NeedsObjectsOrValuesReconstruction => (!(ItemsLoader is null) && ItemsLoader.NeedsObjectsOrValuesReconstruction) || Factory.NeedsObjectsOrValuesReconstruction;

    //    protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
    //    {

    //        base.OnDeepClone(browsableObjectInfo);

    //        var _browsableObjectInfo = (BrowsableObjectInfo<TItems, TFactory>)browsableObjectInfo;

    //        _browsableObjectInfo.Factory = (TFactory)(IBrowsableObjectInfoFactory)_browsableObjectInfo.Factory.DeepClone();

    //    }
    //    //protected virtual IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> GetModifier() => (IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems>)new BrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this);

    //    //public void AddTo(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Add(GetModifier());

    //    //public void InsertTo(int index, IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Insert(index, GetModifier());

    //    //public void RemoveFrom(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Remove(this);

    //    //public void AddTo(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Add((IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo>)GetModifier());

    //    //public void InsertTo(int index, IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Insert(index, (IBrowsableObjectInfoModifier< BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo>)GetModifier());

    //    //public void RemoveFrom(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Remove(this);

    //}

}
