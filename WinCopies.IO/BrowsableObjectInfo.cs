﻿using System;
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

    public interface IBrowsableObjectInfoModifier<TParent, TItem> : System.IDisposable where TParent : IBrowsableObjectInfo where TItem : IBrowsableObjectInfo

    {

        TItem Item { get; }

        void SetParent(IBrowsableObjectInfoAccessor<TParent, TItem> parent, int index);

        void Reset(IBrowsableObjectInfoAccessor<TParent, TItem> parent, int index);

    }

    internal class BrowsableObjectInfoModifier<TParent, TItem> : IBrowsableObjectInfoModifier<TParent, TItem> where TParent : class, IBrowsableObjectInfo where TItem : BrowsableObjectInfo<TParent, TItem, IBrowsableObjectInfoFactory>

    {

        TItem IBrowsableObjectInfoModifier<TParent, TItem>.Item => (TItem)Item;

        public BrowsableObjectInfo<TParent, TItem, IBrowsableObjectInfoFactory> Item { get; internal set; }

        public BrowsableObjectInfoModifier(BrowsableObjectInfo<TParent, TItem, IBrowsableObjectInfoFactory> item) => Item = item;

        public void SetParent(IBrowsableObjectInfoAccessor<TParent, TItem> parent, int index)

        {

            if (object.ReferenceEquals(((IReadOnlyList<IBrowsableObjectInfo>)parent.ItemCollection)[index], Item))

                Item.Parent = parent.Owner;

            else throw new ArgumentException("Invalid owner collection.");

        }

        public void Reset(IBrowsableObjectInfoAccessor<TParent, TItem> parent, int index)

        {

            if (object.ReferenceEquals(parent.Owner, Item.Parent))

            {

                Item.Parent = default;

                Dispose();

            }

            else throw new ArgumentException("Invalid owner collection.");

        }

        public void Dispose() => Dispose(true);

        protected void Dispose(bool disposing)

        {

            if (disposing)

                Item = default;

        }

        ~BrowsableObjectInfoModifier() => Dispose(false);

    }

    /// <summary>
    /// The base class for all browsable items of the WinCopies framework.
    /// </summary>
    public abstract class BrowsableObjectInfo<TParent, TItems, TFactory> : FileSystemObject, IBrowsableObjectInfo<TParent, TItems, TFactory> where TParent : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory
    {

        internal static Icon TryGetIcon(int iconIndex, string dll, System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

        /// <summary>
        /// When overridden in a derived class, gets the small <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public abstract BitmapSource SmallBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the medium <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public abstract BitmapSource MediumBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public abstract BitmapSource LargeBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the extra large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public abstract BitmapSource ExtraLargeBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> is browsable.
        /// </summary>
        public abstract bool IsBrowsable { get; }

        IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

        private TFactory _factory;

        /// <summary>
        /// Gets or sets the factory for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> and its associated <see cref="ItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public TFactory Factory
        {

            get => _factory;

            set

            {

                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

                TFactory oldFactory = _factory;

                value.RegisterPath(this);

                _factory = value;

                oldFactory.UnregisterPath();

            }

        }

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> are currently loaded.
        /// </summary>
        public bool AreItemsLoaded { get; internal set; }

        // IBrowsableObjectInfoLoader<IBrowsableObjectInfo> IBrowsableObjectInfo.ItemsLoader => (IBrowsableObjectInfoLoader)ItemsLoader;

        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        /// <summary>
        /// Gets the items loader for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public IBrowsableObjectInfoLoader ItemsLoader { get; internal set; }

        IPathModifier IBrowsableObjectInfo.RegisterLoader(IBrowsableObjectInfoLoader itemsLoader)
        {

            if (object.ReferenceEquals(ItemsLoader, itemsLoader))

                throw new InvalidOperationException("This items loader is already registered.");

            ItemsLoader = object.ReferenceEquals(itemsLoader.Path, this) ? itemsLoader : throw new InvalidOperationException("Can not make a reference to the given items loader; the given items loader has to have registered the current path before calling the RegisterLoader method.");

            return new PathModifier<TParent, TItems, TFactory>(this);

        }

        void IBrowsableObjectInfo.UnregisterLoader()

        {

            if (object.ReferenceEquals(ItemsLoader.Path, this))

                throw new InvalidOperationException("Can not unregister the current items loader because it still references the current path. You need to unregister the current path from the current items loader before calling the UnregisterLoader method.");

            ItemsLoader = null;

        }

        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

        internal BrowsableObjectInfoCollection<BrowsableObjectInfo<TParent, TItems, TFactory>, TItems> items;

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public virtual IReadOnlyObservableCollection<TItems> Items { get; internal set; }

        protected virtual void OnItemsChanging(NotifyCollectionChangedEventArgs e)
        {

            if (e.NewItems != null)

                foreach (object item in e.NewItems)

                    if (item is TItems _browsableObjectInfo)

                        _browsableObjectInfo.Parent = Path;

        }

        private void ItemsChanging(object sender, NotifyCollectionChangedEventArgs e) => OnItemsChanging(e);

        private TParent _parent = default;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public virtual TParent Parent { get => _parent ?? (_parent = GetParent()); internal set => _parent = value; }

        IBrowsableObjectInfo IBrowsableObjectInfo.Parent => Parent;

        protected virtual IBrowsableObjectInfoCollection<TItems> ItemsInternal { get; }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposing.
        /// </summary>
        public bool IsDisposing { get; private set; }

        // private bool _considerAsPathRoot = false;

        // public bool ConsiderAsPathRoot { get => _considerAsPathRoot; set => OnPropertyChanged(nameof(ConsiderAsPathRoot), nameof(_considerAsPathRoot), value, typeof(BrowsableObjectInfo)); }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
        /// <param name="factory">The factory for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> and its associated <see cref="ItemsLoader"/>.</param>
        /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        protected BrowsableObjectInfo(string path, FileType fileType, TFactory factory) : base(path, fileType)

        {

            ThrowOnInvalidFactoryUpdateOperation(factory, nameof(factory));

            factory.UnregisterPath();

            _factory = factory;

            items = new ObservableCollection<IBrowsableObjectInfo>();

            Items = new ReadOnlyObservableCollection<IBrowsableObjectInfo>(items);

            ((INotifyCollectionChanging)Items).CollectionChanging -= ItemsChanging;

            ((INotifyCollectionChanging)Items).CollectionChanging += ItemsChanging;

        }

        /// <summary>
        /// Checks if an <see cref="IBrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> and throw an exception if the validation failed.
        /// </summary>
        /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> and in its associated <see cref="ItemsLoader"/>.</param>
        /// <param name="paramName">The parameter name to include in error messages.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newFactory"/> is null.</exception>
        protected virtual void ThrowOnInvalidFactoryUpdateOperation(IBrowsableObjectInfoFactory newFactory, string paramName)

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
        /// Loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public virtual void LoadItems()

        {

            if (ItemsLoader == null)

                LoadItems(true, true);

            else

                ItemsLoader.LoadItems();

        }

        /// <summary>
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItems(IBrowsableObjectInfoLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = itemsLoader;

            ItemsLoader.LoadItems();

        }

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> asynchronously.
        /// </summary>
        public virtual void LoadItemsAsync()

        {

            if (ItemsLoader == null)

                LoadItemsAsync(true, true);

            else

                ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public abstract void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader">A custom items loader.</param>
        public virtual void LoadItemsAsync(IBrowsableObjectInfoLoader itemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = itemsLoader;

            ItemsLoader.LoadItemsAsync();

        }

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</returns>
        protected abstract TParent GetParent();

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/> in memory.
        // /// </summary>

        // public abstract bool IsRenamingSupported { get; }

        ///// <summary>
        ///// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> with the specified name.
        ///// </summary>
        ///// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
        //public abstract void Rename(string newValue);

        /// <summary>
        /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        protected virtual void OnDeepClone(BrowsableObjectInfo< TParent, TItems, TFactory> browsableObjectInfo)

        {

            // browsableObjectInfo.AreItemsLoaded = false;

            if (!(ItemsLoader is null))

                browsableObjectInfo.ItemsLoader = (IBrowsableObjectInfoLoader)ItemsLoader.DeepClone();

            // browsableObjectInfo.SetItemsProperty();

            //if (Factory.UseRecursively)

            browsableObjectInfo.Factory = (TFactory)(IBrowsableObjectInfoFactory)browsableObjectInfo.Factory.DeepClone();

            // else

            // browsableObjectInfo._factory = null;

            // browsableObjectInfo._parent = null;

        }

        /// <summary>
        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. The <see cref="OnDeepClone(BrowsableObjectInfo{TParent, TItems, TFactory})"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool?)"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        protected abstract BrowsableObjectInfo< TParent, TItems, TFactory> DeepCloneOverride();

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</returns>
        public object DeepClone()

        {

            //var callee = new StackFrame(0).GetMethod();

            //var caller = new StackFrame(1).GetMethod();

            //if (callee.DeclaringType.Equals(caller.DeclaringType) || (caller.IsConstructor && caller.DeclaringType.BaseType.Equals(this.GetType())))

            //{

            ((IDisposable)this).ThrowIfDisposingOrDisposed();

            BrowsableObjectInfo<TFactory> browsableObjectInfo = DeepCloneOverride();

            OnDeepClone(browsableObjectInfo);

            return browsableObjectInfo;

            //}

            //    else

            //        throw new InvalidOperationException("The type of the caller of the current constructor is not the same as the type of this constructor.");

        }

        /// <summary>
        /// Disposes the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        public void Dispose() => Dispose(false, false, false, false);

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        /// <param name="disposeItemsLoader">Whether to dispose the items loader of the current path.</param>
        /// <param name="disposeParent">Whether to dispose the parent of the current path.</param>
        /// <param name="disposeItems">Whether to dispose the items of the current path.</param>
        /// <param name="recursively">Whether to dispose recursively.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected virtual void Dispose(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)

        {

            if (ItemsLoader != null)

            {

                if (ItemsLoader.IsBusy)

                    ItemsLoader.Cancel();

                if (disposeItemsLoader)

                    // todo: if disposing == false, this call is from the finalizer, so ItemsLoader.Dispose should also be able to be called with the false value for the disposing parameter

                    ItemsLoader.Dispose();

                // ItemsLoader.Path = null;

            }

            if (disposeParent && Parent != null)

                Parent.Dispose(disposeItemsLoader && recursively, recursively, disposeItems && recursively, recursively);

            if (disposing)

                Parent = null;

            if (disposeItems)

                while (items.Count > 0)
                {

                    Items[0].Dispose(disposeItemsLoader && recursively, false, recursively, recursively);

                    items.RemoveAt(0);

                }

            else if (disposing)

                items.Clear();

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

            Dispose(true, disposeItemsLoader, disposeParent, disposeItems, recursively);

            GC.SuppressFinalize(this);

            IsDisposed = true;

            IsDisposing = false;

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~BrowsableObjectInfo()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        {

            Dispose(false, false, false, false, false);

        }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public virtual bool NeedsObjectsOrValuesReconstruction => (!(ItemsLoader is null) && ItemsLoader.NeedsObjectsOrValuesReconstruction) || Factory.NeedsObjectsOrValuesReconstruction;
    }

}
