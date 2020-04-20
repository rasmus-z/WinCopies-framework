/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//public interface IBrowsableObjectInfoAccessor<TOwner, TItems> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
//{

//    TOwner Owner { get; }

//    IBrowsableObjectInfoCollection<TOwner, TItems> ItemCollection { get; }

//}

//internal class BrowsableObjectInfoAccessor< TOwner, TItems, TFactory, TParentItems> : IBrowsableObjectInfoAccessor<TOwner, TItems> where TOwner : class, IBrowsableObjectInfo where TParentItems : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory

//{

//    TOwner IBrowsableObjectInfoAccessor<TOwner, TItems>.Owner => (TOwner) (IBrowsableObjectInfo) Owner;

//    public BrowsableObjectInfo<TItems, TFactory, TParentItems> Owner { get; }

//    public IBrowsableObjectInfoCollection<TOwner, TItems> ItemCollection => (IBrowsableObjectInfoCollection<TOwner, TItems>) Owner.ItemCollection;

//    public BrowsableObjectInfoAccessor(BrowsableObjectInfo<TItems, TFactory, TParentItems> owner) => Owner = owner;

//}

//public interface IBrowsableObjectInfoModifier< TOwner, TItem > : System.IDisposable where TOwner : class, IBrowsableObjectInfo where TItem : class, IBrowsableObjectInfo

//{

//    /// <summary>
//    /// The item of this wrapper.
//    /// </summary>
//    TItem Item { get; }

//    /// <summary>
//    /// Sets the parent of <see cref="Item"/>.
//    /// </summary>
//    /// <param name="parent">An <see cref="IBrowsableObjectInfoAccessor"/> that represents the parent of <see cref="Item"/>.</param>
//    /// <param name="index">The index of this item in <paramref name="parent"/>'s item collection.</param>
//    /// <exception cref="InvalidOperationException">The current object is dispsoed.</exception>
//    /// <exception cref="ArgumentException">The owner collection is invalid.</exception>
//    void SetParent(IBrowsableObjectInfoAccessor<TOwner, TItem> parent, int index);

//    /// <summary>
//    /// Resets the parent of <see cref="Item"/> and disposes the current <see cref="IBrowsableObjectInfoModifier{TItem}"/>.
//    /// </summary>
//    /// <param name="parent"><see cref="Item"/>'s parent.</param>
//    /// <exception cref="InvalidOperationException">The current object is dispsoed.</exception>
//    /// <exception cref="ArgumentException">The owner collection is invalid.</exception>
//    void Reset(IBrowsableObjectInfoAccessor<TOwner, TItem> parent);

//}

//internal class BrowsableObjectInfoModifier< TOwner, TItems, TFactory, TParentItems> : IBrowsableObjectInfoModifier<TOwner, TParentItems> where TParentItems : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory where TOwner : class, IBrowsableObjectInfo

//{

//     TParentItems IBrowsableObjectInfoModifier<TOwner, TParentItems>.Item => (TParentItems) (IBrowsableObjectInfo)Item;

//    public BrowsableObjectInfo<TItems, TFactory, TParentItems> Item { get; internal set; }

//    public BrowsableObjectInfoModifier(BrowsableObjectInfo<TItems, TFactory, TParentItems> item) => Item = item;

//    bool _disposed;

//    public void SetParent(IBrowsableObjectInfoAccessor<TOwner, TParentItems> parent, int index)

//    {

//        if (_disposed)

//            throw new InvalidOperationException("The current object is disposed.");

//        if (object.ReferenceEquals(((System.Collections.Generic.IReadOnlyList<IBrowsableObjectInfo>)parent.ItemCollection)[index], Item))

//        {

//            Item.Parent = parent.Owner;

//            Item.HasParent = true;

//        }

//        else throw new ArgumentException("Invalid owner collection.");

//    }

//    public void Reset(IBrowsableObjectInfoAccessor<TOwner, TParentItems> parent)

//    {

//        if (_disposed)

//            throw new InvalidOperationException("The current object is disposed.");

//        if (object.ReferenceEquals(parent.Owner, Item.Parent))

//        {

//            Item.Parent = default;

//            Item.HasParent = false;

//            Dispose();

//        }

//        else throw new ArgumentException("Invalid owner collection.");

//    }

//    public void Dispose() => Dispose(true);

//    protected void Dispose(bool disposing)

//    {

//        if (disposing)

//        {

//            Item = default;

//            _disposed = true;

//        }

//    }

//    ~BrowsableObjectInfoModifier() => Dispose(false);

//}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Windows.Media.Imaging;
//using WinCopies.Collections;
//using WinCopies.Util;
//using IDisposable = WinCopies.Util.IDisposable;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// Provides interoperability for interacting with browsable items.
//    /// </summary>
//    public interface IBrowsableObjectInfo : IFileSystemObject, IDeepCloneable, IDisposable
//    {

//        /// <summary>
//        /// Gets the small <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        BitmapSource SmallBitmapSource { get; }

//        /// <summary>
//        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        BitmapSource MediumBitmapSource { get; }

//        /// <summary>
//        /// Gets the large <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        BitmapSource LargeBitmapSource { get; }

//        /// <summary>
//        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        BitmapSource ExtraLargeBitmapSource { get; }

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is browsable.
//        /// </summary>
//        bool IsBrowsable { get; }

//        /// <summary>
//        /// Gets or sets the factory for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
//        /// <exception cref="ArgumentNullException">value is null.</exception>
//        IBrowsableObjectInfoFactory Factory { get; }

//        /// <summary>
//        /// Gets a value that indicates whether the items of this <see cref="IBrowsableObjectInfo"/> are currently loaded.
//        /// </summary>
//        bool AreItemsLoaded { get; }

//        /// <summary>
//        /// Gets or sets the items loader for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
//        IBrowsableObjectInfoLoader ItemsLoader { get; }

//        /// <summary>
//        /// Gets the items of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        IReadOnlyBrowsableObjectInfoCollection<IBrowsableObjectInfo> Items { get; }

//        bool HasParent { get; }

//        /// <summary>
//        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="IBrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
//        /// </summary>
//        IBrowsableObjectInfo Parent { get; }

//        // IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo);

//        /// <summary>
//        /// Loads the items of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        void LoadItems();

//        /// <summary>
//        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

//        ///// <summary>
//        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
//        ///// </summary>
//        ///// <param name="itemsLoader">A custom items loader.</param>
//        //void LoadItems(IBrowsableObjectInfoLoader itemsLoader);

//        /// <summary>
//        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously.
//        /// </summary>
//        void LoadItemsAsync();

//        /// <summary>
//        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

//        IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo> RegisterLoader(IBrowsableObjectInfoLoader browsableObjectInfoLoader);

//        void UnregisterLoader();

//        ///// <summary>
//        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
//        ///// </summary>
//        ///// <param name="itemsLoader">A custom items loader.</param>
//        //void LoadItemsAsync(IBrowsableObjectInfoLoader itemsLoader);

//        // bool IsRenamingSupported { get; }

//        ///// <summary>
//        ///// Renames or move to a relative path, or both, the current <see cref="IBrowsableObjectInfo"/> with the specified name.
//        ///// </summary>
//        ///// <param name="newValue">The new name or relative path for this <see cref="IBrowsableObjectInfo"/>.</param>
//        //void Rename(string newValue);

//        // string ToString();

//        ///// <summary>
//        ///// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="IBrowsableObjectInfo"/>.
//        ///// </summary>
//        ///// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="IBrowsableObjectInfo"/>.</returns>
//        //IBrowsableObjectInfo Clone();

//        /// <summary>
//        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
//        /// </summary>
//        /// <param name="disposeItemsLoader">Whether to dispose the items loader of the current path.</param>
//        /// <param name="disposeParent">Whether to dispose the parent of the current path.</param>
//        /// <param name="disposeItems">Whether to dispose the items of the current path.</param>
//        /// <param name="recursively">Whether to dispose recursively.</param>
//        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
//        void Dispose(bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively);

//        void AddTo(IBrowsableObjectInfoCollection<IBrowsableObjectInfo, IBrowsableObjectInfo> collection);

//        void InsertTo(int index, IBrowsableObjectInfoCollection<IBrowsableObjectInfo, IBrowsableObjectInfo> collection);

//        void RemoveFrom(IBrowsableObjectInfoCollection<IBrowsableObjectInfo, IBrowsableObjectInfo> collection);

//    }

//    public interface IBrowsableObjectInfo<TFactory> : IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory

//    {

//        new TFactory Factory { get; set; }

//    }

//    public interface IBrowsableObjectInfo<TItems, TFactory> : IBrowsableObjectInfo<TFactory> where TParentItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory where TItems : class, IBrowsableObjectInfo

//    {

//        /// <summary>
//        /// Gets the items of this <see cref="IBrowsableObjectInfo"/>.
//        /// </summary>
//        new IReadOnlyBrowsableObjectInfoCollection<TItems> Items { get; }

//        IPathModifier<IBrowsableObjectInfo<TItems, TFactory>, TItems> RegisterLoader(IBrowsableObjectInfoLoader browsableObjectInfoLoader);

//        void AddTo(IBrowsableObjectInfoCollection<IBrowsableObjectInfo<TItems, TFactory>, TParentItems> collection);

//        void InsertTo(int index, IBrowsableObjectInfoCollection<IBrowsableObjectInfo<TItems, TFactory>, TParentItems> collection);

//        void RemoveFrom(IBrowsableObjectInfoCollection<IBrowsableObjectInfo<TItems, TFactory>, TParentItems> collection);

//    }

//}

//public interface IPathModifier<TOwner, TItems> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
//{

//    bool AreItemsLoaded { set; }

//    IBrowsableObjectInfoAccessor<TOwner, TItems> Accessor { get; }

//    // IBrowsableObjectInfo Parent { set; }

//    // IBrowsableObjectInfoCollection<IBrowsableObjectInfo> Items { get; }

//}

//public class ProgressChangedEventArgs<TItems, TFactory, TParentItems> : System.ComponentModel.ProgressChangedEventArgs where TItems : class, IBrowsableObjectInfo where TFactory : class, IBrowsableObjectInfoFactory where TParentItems : class, IBrowsableObjectInfo
//{

//    public new IBrowsableObjectInfo<TItems, TFactory, TParentItems> UserState => base.UserState as IBrowsableObjectInfo<TItems, TFactory, TParentItems>;

//    public ProgressChangedEventArgs(int progressPercentage, object userState) : base(progressPercentage, userState)
//    {

//    }

//}

//public class ReadOnlyBrowsableObjectInfoCollection< TOwner, TItems> : IReadOnlyBrowsableObjectInfoCollection<TItems> where TItems : class, IBrowsableObjectInfo where TOwner : class, IBrowsableObjectInfo
//{

//    protected IBrowsableObjectInfoCollection< TOwner, TItems> Items { get; }

//    public IBrowsableObjectInfo Owner => ((IBrowsableObjectInfoCollection< TOwner, TItems>)this.Items).Owner;

//    public int Count => Items.Count;

//    // bool WinCopies.Collections.IReadOnlyList<TItems>.IsReadOnly => true;

//    bool System.Collections.Generic.ICollection<TItems>.IsReadOnly => true;

//    bool System.Collections.IList.IsReadOnly => true;

//    bool System.Collections.IList.IsFixedSize => Items.IsFixedSize;

//    object ICollection.SyncRoot => Items.SyncRoot;

//    bool ICollection.IsSynchronized => Items.IsSynchronized;

//    // TItems IReadOnlyList<TItems>.this[int index] => throw new NotImplementedException();

//    object System.Collections.IList.this[int index] { get => this[index]; set => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection); }

//    public TItems this[int index] { get => Items[index].Item; set => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection); }

//    public ReadOnlyBrowsableObjectInfoCollection(IBrowsableObjectInfoCollection< TOwner, TItems> list) => Items = list;

//    void Collections.IReadOnlyList<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void ICollection<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void IList.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    //void WinCopies.Collections.IReadOnlyList<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    //void System.Collections.Generic.ICollection<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    public IEnumerator<TItems> GetEnumerator() => new Enumerator(Items);

//    void System.Collections.IList.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void System.Collections.Generic.IList<TItems>.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void WinCopies.Collections.IReadOnlyList<TItems>.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    public int IndexOf(TItems item)
//    {

//        for (int i = 0; i < Count; i++)

//            if (Items[i].Item.Equals(item))

//                return i;

//        return -1;

//    }

//    void System.Collections.Generic.IList<TItems>.Insert(int index, TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void System.Collections.Generic.ICollection<TItems>.Add(TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    public bool Contains(TItems item)
//    {

//        foreach (IBrowsableObjectInfoModifier< TOwner, TItems> _item in Items)

//            if (_item.Item.Equals(item))

//                return true;

//        return false;

//    }

//    public void CopyTo(TItems[] array, int arrayIndex)

//    {

//        if (arrayIndex < 0 || arrayIndex > array.Length)

//            throw new IndexOutOfRangeException("arrayIndex is not in the required value range.");

//        if (Count > array.Length - arrayIndex)

//            throw new InvalidOperationException("There is not enough space starting with the given index in the given array.");

//        foreach (var item in Items)

//            array[arrayIndex++] = item.Item;

//    }

//    bool System.Collections.Generic.ICollection<TItems>.Remove(TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    int System.Collections.IList.Add(object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    bool System.Collections.IList.Contains(object value) => value is TItems item ? Contains(item) : false;

//    int System.Collections.IList.IndexOf(object value) => value is TItems item ? IndexOf(item) : -1;

//    void System.Collections.IList.Insert(int index, object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void System.Collections.IList.Remove(object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

//    void ICollection.CopyTo(Array array, int index) => CopyTo((TItems[])array, index);

//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//    [Serializable]
//    public struct Enumerator : IEnumerator<TItems>, IEnumerator
//    {

//        private readonly IEnumerator<IBrowsableObjectInfoModifier< TOwner, TItems>> _enumerator;

//        public TItems Current { get; private set; }

//        object IEnumerator.Current => Current;

//        public Enumerator(IEnumerable<IBrowsableObjectInfoModifier< TOwner, TItems>> innerEnumerable)

//        {

//            _enumerator = innerEnumerable.GetEnumerator();

//            Current = default;

//        }

//        public bool MoveNext()
//        {

//            if (_enumerator.MoveNext())

//            {

//                Current = _enumerator.Current.Item;

//                return true;

//            }

//            else

//                return false;

//        }

//        public void Reset() => Dispose();

//        public void Dispose()
//        {

//            _enumerator.Dispose();

//            Current = default;

//        }

//    }

//}
//public class BrowsableObjectInfoCollection<TOwner, TItems> : WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<TOwner, TItems>> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
//{

//    // todo: Owner must be accessible from the read-only wrapper of this collection

//    public TOwner Owner => _accessor.Owner;

//    private readonly IBrowsableObjectInfoAccessor<TOwner, TItems> _accessor;

//    private readonly BrowsableObjectInfoCollectionInternal<TOwner, TItems> _innerList;

//    protected IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => _innerList.Items;

//    public BrowsableObjectInfoCollection(IBrowsableObjectInfoAccessor<TOwner, TItems> accessor)
//    {

//        if (object.ReferenceEquals(this, accessor.ItemCollection))

//        {

//            _accessor = accessor;

//            _innerList = new BrowsableObjectInfoCollectionInternal<TOwner, TItems>();

//        }

//        else

//            throw new ArgumentException("Invalid owner.");

//    }

//    // protected virtual T GetItem(int index) => _innerList[index].Item;

//    protected virtual void SetItem(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item)
//    {

//        _innerList[index].Reset(_accessor, index);

//        _innerList[index] = item;

//        _innerList[index].SetParent(_accessor, index);

//    }

//    /// <summary>
//    /// Determines the index of a specific item in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.
//    /// </summary>
//    /// <param name="item">The object to locate in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</param>
//    /// <returns>The index of item if found in the list; otherwise, -1.</returns>
//    public int IndexOf(IBrowsableObjectInfoModifier<TOwner, TItems> item) => IndexOfItem(item);

//    int IList.IndexOf(object value) => value is IBrowsableObjectInfoModifier<TOwner, TItems> item ? IndexOfItem(item) : -1;

//    protected virtual int IndexOfItem(IBrowsableObjectInfoModifier<TOwner, TItems> item)
//    {
//        for (int i = 0; i < Count; i++)

//            if (_innerList[i].Item?.Equals(item) == true)

//                return i;

//        return -1;
//    }

//    public bool IsReadOnly => true;

//    public int Count => _innerList.Count;

//    public virtual bool IsFixedSize => false;

//    public object SyncRoot => _innerList.sync;

//    public bool IsSynchronized => sync

//    object IList.this[int index] { get => _innerList[index]; set => _innerList[index] = value as IBrowsableObjectInfoModifier<TOwner, TItems> ?? throw new ArgumentException("The given value is not an IBrowsableObjectInfoModifier."); }

//    /// <summary>
//    /// Gets or sets the element at the specified index.
//    /// </summary>
//    /// <param name="index">The zero-based index of the element to get or set.</param>
//    /// <returns>The element at the specified index.</returns>
//    /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
//    /// <exception cref="System.NotSupportedException">The property is set and the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
//    public IBrowsableObjectInfoModifier<TOwner, TItems> this[int index] { get => _innerList[index]; set => _innerList[index] = value; }

//    /// <summary>
//    /// Inserts an item to the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> at the specified index.
//    /// </summary>
//    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
//    /// <param name="item">The object to insert into the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</param>
//    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
//    /// <exception cref="NotSupportedException">The <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
//    public void Insert(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item) => InsertItem(index, item);

//    protected virtual void InsertItem(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item)
//    {

//        _innerList.Insert(index, item);

//        item.SetParent(_accessor, index);

//    }

//    /// <summary>
//    /// Removes the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> item at the specified index.
//    /// </summary>
//    /// <param name="index">The zero-based index of the item to remove.</param>
//    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
//    /// <exception cref="NotSupportedException">The <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
//    public void RemoveAt(int index) => RemoveItem(index);

//    protected virtual bool RemoveItem(int index)

//    {

//        _innerList[index].Reset(_accessor, index);

//        _innerList.RemoveAt(index);

//        return true;

//    }

//    public void Clear() => ClearItems();

//    protected virtual void ClearItems()

//    {

//        for (int i = 0; i < _innerList.Count; i++)

//            _innerList[i].Reset(_accessor, i);

//        _innerList.Clear();

//    }

//    public void Add(IBrowsableObjectInfoModifier<TOwner, TItems> item) => InsertItem(Count, item);

//    public bool Contains(IBrowsableObjectInfoModifier<TOwner, TItems> item)
//    {
//        for (int i = 0; i < Count; i++)

//            if (_innerList[i].Item?.Equals(item) == true)

//                return true;

//        return false;
//    }

//    public void CopyTo(IBrowsableObjectInfoModifier<TOwner, TItems>[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

//    public IEnumerator<IBrowsableObjectInfoModifier<TOwner, TItems>> GetEnumerator() => _innerList.GetEnumerator();

//    public int Add(object value) => value is IBrowsableObjectInfoModifier<TOwner, TItems> item ? Add(item) : throw new ArgumentException("The given value is not an IBrowsableObjectInfoModifier.") ;

//    public bool Contains(object value) => value is TItems item ? Contains(item) : false;

//    public void Insert(int index, object value)
//    {

//        if (value is IBrowsableObjectInfoModifier<TOwner, TItems> item)

//            Insert(index, item);

//        else

//            throw new ArgumentException("The given value is not an IBrowsableObjectInfoModifier.");

//    }

//    public bool Remove(IBrowsableObjectInfoModifier<TOwner, TItems> item) => RemoveItem(IndexOfItem(item)) ; 

//    public void Remove(object value)
//    {

//        if (value is IBrowsableObjectInfoModifier<TOwner, TItems> item)

//            Remove(item);

//        else

//            throw new ArgumentException("The given value is not an IBrowsableObjectInfoModifier.");

//    }

//    public void CopyTo(Array array, int index) => CopyTo((TItems[])array, index);

//    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerList).GetEnumerator() ;

//    //[Serializable]
//    //public struct Enumerator : IEnumerator<TItems>, IEnumerator
//    //{

//    //    private IEnumerator<IBrowsableObjectInfoModifier<TOwner, TItems>> _enumerator;

//    //    public TItems Current { get; private set; }

//    //    object IEnumerator.Current => Current;

//    //    public Enumerator(IEnumerable<IBrowsableObjectInfoModifier<TOwner, TItems>> innerEnumerable)

//    //    {

//    //        _enumerator = innerEnumerable.GetEnumerator();

//    //        Current = default;

//    //    }

//    //    public bool MoveNext()
//    //    {

//    //        if (_enumerator.MoveNext())

//    //        {

//    //            Current = _enumerator.Current.Item;

//    //            return true;

//    //        }

//    //        else

//    //            return false;

//    //    }

//    //    public void Reset() => Dispose();

//    //    public void Dispose()
//    //    {

//    //        _enumerator.Dispose();

//    //        Current = default;

//    //    }

//    //}

//}

//internal class PathModifier< TOwner, TItems, TFactory, TParentItems> : IPathModifier<TOwner, TParentItems> where TParentItems : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory where TOwner : class, IBrowsableObjectInfo

//{

//    public bool AreItemsLoaded { set => Accessor.Owner.AreItemsLoaded = value; }

//    public BrowsableObjectInfoAccessor< TOwner, TItems, TFactory, TParentItems> Accessor { get; }

//    IBrowsableObjectInfoAccessor<TOwner, TParentItems> IPathModifier<TOwner, TParentItems>.Accessor => (IBrowsableObjectInfoAccessor<TOwner, TParentItems>)Accessor;

//    // public IBrowsableObjectInfo Parent { set => _path.Parent = value; }

//    // public IBrowsableObjectInfoCollection<TItems> Items => _path.items;

//    // IBrowsableObjectInfoCollection<IBrowsableObjectInfo> IPathModifier.Items => (IBrowsableObjectInfoCollection<IBrowsableObjectInfo>) _path.items ; 

//    public PathModifier(BrowsableObjectInfoAccessor< TOwner, TItems, TFactory, TParentItems> accessor) => Accessor = accessor;

//}

///// <summary>
///// Represents a file system item that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
///// </summary>
//public class ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory> : ArchiveItemInfoProvider<TItems, TFactory>, IShellObjectInfo where TItems : IFileSystemObjectInfo where TArchiveItemInfoItems : IArchiveItemInfo where TFactory : IShellObjectInfoFactory
//{

//    private IArchiveItemInfoFactory _archiveItemInfoFactory;

//    /// <summary>
//    /// Gets or sets the factory this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/>'s use to create new objects that represent archive items.
//    /// </summary>
//    /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is busy.</exception>
//    /// <exception cref="ArgumentNullException">The given value is null.</exception>
//    public override IArchiveItemInfoFactory ArchiveItemInfoFactory
//    {
//        get => _archiveItemInfoFactory; set
//        {

//            ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

//            IArchiveItemInfoFactory oldFactory = _archiveItemInfoFactory;

//            value.RegisterPath(this);

//            _archiveItemInfoFactory = value;

//            oldFactory.UnregisterPath();

//        }
//    }

//    /// <summary>
//    /// The parent <see cref="IShellObjectInfo"/> of the current archive item. If the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents an archive file, this property returns the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>, or <see langword="null"/> otherwise.
//    /// </summary>
//    public override IShellObjectInfo ArchiveShellObject => FileType == FileType.Archive ? this : null;

//    /// <summary>
//    /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    public ShellObject ShellObject { get; private set; } = null;

//    /// <summary>
//    /// Gets the localized name of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> depending the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
//    /// </summary>
//    public override string LocalizedName => ShellObject.GetDisplayName(DisplayNameType.Default);

//    /// <summary>
//    /// Gets the name of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> depending of the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
//    /// </summary>
//    public override string Name => ShellObject.Name;

//    /// <summary>
//    /// Gets the small <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    public override BitmapSource SmallBitmapSource => ShellObject.Thumbnail.SmallBitmapSource;

//    /// <summary>
//    /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    public override BitmapSource MediumBitmapSource => ShellObject.Thumbnail.MediumBitmapSource;

//    /// <summary>
//    /// Gets the large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    public override BitmapSource LargeBitmapSource => ShellObject.Thumbnail.LargeBitmapSource;

//    /// <summary>
//    /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    public override BitmapSource ExtraLargeBitmapSource => ShellObject.Thumbnail.ExtraLargeBitmapSource;

//    /// <summary>
//    /// Gets a value that indicates whether this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is browsable.
//    /// </summary>
//    public override bool IsBrowsable => (ShellObject is IEnumerable<ShellObject> || FileType == FileType.Archive) && FileType != FileType.File && FileType != FileType.Link; // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

//    /// <summary>
//    /// Gets a <see cref="FileSystemInfo"/> object that provides info for the folders and files. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is not a folder, drive or file. See the <see cref="FileSystemObject.FileType"/> property for more details.
//    /// </summary>
//    public FileSystemInfo FileSystemInfoProperties { get; private set; } = null;

//    /// <summary>
//    /// Gets a <see cref="DriveInfo"/> object that provides info for drives. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is not a drive. See the <see cref="FileSystemObject.FileType"/> property for more details.
//    /// </summary>
//    public DriveInfo DriveInfoProperties { get; private set; } = null;

//    /// <summary>
//    /// Gets a <see cref="IKnownFolder"/> object that provides info for the system known folders. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is not a system known folder. See the <see cref="FileSystemObject.FileType"/> property for more details.
//    /// </summary>
//    public IKnownFolder KnownFolderInfo { get; private set; } = null;

//    //private FileStream _archiveFileStream = null;

//    ///// <summary>
//    ///// The <see cref="FileStream"/> for this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> when it represents an archive file system item. See the remarks section.
//    ///// </summary>
//    ///// <remarks>
//    ///// This field is only used by the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>, <see cref="FolderLoader"/> and the <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> classes in order to lock the file that the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents when the items of the archive are loaded.
//    ///// </remarks>
//    //public FileStream ArchiveFileStream { get => _archiveFileStream; internal set => OnPropertyChanged(nameof(ArchiveFileStream), nameof(_archiveFileStream), value, typeof(ShellObjectInfo)); }

//    /// <summary>
//    /// Gets the special folder type of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>. <see cref="SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is a casual file system item.
//    /// </summary>
//    public SpecialFolder SpecialFolder { get; private set; } = SpecialFolder.OtherFolderOrFile;

//    private readonly DeepClone<ShellObject> _shellObjectDelegate;

//    ///// <summary>
//    ///// Gets the <see cref="IO.FileType"/> and <see cref="IO.SpecialFolder"/> for a given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
//    ///// </summary>
//    ///// <param name="path">The path from which to get the associated <see cref="FileType"/>.</param>
//    ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to get the associated <see cref="FileType"/>.</param>
//    ///// <returns>The <see cref="FileType"/> and <see cref="IO.SpecialFolder"/> for the given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
//    //public static (FileType fileType, SpecialFolder specialFolder) GetFileType(string path, ShellObject shellObject) => (!shellObject.IsFileSystemObject
//    //        ? FileType.SpecialFolder
//    //        : shellObject is FileSystemKnownFolder && ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) && shellObject is ShellFile
//    //        ? FileType.Archive
//    //        : shellObject is ShellFile
//    //        ? shellObject.IsLink
//    //            ? FileType.Link
//    //            : ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) ? FileType.Archive : FileType.File
//    //        : System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder, GetSpecialFolderType(shellObject));

//    ///// <summary>
//    ///// Returns the <see cref="IO.SpecialFolder"/> value for a given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
//    ///// </summary>
//    ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to return a <see cref="IO.SpecialFolder"/> value.</param>
//    ///// <returns>A <see cref="IO.SpecialFolder"/> value that correspond to the given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
//    //public static SpecialFolder GetSpecialFolderType(ShellObject shellObject)

//    //{

//    //    SpecialFolder? value = null;

//    //    PropertyInfo[] knownFoldersProperties = typeof(KnownFolders).GetProperties();

//    //    for (int i = 1 ; i < knownFoldersProperties.Length; i++)

//    //        try
//    //        {

//    //            for (; i < knownFoldersProperties.Length; i++)

//    //                if (shellObject.ParsingName == (knownFoldersProperties[i].GetValue(null) as IKnownFolder)?.ParsingName)

//    //                    value = (SpecialFolder)typeof(SpecialFolder).GetField(knownFoldersProperties[i].Name).GetValue(null);

//    //            break;

//    //        }

//    //        catch (ShellException) { i++; }

//    //    #region Comments

//    //    //    else if (shellObject.ParsingName == KnownFolders.MusicLibrary.ParsingName)

//    //    //    value = SpecialFolder.MusicLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.PicturesLibrary.ParsingName)

//    //    //    value = SpecialFolder.PicturesLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.CameraRollLibrary.ParsingName)

//    //    //    value = SpecialFolder.CameraRollLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.SavedPicturesLibrary.ParsingName)

//    //    //    value = SpecialFolder.SavedPicturesLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.RecordedTVLibrary.ParsingName)

//    //    //    value = SpecialFolder.RecordedTVLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.VideosLibrary.ParsingName)

//    //    //    value = SpecialFolder.VideosLibrary;

//    //    //else if (shellObject.ParsingName == KnownFolders.UsersLibraries.ParsingName)

//    //    //    value = SpecialFolder.UsersLibraries;

//    //    //else if (shellObject.ParsingName == KnownFolders.Libraries.ParsingName)

//    //    //    value = SpecialFolder.Libraries;

//    //    //else if (shellObject.ParsingName == KnownFolders.Computer.ParsingName)

//    //    //    value = SpecialFolder.Computer;

//    //    //else

//    //    //    value = SpecialFolder.OtherFolderOrFile;

//    //    #endregion

//    //    return value ?? SpecialFolder.OtherFolderOrFile;

//    //}

//    ///// <summary>
//    ///// Initializes a new instance of the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> class.
//    ///// </summary>
//    ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents.</param>
//    ///// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    //public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) : this(shellObject, path, fileType, specialFolder, new ShellObjectInfoFactory(), new ArchiveItemInfoFactory()) { }

//    ///// <summary>
//    ///// Initializes a new instance of the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> class using custom factories for <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//    ///// </summary>
//    ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents.</param>
//    ///// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    ///// <param name="shellObjectInfoFactory">The factory this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/>'s use to create new objects that represent casual file system items.</param>
//    ///// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/>'s use to create new objects that represent archive items.</param>
//    //public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder, ShellObjectInfoFactory shellObjectInfoFactory, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, archiveItemInfoFactory) =>

//    //    Init(shellObject, nameof(FileType), specialFolder, shellObjectInfoFactory);

//    /// <summary>
//    /// Initializes a new instance of the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/>.
//    /// </summary>
//    /// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    /// <param name="fileType">The file type of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>. <see cref="IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is a casual file system item.</param>
//    /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.DeepClone()"/> method to get a new <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</param>
//    /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents.</param>
//    public ShellObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) : this(path, fileType, specialFolder, shellObject, shellObjectDelegate, null, null) { }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>s and <see cref="ArchiveItemInfo{TItems, TFactory}"/>s.
//    /// </summary>
//    /// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    /// <param name="fileType">The file type of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> is a casual file system item.</param>
//    /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.DeepClone()"/> method to get a new <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</param>
//    /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents.</param>
//    /// <param name="factory">The factory this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and associated <see cref="FolderLoader"/>s and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/>s use to create new objects that represent casual file system items.</param>
//    /// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/>'s use to create new objects that represent archive items.</param>
//    public ShellObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate, IShellObjectInfoFactory factory, IArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, (TFactory)(factory ?? new ShellObjectInfoFactory<TItems, TArchiveItemInfoItems>())) // =>

//    // Init(specialFolder, shellObjectDelegate, shellObject, nameof(fileType), archiveItemInfoFactory); // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

//    // private void Init(SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject, string fileTypeParameterName, IArchiveItemInfoFactory archiveItemInfoFactory)

//    {

//        _shellObjectDelegate = shellObjectDelegate;

//        if (shellObject is null)

//            shellObject = shellObjectDelegate(null);

//#if DEBUG

//        if (shellObject.ParsingName != Path)

//            Debug.WriteLine("");

//#endif

//        if ((FileType == FileType.SpecialFolder && specialFolder == SpecialFolder.OtherFolderOrFile) || (FileType != FileType.SpecialFolder && specialFolder != SpecialFolder.OtherFolderOrFile))

//            throw new ArgumentException(string.Format(Generic.FileTypeAndSpecialFolderNotCorrespond, nameof(fileType), nameof(specialFolder), FileType, specialFolder));



//        if (archiveItemInfoFactory is null)

//            archiveItemInfoFactory = new ArchiveItemInfoFactory<IArchiveItemInfo>();

//        else

//            ThrowOnInvalidFactoryUpdateOperation(archiveItemInfoFactory, nameof(archiveItemInfoFactory));



//        // archiveItemInfoFactory.Path = this;

//        _archiveItemInfoFactory = archiveItemInfoFactory;

//        ShellObject = shellObject;

//        // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

//        // NormalizedPath = Util.GetNormalizedPath(path);

//        SpecialFolder = specialFolder;

//        SetFileSystemInfoProperties(shellObject, false);

//    }

//    private void SetFileSystemInfoProperties(ShellObject shellObject, bool reinit)
//    {

//        if (reinit)

//        {

//            FileSystemInfoProperties = null;

//            DriveInfoProperties = null;

//            KnownFolderInfo = null;

//        }

//        if (FileType == FileType.Folder || (FileType == FileType.SpecialFolder && shellObject.IsFileSystemObject))

//            FileSystemInfoProperties = new DirectoryInfo(Path);

//        else if (FileType == FileType.File || FileType == FileType.Archive || FileType == FileType.Link)

//            FileSystemInfoProperties = new FileInfo(Path);

//        else if (FileType == FileType.Drive)

//        {

//            FileSystemInfoProperties = new DirectoryInfo(Path);

//            DriveInfoProperties = new DriveInfo(Path);

//        }

//        else if (FileType == FileType.SpecialFolder)

//            KnownFolderInfo = KnownFolderHelper.FromParsingName(shellObject.ParsingName);

//    }

//    /// <summary>
//    /// Returns the parent of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    /// <returns>The parent of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</returns>
//    protected override IBrowsableObjectInfo GetParent()
//    {

//        (FileType, SpecialFolder) getFileType(ShellObject _shellObject)

//        {

//            SpecialFolder specialFolder = IO.Path.GetSpecialFolder(_shellObject);

//            FileType fileType = specialFolder == SpecialFolder.OtherFolderOrFile ? FileType.Folder : FileType.SpecialFolder;

//            return (fileType, specialFolder);

//        }

//        if (FileType == FileType.Folder || FileType == FileType.Archive || (FileType == FileType.SpecialFolder && ShellObject.IsFileSystemObject))

//        {

//            DirectoryInfo parentDirectoryInfo = FileType == FileType.Archive ? new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) : Directory.GetParent(Path);

//            string parent = parentDirectoryInfo.FullName;

//            var shellObject = ShellObject.FromParsingName(parent);

//            (FileType fileType, SpecialFolder specialFolder) = getFileType(shellObject);

//            return Factory.GetBrowsableObjectInfo(parent, fileType, specialFolder, shellObject, DefaultShellObjectDeepClone);

//        }

//        else if (FileType == FileType.Drive)

//            return Factory.GetBrowsableObjectInfo(KnownFolders.Computer.Path, FileType.SpecialFolder, SpecialFolder.Computer, ShellObject.FromParsingName(KnownFolders.Computer.ParsingName), DefaultShellObjectDeepClone);

//        else if (FileType == FileType.SpecialFolder && SpecialFolder != SpecialFolder.Computer)

//        {

//            ShellObject shellObject = ShellObject.Parent;

//            string path = Path;

//            if (path.EndsWith(PathSeparator.ToString()))

//                path = path.Remove(path.Length - 1);

//            (FileType fileType, SpecialFolder specialFolder) = getFileType(shellObject);

//            return Factory.GetBrowsableObjectInfo(path.Remove(path.LastIndexOf(PathSeparator)), fileType, specialFolder, shellObject, DefaultShellObjectDeepClone);

//        }

//        else return null;

//    }

//    /// <summary>
//    /// Loads the items of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> asynchronously.
//    /// </summary>
//    /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will report progress.</param>
//    /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will supports cancellation.</param>
//    public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation)
//    {

//        if (!IsBrowsable)

//            throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

//        if (ShellObject.IsFileSystemObject)

//        {

//            if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

//                LoadItems((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//            else if (FileType == FileType.Archive)

//                LoadItems((IBrowsableObjectInfoLoader)new ArchiveLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        }

//        else

//            LoadItems((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        //else

//        //{

//        //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

//        //    foreach (ShellObject item in items)

//        //    {

//        //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

//        //    }

//        //}

//    }

//    /// <summary>
//    /// Loads the items of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> asynchronously.
//    /// </summary>
//    /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will report progress.</param>
//    /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will supports cancellation.</param>
//    public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation)
//    {

//        if (!IsBrowsable)

//            throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

//        if (ShellObject.IsFileSystemObject)

//        {

//            if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

//                LoadItemsAsync((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//            else if (FileType == FileType.Archive)

//                LoadItemsAsync((IBrowsableObjectInfoLoader)new ArchiveLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        }

//        else

//            LoadItemsAsync((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        //else

//        //{

//        //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

//        //    foreach (ShellObject item in items)

//        //    {

//        //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

//        //    }

//        //}

//    }

//    // /// <summary>
//    // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> in memory.
//    // /// </summary>

//    /// <summary>
//    /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
//    /// </summary>
//    /// <param name="disposing">Whether to dispose managed resources.</param>
//    /// <param name="disposeItemsLoader">Whether to dispose the items loader of the current path.</param>
//    /// <param name="disposeParent">Whether to dispose the parent of the current path.</param>
//    /// <param name="disposeItems">Whether to dispose the items of the current path.</param>
//    /// <param name="recursively">Whether to dispose recursively.</param>
//    /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is busy and does not support cancellation.</exception>
//    protected override void Dispose(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
//    {

//        base.Dispose(disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

//        ShellObject.Dispose();

//        //if (ArchiveFileStream != null)

//        //{

//        //    ArchiveFileStream.Dispose();

//        //    ArchiveFileStream.Close();

//        //}

//    }

//    /// <summary>
//    /// Gets a string representation of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.
//    /// </summary>
//    /// <returns>The <see cref="LocalizedName"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</returns>
//    public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

//    ///// <summary>
//    ///// Gets or sets the factory for this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/>.
//    ///// </summary>
//    ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
//    ///// <exception cref="ArgumentNullException">value is null.</exception>
//    //public new ShellObjectInfoFactory Factory { get => (ShellObjectInfoFactory)base.Factory; set => base.Factory = value; }

//    // public override bool IsRenamingSupported => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.File, FileType.Drive, FileType.Archive, FileType.Link);

//    ///// <summary>
//    ///// Renames or move to a relative path, or both, the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
//    ///// </summary>
//    ///// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
//    //public override void Rename(string newValue)

//    //{

//    //    if (If(IfCT.Or, IfCM.Logical, IfComp.NotEqual, out string key, FileType, GetKeyValuePair(nameof(FileType.Folder), FileType.Folder), GetKeyValuePair(nameof(FileType.File), FileType.File), GetKeyValuePair(nameof(FileType.Drive), FileType.Drive)))

//    //        throw new InvalidOperationException($"{nameof(FileType)} must have one of the following values: {nameof(FileType.Folder)}, {nameof(FileType.File)} or {nameof(FileType.Drive)}. The value was {key}.");

//    //    string getNewPath() => System.IO.Path.GetDirectoryName(Path) + IO.Path.PathSeparator + newValue;

//    //    switch (FileType)
//    //    {

//    //        case FileType.Folder:

//    //            Directory.Move(Path, getNewPath());

//    //            break;

//    //        case FileType.File:

//    //            File.Move(Path, getNewPath());

//    //            break;

//    //        case FileType.Drive:

//    //            DriveInfoProperties.VolumeLabel = newValue;

//    //            break;

//    //    }

//    //}

//    /// <summary>
//    /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//    /// </summary>
//    public override bool NeedsObjectsOrValuesReconstruction => true;

//    /// <summary>
//    /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//    /// </summary>
//    /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
//    protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
//    {

//        base.OnDeepClone(browsableObjectInfo);

//        if (ArchiveItemInfoFactory.UseRecursively)

//            ((ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>)browsableObjectInfo).ArchiveItemInfoFactory = (ArchiveItemInfoFactory<TArchiveItemInfoItems>)ArchiveItemInfoFactory.DeepClone();

//    }

//    /// <summary>
//    /// Gets a deep clone of this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>. The <see cref="OnDeepClone(BrowsableObjectInfo{TParentItems, TItems, TFactory})"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//    /// </summary>
//    protected override BrowsableObjectInfo DeepCloneOverride() => new ShellObjectInfo<TItems, TArchiveItemInfoItems, TFactory>(Path, FileType, SpecialFolder, _shellObjectDelegate(ShellObject), _shellObjectDelegate, (ShellObjectInfoFactory<TItems, TArchiveItemInfoItems>)Factory.DeepClone(), (ArchiveItemInfoFactory<TArchiveItemInfoItems>)ArchiveItemInfoFactory.DeepClone());

//}

//    public abstract class BrowsableObjectInfo : FileSystemObject, IBrowsableObjectInfo
//    {

//        public abstract bool NeedsObjectsOrValuesReconstruction { get; }

//        /// <summary>
//        /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//        /// </summary>
//        /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
//        protected virtual void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)

//        {

//            // browsableObjectInfo.AreItemsLoaded = false;

//            if (!(ItemsLoader is null))

//                browsableObjectInfo.ItemsLoader = (IBrowsableObjectInfoLoader)ItemsLoader.DeepClone();

//            // browsableObjectInfo.SetItemsProperty();

//            //if (Factory.UseRecursively)

//            // else

//            // browsableObjectInfo._factory = null;

//            // browsableObjectInfo._parent = null;

//        }

//        /// <summary>
//        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//        /// </summary>
//        protected abstract BrowsableObjectInfo DeepCloneOverride();

//        /// <summary>
//        /// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
//        public object DeepClone()

//        {

//            //var callee = new StackFrame(0).GetMethod();

//            //var caller = new StackFrame(1).GetMethod();

//            //if (callee.DeclaringType.Equals(caller.DeclaringType) || (caller.IsConstructor && caller.DeclaringType.BaseType.Equals(this.GetType())))

//            //{

//            ((IDisposable)this).ThrowIfDisposingOrDisposed();

//            BrowsableObjectInfo browsableObjectInfo = DeepCloneOverride();

//            OnDeepClone(browsableObjectInfo);

//            return browsableObjectInfo;

//            //}

//            //    else

//            //        throw new InvalidOperationException("The type of the caller of the current constructor is not the same as the type of this constructor.");

//        }

//        /// <summary>
//        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
//        /// </summary>
//        /// <param name="disposing">Whether to dispose managed resources.</param>
//        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy and does not support cancellation.</exception>
//        protected virtual void Dispose(bool disposing)

//        {

//            if (ItemsLoader != null)

//            {

//                if (ItemsLoader.IsBusy)

//                    ItemsLoader.Cancel();

//                // ItemsLoader.Path = null;

//            }

//            if (disposing)

//                Parent = null;

//        }

//        internal static Icon TryGetIcon(int iconIndex, string dll, System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

//        /// <summary>
//        /// When overridden in a derived class, gets the small <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public abstract BitmapSource SmallBitmapSource { get; }

//        /// <summary>
//        /// When overridden in a derived class, gets the medium <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public abstract BitmapSource MediumBitmapSource { get; }

//        /// <summary>
//        /// When overridden in a derived class, gets the large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public abstract BitmapSource LargeBitmapSource { get; }

//        /// <summary>
//        /// When overridden in a derived class, gets the extra large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public abstract BitmapSource ExtraLargeBitmapSource { get; }

//        /// <summary>
//        /// When overridden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is browsable.
//        /// </summary>
//        public abstract bool IsBrowsable { get; }

//        /// <summary>
//        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
//        /// </summary>
//        public bool AreItemsLoaded { get; internal set; }

//        /// <summary>
//        /// Gets the items loader for this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public IBrowsableObjectInfoLoader ItemsLoader { get; internal set; }

//        public bool HasParent { get; internal set; }

//        /// <summary>
//        /// Gets a value that indicates whether the current object is disposing.
//        /// </summary>
//        public bool IsDisposing { get; internal set; }

//        private IBrowsableObjectInfo _parent = default;

//        /// <summary>
//        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
//        /// </summary>
//        public IBrowsableObjectInfo Parent { get { if (HasParent) return _parent; else _parent = GetParent(); HasParent = true; return _parent; } internal set => _parent = value; }

//        /// <summary>
//        /// Loads the items of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public virtual void LoadItems()

//        {

//            if (ItemsLoader == null)

//                LoadItems(true, true);

//            else

//                ItemsLoader.LoadItems();

//        }

//        /// <summary>
//        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

//        /// <summary>
//        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
//        /// </summary>
//        /// <param name="itemsLoader">A custom items loader.</param>
//        public virtual void LoadItems(IBrowsableObjectInfoLoader itemsLoader)

//        {

//            if (!IsBrowsable)

//                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

//            ItemsLoader = itemsLoader;

//            ItemsLoader.LoadItems();

//        }

//        /// <summary>
//        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously.
//        /// </summary>
//        public virtual void LoadItemsAsync()

//        {

//            if (ItemsLoader == null)

//                LoadItemsAsync(true, true);

//            else

//                ItemsLoader.LoadItemsAsync();

//        }

//        /// <summary>
//        /// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        public abstract void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

//        /// <summary>
//        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
//        /// </summary>
//        /// <param name="itemsLoader">A custom items loader.</param>
//        public virtual void LoadItemsAsync(IBrowsableObjectInfoLoader itemsLoader)

//        {

//            if (!IsBrowsable)

//                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

//            ItemsLoader = itemsLoader;

//            ItemsLoader.LoadItemsAsync();

//        }

//        /// <summary>
//        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        /// <returns>The parent of this <see cref="BrowsableObjectInfo"/>.</returns>
//        protected abstract IBrowsableObjectInfo GetParent();

//        /// <summary>
//        /// Gets a value that indicates whether the current object is disposed.
//        /// </summary>
//        public bool IsDisposed { get; internal set; }

//        /// <summary>
//        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">The <see cref="ItemsLoader"/> is busy and does not support cancellation.</exception>
//        public void Dispose()

//        {

//            IsDisposing = true;

//            Dispose(true);

//            GC.SuppressFinalize(this);

//            IsDisposed = true;

//            IsDisposing = false;

//        }

//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
//        ~BrowsableObjectInfo()
//#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

//        {

//            Dispose(false);

//        }

//    }

//    /// <summary>
//    /// The base class for all browsable items of the WinCopies framework.
//    /// </summary>
//    public abstract class BrowsableObjectInfo<TItems, TFactory> : BrowsableObjectInfo, IBrowsableObjectInfo<TItems, TFactory> where TItems : BrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory
//    {

//        // IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

//        private TFactory _factory;

//        /// <summary>
//        /// Gets or sets the factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo.ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
//        /// <exception cref="ArgumentNullException">value is null.</exception>
//        public TFactory Factory
//        {

//            get => _factory;

//            set

//            {

//                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

//                TFactory oldFactory = _factory;

//                value.RegisterPath(this);

//                _factory = value;

//                oldFactory.UnregisterPath();

//            }

//        }

//        // IBrowsableObjectInfoLoader<IBrowsableObjectInfo> IBrowsableObjectInfo.ItemsLoader => (IBrowsableObjectInfoLoader)ItemsLoader;

//        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

//        //IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo> IBrowsableObjectInfo.RegisterLoader(IBrowsableObjectInfoLoader itemsLoader)
//        //{

//        //    if (object.ReferenceEquals(ItemsLoader, itemsLoader))

//        //        throw new InvalidOperationException("This items loader is already registered.");

//        //    ItemsLoader = object.ReferenceEquals(itemsLoader.Path, this) ? itemsLoader : throw new InvalidOperationException("Can not make a reference to the given items loader; the given items loader has to have registered the current path before calling the RegisterLoader method.");

//        //    return (IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo>) new PathModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor< BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this));

//        //}

//        //IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems> IBrowsableObjectInfo<TItems, TFactory, TParentItems>.RegisterLoader(IBrowsableObjectInfoLoader itemsLoader)
//        //{

//        //    if (object.ReferenceEquals(ItemsLoader, itemsLoader))

//        //        throw new InvalidOperationException("This items loader is already registered.");

//        //    ItemsLoader = object.ReferenceEquals(itemsLoader.Path, this) ? itemsLoader : throw new InvalidOperationException("Can not make a reference to the given items loader; the given items loader has to have registered the current path before calling the RegisterLoader method.");

//        //    return (IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems>)(IPathModifier<IBrowsableObjectInfo, IBrowsableObjectInfo>)new PathModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this));

//        //}

//        //void IBrowsableObjectInfo.UnregisterLoader()

//        //{

//        //    if (object.ReferenceEquals(ItemsLoader.Path, this))

//        //        throw new InvalidOperationException("Can not unregister the current items loader because it still references the current path. You need to unregister the current path from the current items loader before calling the UnregisterLoader method.");

//        //    ItemsLoader = null;

//        //}

//        // internal IBrowsableObjectInfoLoader<IBrowsableObjectInfo> ItemsLoaderInternal { set => ItemsLoader = (BrowsableObjectInfoLoader<BrowsableObjectInfo>)value; }

//        protected internal BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems> ItemCollection { get; }

//        /// <summary>
//        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
//        /// </summary>
//        public ReadOnlyBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems> Items { get; }

//        // IReadOnlyBrowsableObjectInfoCollection<IBrowsableObjectInfo> IBrowsableObjectInfo.Items => (IReadOnlyBrowsableObjectInfoCollection<IBrowsableObjectInfo>)Items;

//        // protected virtual IReadOnlyBrowsableObjectInfoCollection<IBrowsableObjectInfo> ItemsOverride { get; }

//        //protected virtual void OnItemsChanging(NotifyCollectionChangedEventArgs e)
//        //{

//        //    if (e.NewItems != null)

//        //        foreach (object item in e.NewItems)

//        //            if (item is TItems _browsableObjectInfo)

//        //                _browsableObjectInfo.Parent = Path;

//        //}

//        //private void ItemsChanging(object sender, NotifyCollectionChangedEventArgs e) => OnItemsChanging(e);

//        // IBrowsableObjectInfo IBrowsableObjectInfo.Parent => Parent;

//        // protected virtual WinCopies.Collections.ICollection<TItems> ItemsInternal { get; }

//        // private bool _considerAsPathRoot = false;

//        // public bool ConsiderAsPathRoot { get => _considerAsPathRoot; set => OnPropertyChanged(nameof(ConsiderAsPathRoot), nameof(_considerAsPathRoot), value, typeof(BrowsableObjectInfo)); }

//        /// <summary>
//        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
//        /// </summary>
//        /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
//        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
//        /// <param name="factory">The factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
//        /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
//        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
//        protected BrowsableObjectInfo(string path, FileType fileType, TFactory factory) : base(path, fileType)

//        {

//            ThrowOnInvalidFactoryUpdateOperation(factory, nameof(factory));

//            factory.UnregisterPath();

//            _factory = factory;

//            ItemCollection = GetNewItemCollection();

//            // ItemCollection.RegisterOwner((IPathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems>)new PathModifier<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(new BrowsableObjectInfoAccessor<IBrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this)));

//            Items = new ReadOnlyBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems>(ItemCollection);

//        }

//        protected virtual BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems> GetNewItemCollection() => (BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems>)new BrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory>, TItems>(this);

//        /// <summary>
//        /// Checks if an <see cref="IBrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo"/> and throw an exception if the validation failed.
//        /// </summary>
//        /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo"/> and in its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
//        /// <param name="paramName">The parameter name to include in error messages.</param>
//        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
//        /// <exception cref="ArgumentNullException"><paramref name="newFactory"/> is null.</exception>
//        protected virtual void ThrowOnInvalidFactoryUpdateOperation(IBrowsableObjectInfoFactory newFactory, string paramName)

//        {

//            if (ItemsLoader?.IsBusy == true)

//                throw new InvalidOperationException($"The {nameof(ItemsLoader)} is busy.");

//            if (newFactory is null)

//                throw new ArgumentNullException(paramName);

//            if (!(newFactory.Path is null))

//                throw new InvalidOperationException("The given factory has already been added to a BrowsableObjectInfo.");

//        }

//        // protected abstract IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileTypes fileType);

//        // /// <summary>
//        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> in memory.
//        // /// </summary>

//        // public abstract bool IsRenamingSupported { get; }

//        ///// <summary>
//        ///// When overridden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/> with the specified name.
//        ///// </summary>
//        ///// <param name="newValue">The new name or relative path for this <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</param>
//        //public abstract void Rename(string newValue);

//        protected override void Dispose(bool disposing)
//        {

//            base.Dispose(disposing);

//            if (disposing)

//                ItemCollection.Clear();

//        }

//        /// <summary>
//        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        /// </summary>
//        public override bool NeedsObjectsOrValuesReconstruction => (!(ItemsLoader is null) && ItemsLoader.NeedsObjectsOrValuesReconstruction) || Factory.NeedsObjectsOrValuesReconstruction;

//        protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
//        {

//            base.OnDeepClone(browsableObjectInfo);

//            var _browsableObjectInfo = (BrowsableObjectInfo<TItems, TFactory>)browsableObjectInfo;

//            _browsableObjectInfo.Factory = (TFactory)(IBrowsableObjectInfoFactory)_browsableObjectInfo.Factory.DeepClone();

//        }
//        //protected virtual IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> GetModifier() => (IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems>)new BrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TItems, TFactory, TParentItems>(this);

//        //public void AddTo(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Add(GetModifier());

//        //public void InsertTo(int index, IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Insert(index, GetModifier());

//        //public void RemoveFrom(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, TParentItems> collection) => collection.Remove(this);

//        //public void AddTo(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Add((IBrowsableObjectInfoModifier<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo>)GetModifier());

//        //public void InsertTo(int index, IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Insert(index, (IBrowsableObjectInfoModifier< BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo>)GetModifier());

//        //public void RemoveFrom(IBrowsableObjectInfoCollection<BrowsableObjectInfo<TItems, TFactory, TParentItems>, IBrowsableObjectInfo> collection) => collection.Remove(this);

//    }

//public abstract class AppInfo

//    //{

//    //    /// <summary>
//    //    /// When overridden in a derived class, gets the display name of this <see cref="AppInfo"/>.
//    //    /// </summary>
//    //    public abstract string DisplayName { get; }

//    //    /// <summary>
//    //    /// When overridden in a derived class, opens a given <see cref="ShellObject"/> using the application represented by this <see cref="AppInfo"/>.
//    //    /// </summary>
//    //    /// <param name="shellObject">The <see cref="ShellObject"/> to open.</param>
//    //    public abstract void Open(ShellObject shellObject);

//    //    /// <summary>
//    //    /// When overridden in a derived class, opens a given <see cref="StorageFile"/> using the application represented by this <see cref="AppInfo"/>.
//    //    /// </summary>
//    //    /// <param name="storageFile">The <see cref="StorageFile"/> to open.</param>
//    //    public abstract void Open(StorageFile storageFile);

//    //    /// <summary>
//    //    /// When overridden in a derived class, opens a given file using the application represented by this <see cref="AppInfo"/>.
//    //    /// </summary>
//    //    /// <param name="fileName">The path to the file to open.</param>
//    //    public abstract void Open(string fileName);

//    //}

//    //public class DesktopAppInfo // : AppInfo
//    //{

//    //    private ShellObject _shellObject;

//    //    /// <summary>
//    //    /// Gets the display name of this <see cref="DesktopAppInfo"/>.
//    //    /// </summary>
//    //    public string DisplayName => (_shellObject ?? (_shellObject = ShellObject.FromParsingName(Path))).Properties.System.FileDescription.Value;

//    //    private string _path;

//    //    /// <summary>
//    //    /// Gets the path of this <see cref="DesktopAppInfo"/>.
//    //    /// </summary>
//    //    public string Path => _path ?? (_path = GetOpenWithSoftwarePathFromCommand(Command));

//    //    /// <summary>
//    //    /// Gets the full command (software path and command line args) of this <see cref="DesktopAppInfo"/>.
//    //    /// </summary>
//    //    public string Command { get; } = null;

//    //    /// <summary>
//    //    /// Gets the Windows Registry file type of this <see cref="DesktopAppInfo"/>.
//    //    /// </summary>
//    //    public string FileType { get; } = null;

//    //    /// <summary>
//    //    /// Initializes a new instance of the <see cref="DesktopAppInfo"/> class for a given Windows Registry file type.
//    //    /// </summary>
//    //    /// <param name="fileType">The Windows Registry file type of the new instance of the <see cref="DesktopAppInfo"/>.</param>
//    //    public DesktopAppInfo(string fileType)

//    //    {

//    //        FileType = fileType;

//    //        Command = GetCommandFromFileType("open", FileType);

//    //    }

//    //    public void Open(ShellObject shellObject) => Open(shellObject.ParsingName, "open");

//    //    public bool Open(ShellObject shellObject, string commandName) => Open(shellObject.ParsingName, commandName);

//    //    public void Open(string fileName) => Open(fileName, "open");

//    //    public bool Open(string fileName, string commandName)

//    //    {

//    //        _ = Process.Start(GetOpenWithSoftwareProcessStartInfoFromCommand(GetCommandFromFileType(commandName, FileType), fileName));

//    //        return true;

//    //    }

//    //}

//    //public class WindowsStoreAppInfo : AppInfo

//    //{

//    //    /// <summary>
//    //    /// Gets the display name of this <see cref="WindowsStoreAppInfo"/>.
//    //    /// </summary>
//    //    public override string DisplayName => AppInfo.DisplayInfo.DisplayName;

//    //    /// <summary>
//    //    /// Gets the <see cref="Windows.ApplicationModel.AppInfo"/> of this <see cref="WindowsStoreAppInfo"/>.
//    //    /// </summary>
//    //    public Windows.ApplicationModel.AppInfo AppInfo { get; } = null;

//    //    /// <summary>
//    //    /// Initializes a new instance of the <see cref="WindowsStoreAppInfo"/> class for a given <see cref="Windows.ApplicationModel.AppInfo"/>.
//    //    /// </summary>
//    //    /// <param name="appInfo">The <see cref="Windows.ApplicationModel.AppInfo"/> of the instance of the <see cref="WindowsStoreAppInfo"/>.</param>
//    //    public WindowsStoreAppInfo(Windows.ApplicationModel.AppInfo appInfo) => AppInfo = appInfo;

//    //    public override void Open(ShellObject shellObject) => Open(shellObject.ParsingName);

//    //    public override void Open(StorageFile storageFile)

//    //    {

//    //        WindowsStoreAppLauncherInterop windowsStoreAppLauncherInterop = new WindowsStoreAppLauncherInterop(storageFile);

//    //        windowsStoreAppLauncherInterop.Loaded += (object sender, EventArgs e) => Open(windowsStoreAppLauncherInterop, false);

//    //        if (windowsStoreAppLauncherInterop.IsLoaded) Open(windowsStoreAppLauncherInterop, false);

//    //    }

//    //    public override void Open(string fileName)

//    //    {

//    //        WindowsStoreAppLauncherInterop winRTAppLauncherInterop = new WindowsStoreAppLauncherInterop(fileName);

//    //        winRTAppLauncherInterop.Loaded += (object sender, EventArgs e) => Open(winRTAppLauncherInterop, false);

//    //        if (winRTAppLauncherInterop.IsLoaded) Open(winRTAppLauncherInterop, false);

//    //    }

//    //    public void Open(WindowsStoreAppLauncherInterop winRTAppLauncherInterop, bool displayApplicationPicker) => winRTAppLauncherInterop.OpenFileUsingCustomApp(AppInfo, displayApplicationPicker);

//    //}

//    public class WindowsStoreAppLauncherInterop
//    {

//        //private static Dictionary<string, WinRTAppLauncherInterop> _winRTAppLauncherInterop = new Dictionary<string, WinRTAppLauncherInterop>();

//        //public static ReadOnlyDictionary<string, WinRTAppLauncherInterop> WinRTAppLauncherInteropObjects { get; } = new ReadOnlyDictionary<string, WinRTAppLauncherInterop>(_winRTAppLauncherInterop);

//        public bool IsLoaded { get; private set; } = false;

//        public string FileName { get; } = null;

//        public StorageFile File { get; private set; } = null;

//        public event EventHandler Loaded;

//        public event SucceededEventHandler FileOpened;

//        public WindowsStoreAppLauncherInterop(string fileName)
//        {

//            FileName = fileName;

//            string extension = System.IO.Path.GetExtension(fileName);

//            // _winRTAppLauncherInterop.Add(extension, this);

//            Launcher.FindFileHandlersAsync(extension).Completed = (IAsyncOperation<IReadOnlyList<Windows.ApplicationModel.AppInfo>> asyncInfo, AsyncStatus asyncStatus) =>

//            {

//                StorageFile.GetFileFromPathAsync(fileName).Completed = (IAsyncOperation<StorageFile> _asyncInfo, AsyncStatus _asyncStatus) => File = _asyncInfo.GetResults();

//                IsLoaded = true;

//                Loaded?.Invoke(this, new EventArgs());

//            };

//        }

//        public WindowsStoreAppLauncherInterop(StorageFile file)
//        {

//            File = file;

//            FileName = file.Path;

//            string extension = System.IO.Path.GetExtension(file.Path);

//            // _winRTAppLauncherInterop.Add(extension, this);

//            Launcher.FindFileHandlersAsync(extension).Completed = (IAsyncOperation<IReadOnlyList<Windows.ApplicationModel.AppInfo>> asyncInfo, AsyncStatus asyncStatus) =>

//            {

//                IsLoaded = true;

//                Loaded?.Invoke(this, new EventArgs());

//            };

//        }

//        public void OpenFileUsingDefaultApp(bool displayApplicationPicker) => Launcher.LaunchFileAsync(File, new LauncherOptions() { DisplayApplicationPicker = displayApplicationPicker }).Completed = OnOpening;

//        public void OpenFileUsingCustomApp(Windows.ApplicationModel.AppInfo appInfo, bool displayApplicationPicker)

//        {

//            LauncherOptions options = new LauncherOptions()
//            {

//                PreferredApplicationDisplayName = appInfo.DisplayInfo.DisplayName,

//                PreferredApplicationPackageFamilyName = appInfo.PackageFamilyName,

//                TargetApplicationPackageFamilyName = appInfo.PackageFamilyName,

//                DisplayApplicationPicker = displayApplicationPicker

//            };

//            try
//            {

//                Launcher.LaunchFileAsync(File, options).Completed = OnOpening;

//            }
//            catch { }

//        }

//        private void OnOpening(IAsyncOperation<bool> asyncInfo, AsyncStatus asyncStatus) => FileOpened?.Invoke(this, new SucceededEventArgs(asyncInfo.GetResults()));

//    }

//    public class WindowsStoreAppHandlersInterop

//    {

//        public Windows.ApplicationModel.AppInfo OpenWithAppInfo { get; private set; } = null;

//        public bool IsOpenWithAppInfoLoaded { get; private set; } = false;

//        public IReadOnlyList<Windows.ApplicationModel.AppInfo> OpenWithAppInfos { get; private set; } = null;

//        public bool AreOpenWithAppInfosLoaded { get; private set; } = false;

//        public string Extension { get; } = null;

//        public event EventHandler AppInfoLoaded;

//        public event EventHandler OpenWithAppInfoLoaded;

//        public WindowsStoreAppHandlersInterop(string extension) => Extension = extension;

//        public void GetFileHandlers() => Launcher.FindFileHandlersAsync(Extension).Completed = (IAsyncOperation<IReadOnlyList<Windows.ApplicationModel.AppInfo>> asyncInfo, AsyncStatus asyncStatus) =>

//                                                         {

//                                                             OpenWithAppInfos = asyncInfo.GetResults();

//                                                             AreOpenWithAppInfosLoaded = true;

//                                                             AppInfoLoaded?.Invoke(this, new EventArgs());

//                                                         };

//        public void GetOpenWithAppInfo()

//        {

//            void onAppInfoLoaded(object sender, EventArgs e)

//            {

//                AppInfoLoaded -= onAppInfoLoaded;

//                RegistryKey registryKey = GetFileTypeRegistryKey(GetFileTypeFromExtension(Extension));

//                if (registryKey?.OpenSubKey("shell\\open\\command") != null && registryKey.OpenSubKey("Application")?.GetValue("AppUserModelId") is string valueAsString)

//                    foreach (Windows.ApplicationModel.AppInfo appInfo in OpenWithAppInfos)

//                        if (appInfo.AppUserModelId == valueAsString)

//                        {

//                            IsOpenWithAppInfoLoaded = true;

//                            OpenWithAppInfo = appInfo;

//                            OpenWithAppInfoLoaded?.Invoke(this, new EventArgs());

//                            return;

//                        }

//            }

//            if (!AreOpenWithAppInfosLoaded)

//            {

//                AppInfoLoaded += onAppInfoLoaded;

//                GetFileHandlers();

//            }

//            else onAppInfoLoaded(null, null);

//        }

//    }

//    public class WinShellAppInfoInterop

//    {

//        public AppInfo OpenWithAppInfo { get; private set; } = null;

//        public bool IsOpenWithAppInfoLoaded { get; private set; } = false;

//        public AppInfo[] OpenWithAppInfos { get; private set; } = null;

//        public bool AreOpenWithAppInfosLoaded { get; private set; } = false;

//        public string Extension { get; } = null;

//        public event EventHandler OpenWithAppInfoLoaded;

//        public event EventHandler OpenWithAppInfosLoaded;

//        public WinShellAppInfoInterop(string extension) => Extension = extension;

//        public void LoadAppInfo()

//        {

//            string openWithSoftwareCommand = GetCommandFromExtension("open", Extension);

//            if (openWithSoftwareCommand == null)

//            {

//                WindowsStoreAppHandlersInterop winRTAppHandlersInterop = new WindowsStoreAppHandlersInterop(Extension);

//                winRTAppHandlersInterop.OpenWithAppInfoLoaded += (object sender, EventArgs e) =>

//                {

//                    OpenWithAppInfo = new WindowsStoreAppInfo(winRTAppHandlersInterop.OpenWithAppInfo);

//                    IsOpenWithAppInfoLoaded = true;

//                    OpenWithAppInfoLoaded?.Invoke(this, new EventArgs());

//                };

//                winRTAppHandlersInterop.GetOpenWithAppInfo();

//            }

//            else

//            {

//                OpenWithAppInfo = new DesktopAppInfo(GetFileTypeFromExtension(Extension));

//                IsOpenWithAppInfoLoaded = true;

//                OpenWithAppInfoLoaded?.Invoke(this, new EventArgs());

//            }

//        }

//        public void LoadAppInfos()

//        {

//            if (Extension == null)

//                throw new ArgumentNullException(nameof(Extension));

//            if (string.IsNullOrEmpty(Extension) || string.IsNullOrWhiteSpace(Extension))

//                throw new ArgumentException(string.Format((string)StringParameterEmptyOrWhiteSpaces, nameof(Extension)));

//            // RegistryKey[] subKeys = { Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes"), Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts"), Microsoft.Win32.Registry.ClassesRoot };

//            // string fileType = null;

//            ArrayAndListBuilder<AppInfo> appInfos = new ArrayAndListBuilder<AppInfo>();

//            // foreach (RegistryKey value in subKeys)

//            // {

//            appInfos.AddRangeLast(GetAppInfosByExtension(Extension/*, value*/));

//            // }

//            WindowsStoreAppHandlersInterop windowsStoreAppHandlersInterop = new WindowsStoreAppHandlersInterop(Extension);

//            windowsStoreAppHandlersInterop.AppInfoLoaded += (object sender, EventArgs e) =>

//            {

//                foreach (Windows.ApplicationModel.AppInfo value in windowsStoreAppHandlersInterop.OpenWithAppInfos)

//                    appInfos.AddLast(new WindowsStoreAppInfo(value));

//                OpenWithAppInfos = appInfos.ToArray();

//                AreOpenWithAppInfosLoaded = true;

//                OpenWithAppInfosLoaded?.Invoke(this, new EventArgs());

//            };

//            windowsStoreAppHandlersInterop.GetFileHandlers();

//        }

//    }

//public class KnownFolder : IShellObject

//{

//    public IKnownFolder Path { get; } = null;

//    object IShellObject.Path => Path;

//    public string ParsingName => Path.ParsingName;

//    public KnownFolder(IKnownFolder path) => Path = path;

//}

//public class ShellObject : IShellObject

//{

//    public Shell.ShellObject Path { get; } = null;

//    object IShellObject.Path => Path;

//    public string ParsingName => Path.ParsingName;

//    public ShellObject(Shell.ShellObject path) => Path = path;

//}

//public interface IShellObject

//{

//    object Path { get; }

//    string ParsingName { get; }

//}

//using SevenZip;

//using System;
//using System.Diagnostics;
//using System.Drawing;
//using System.Windows;
//using System.Windows.Interop;
//using System.Windows.Media.Imaging;

//using TsudaKageyu;

//using static WinCopies.Util.Util;
//using IfCT = WinCopies.Util.Util.ComparisonType;
//using IfCM = WinCopies.Util.Util.ComparisonMode;
//using IfComp = WinCopies.Util.Util.Comparison;
//using System.Linq;
//using System.Collections.Generic;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// Represents an archive that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
//    /// </summary>
//    public interface IArchiveItemInfo : IArchiveItemInfoProvider
//    {

//        /// <summary>
//        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
//        /// </summary>
//        ArchiveFileInfo? ArchiveFileInfo { get; }

//    }

//    public delegate ArchiveFileInfo? ArchiveFileInfoDeepClone(ArchiveFileInfo? obj, string archivePath);

//    public static class ArchiveItemInfo
//    {

//        public static ArchiveFileInfoDeepClone DefaultArchiveFileInfoDeepClone { get; } = (ArchiveFileInfo? archiveFileInfo, string archivePath) => archiveFileInfo.HasValue
//                ? (ArchiveFileInfo?)new SevenZipExtractor(archivePath).ArchiveFileData.First(item => item.FileName.ToLower() == archiveFileInfo.Value.FileName)
//                : null;

//    }

//    /// <summary>
//    /// Represents an archive that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
//    /// </summary>
//    public class ArchiveItemInfo<TItems, TFactory> : ArchiveItemInfoProvider<TItems, TFactory>, IArchiveItemInfo where TItems : BrowsableObjectInfo<TItems, TFactory>, IArchiveItemInfo where TFactory : IArchiveItemInfoFactory
//    {

//        // public override bool IsRenamingSupported => false;

//        /// <summary>
//        /// Gets the localized path of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override string LocalizedName => ArchiveShellObject.LocalizedName;

//        /// <summary>
//        /// Gets the name of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override string Name => System.IO.Path.GetFileName(Path);

//        /// <summary>
//        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

//        /// <summary>
//        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

//        /// <summary>
//        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

//        /// <summary>
//        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo{TItems, TFactory}"/> is browsable.
//        /// </summary>
//        public override bool IsBrowsable => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

//        ///// <summary>
//        ///// Gets or sets the factory for this <see cref="ArchiveItemInfo{TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ArchiveItemInfo{TItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/>.
//        ///// </summary>
//        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
//        ///// <exception cref="ArgumentNullException">value is null.</exception>
//        //public new ArchiveItemInfoFactory Factory { get => (ArchiveItemInfoFactory)base.Factory; set => base.Factory = value; }

//        /// <summary>
//        /// The factory used to create the new <see cref="IArchiveItemInfo"/>s.
//        /// </summary>
//        public sealed override IArchiveItemInfoFactory ArchiveItemInfoFactory { get => Factory; set => Factory = (TFactory)value; }

//        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

//        private readonly DeepClone<ArchiveFileInfo?> _archiveFileInfoDelegate;

//        /// <summary>
//        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
//        /// </summary>
//        public ArchiveFileInfo? ArchiveFileInfo { get; private set; }

//        /// <summary>
//        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
//        /// </summary>
//        public override IShellObjectInfo ArchiveShellObject { get; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class.
//        /// </summary>
//        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
//        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{TItems, TFactory}"/> represents a folder that exists implicitly in the archive.</param>
//        /// <param name="path">The full path to this archive item</param>
//        /// <param name="fileType">The file type of this archive item</param>
//        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) : this(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate, default) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class using a custom factory for <see cref="ArchiveItemInfo{TItems, TFactory}"/>s.
//        /// </summary>
//        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
//        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{TItems, TFactory}"/> represents a folder that exists implicitly in the archive.</param>
//        /// <param name="path">The full path to this archive item</param>
//        /// <param name="fileType">The file type of this archive item</param>
//        /// <param name="factory">The factory this <see cref="ArchiveItemInfo{TItems, TFactory}"/> and associated <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> use to create new instances of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class.</param>
//        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate, IArchiveItemInfoFactory factory) : base(path, fileType, (TFactory)(factory ?? new ArchiveItemInfoFactory<TItems>()))

//        {

//            if (fileType == FileType.SpecialFolder)

//                // todo:

//                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

//            _archiveFileInfoDelegate = archiveFileInfoDelegate;

//            ArchiveFileInfo = archiveFileInfo;

//            if (ArchiveFileInfo.HasValue && !path.EndsWith(ArchiveFileInfo.Value.FileName))

//                // todo:

//                throw new ArgumentException($"'{nameof(path)}' must end with '{nameof(ArchiveFileInfo.Value.FileName)}'");

//            ArchiveShellObject = archiveShellObject;

//#if DEBUG 

//            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

//#endif

//            // Path = path;

//#if DEBUG

//            Debug.WriteLine("path: " + path);

//#endif

//            #region Comments

//            // ArchiveFileInfo? archiveParentFileInfo = null;

//            // FileType _fileType = FileType.None;

//            // SevenZipExtractor archiveExtractor = new SevenZipExtractor(archiveShellObject.Path);

//            // System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

//            //foreach (ArchiveFileInfo _archiveFileInfo in archiveFileData)

//            //{

//            //if (_archiveFileInfo.FileName.Substring(0, path.LastIndexOf(IO.Path.PathSeparator)) == parent)

//            //{

//            // archiveParentFileInfo = _archiveFileInfo;

//            //_fileType = _archiveFileInfo.IsDirectory ? FileType.Folder : FileType.File;

//            //break;

//            //}

//            //else

//            // todo:

//            //throw new Exception("");

//            //}

//            #endregion

//        }

//        /// <summary>
//        /// Loads the items of this <see cref="ArchiveItemInfo{TItems, TFactory}"/> using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo<TItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        /// <summary>
//        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
//        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
//        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo<TItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

//        /// <summary>
//        /// When overridden in a derived class, returns the parent of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        /// <returns>the parent of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.</returns>
//        protected override IBrowsableObjectInfo GetParent()
//        {

//            IBrowsableObjectInfo result ;

//            if (Path.Length > ArchiveShellObject.Path.Length)

//            {

//                string path = Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator));

//                ArchiveFileInfo? archiveFileInfo = null;

//                using (var extractor = new SevenZipExtractor(ArchiveShellObject.Path))

//                    foreach (ArchiveFileInfo item in extractor .ArchiveFileData)

//                        if (item.FileName.ToLower() == path.ToLower())

//                            archiveFileInfo = item;

//                result = Factory.GetBrowsableObjectInfo(path, FileType.Folder, ArchiveShellObject, archiveFileInfo, _archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(_archiveFileInfo, ArchiveShellObject.Path) /*archiveParentFileInfo.Value*/);

//            }

//            else

//                result = ArchiveShellObject;

//                return result /*&& Path.Contains(IO.Path.PathSeparator)*/;

//        }

//        ///// <summary>
//        ///// Currently not implemented.
//        ///// </summary>
//        ///// <param name="newValue"></param>
//        //public override void Rename(string newValue) =>

//        //    // string getNewPath() => System.IO.Path.GetDirectoryName(Path) + IO.Path.PathSeparator + newValue;

//        //    //SevenZipCompressor a = new SevenZipCompressor();

//        //    //Dictionary<int, string> dico = new Dictionary<int, string>();

//        //    //dico.Add(ArchiveFileInfo.Index, ArchiveFileInfo.FileName);

//        //    //a.ModifyArchive(ArchiveShellObject.Path, dico);

//        //    // todo:

//        //    throw new NotSupportedException("This feature is currently not supported for the content archive items.");

//        // protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
//        // {

//        // base.OnDeepClone(browsableObjectInfo);

//        //using (var sevenZipExtractor = new SevenZipExtractor(ArchiveShellObject.Path))

//        //    browsableObjectInfo.ArchiveFileInfo = sevenZipExtractor.ArchiveFileData.FirstOrDefault(item => item.FileName == Path.Substring(ArchiveShellObject.Path.Length));

//        // }

//        /// <summary>
//        /// Gets a deep clone of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>. The <see cref="BrowsableObjectInfo.OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//        /// </summary>
//        protected override BrowsableObjectInfo DeepCloneOverride() => new ArchiveItemInfo<TItems, TFactory>(Path, FileType, (IShellObjectInfo)ArchiveShellObject.DeepClone(), _archiveFileInfoDelegate(ArchiveFileInfo), _archiveFileInfoDelegate, (ArchiveItemInfoFactory<TItems>)Factory.DeepClone());

//        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

//        /// <summary>
//        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//        /// </summary>
//        protected override void Dispose(bool disposing)
//        {

//            base.Dispose(disposing);

//            ArchiveShellObject.Dispose();

//        }

//        // public override string ToString() => System.IO.Path.GetFileName(Path);

//        private Icon TryGetIcon(System.Drawing.Size size) =>

//            // if (System.IO.Path.HasExtension(Path))

//            Microsoft.WindowsAPICodePack.Shell.FileOperation.GetFileInfo(System.IO.Path.GetExtension(Path), Microsoft.WindowsAPICodePack.Shell.FileAttributes.Normal, Microsoft.WindowsAPICodePack.Shell.GetFileInfoOptions.Icon | Microsoft.WindowsAPICodePack.Shell.GetFileInfoOptions.UseFileAttributes).Icon?.TryGetIcon(size, 32, true, true) ?? TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);// else// return TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);

//        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

//        {

//            using (Icon icon = TryGetIcon(size))

//                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

//        }

//        /// <summary>
//        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        /// </summary>
//        public override bool NeedsObjectsOrValuesReconstruction => true; // True because of the ShellObjectInfo's ShellObject

//        // public ArchiveFileInfo ArchiveFileInfo { get; } = null;

//        // public bool AreItemsLoaded { get => areItemsLoaded; private set => OnPropertyChanged(nameof(AreItemsLoaded), nameof(areItemsLoaded), value); }

//        // private ReadOnlyObservableCollection<IBrowsableObjectInfo> items = null;

//        // public event PropertyChangedEventHandler PropertyChanged;

//        //public ReadOnlyObservableCollection<IBrowsableObjectInfo> Items
//        //{

//        //    get => items;

//        //    public set

//        //    {

//        //        OnPropertyChanged(nameof(Items), nameof(items), value);

//        //        if (value != null)

//        //            AreItemsLoaded = true;

//        //    }

//        //}

//        // public FileTypes FileType { get; } = FileTypes.None;

//        //public BrowsableObjectInfoItemsLoader ItemsLoader
//        //{

//        //    get => ItemsLoader;

//        //    set => ItemsLoader = (FolderLoader)value;

//        //}

//    }

//}

//using System;
//using System.Text;
//using System.Windows.Media.Imaging;
//using static WinCopies.Util.Util;
//using System.Management;
//using System.Windows;
//using System.Windows.Interop;
//using System.Drawing;
//using System.Globalization;
//using WinCopies.Util;
//using System.Security;
//using static WinCopies.IO.WMIItemInfo;
//using static WinCopies.IO.Path;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// The WMI item type.
//    /// </summary>
//    public enum WMIItemType
//    {

//        /// <summary>
//        /// The WMI item is a namespace.
//        /// </summary>
//        Namespace,

//        /// <summary>
//        /// The WMI item is a class.
//        /// </summary>
//        Class,

//        /// <summary>
//        /// The WMI item is an instance.
//        /// </summary>
//        Instance

//    }

//    /// <summary>
//    /// Determines the WMI items to load.
//    /// </summary>
//    [Flags]
//    public enum WMIItemTypes
//    {

//        /// <summary>
//        /// Do not load any items.
//        /// </summary>
//        None = 0,

//        /// <summary>
//        /// Load the namespace items.
//        /// </summary>
//        Namespace = 1,

//        /// <summary>
//        /// Load the class items.
//        /// </summary>
//        Class = 2,

//        /// <summary>
//        /// Load the instance items.
//        /// </summary>
//        Instance = 4

//    }

//    public delegate ManagementObject ManagementObjectDeepClone(ManagementObject managementObject, SecureString password);

//    public delegate ManagementClass ManagementClassDeepClone(ManagementClass managementClass, SecureString password);

//    public delegate ConnectionOptions ConnectionOptionsDeepClone(ConnectionOptions connectionOptions, SecureString password);

//    public static class WMIItemInfo
//    {

//        public const string RootPath = @"\\.\ROOT:__NAMESPACE";
//        public const string NamespacePath = ":__NAMESPACE";
//        public const string NameConst = "Name";
//        public const string RootNamespace = "root:__namespace";
//        public const string ROOT = "ROOT";

//        public static ConnectionOptionsDeepClone DefaultConnectionOptionsDeepClone { get; } = (ConnectionOptions connectionOptions, SecureString password) => new ConnectionOptions()
//        {
//            Locale = connectionOptions.Locale,
//            Username = connectionOptions.Username,
//            SecurePassword = password,
//            Authority = connectionOptions.Authority,
//            Impersonation = connectionOptions.Impersonation,
//            Authentication = connectionOptions.Authentication,
//            EnablePrivileges = connectionOptions.EnablePrivileges,
//            Timeout = connectionOptions.Timeout
//        };

//        public static DeepClone<ManagementPath> DefaultManagementPathDeepClone { get; } = managementPath => new ManagementPath() { Path = managementPath.Path, ClassName = managementPath.ClassName, NamespacePath = managementPath.NamespacePath, RelativePath = managementPath.RelativePath, Server = managementPath.Server };

//        public static DeepClone<ObjectGetOptions> DefaultObjectGetOptionsDeepClone { get; } = objectGetOptions => new ObjectGetOptions() { Timeout = objectGetOptions.Timeout, UseAmendedQualifiers = objectGetOptions.UseAmendedQualifiers };

//        public static ManagementObjectDeepClone DefaultManagementObjectDeepClone { get; } = (ManagementObject managementObject, SecureString password) =>

//        {

//            ManagementObject _managementObject = managementObject as ManagementClass ?? managementObject as ManagementObject ?? throw new ArgumentException("managementObject must be a ManagementClass or a ManagementObject.", nameof(managementObject));

//            ManagementPath path = DefaultManagementPathDeepClone(_managementObject.Scope?.Path ?? _managementObject.Path);

//            return _managementObject is ManagementClass managementClass ? DefaultManagementClassDeepCloneDelegate(managementClass, null) : new ManagementObject(
//                new ManagementScope(
//    path,
//                    _managementObject.Scope?.Options is null ? null : DefaultConnectionOptionsDeepClone(_managementObject.Scope?.Options, password)
//                    ), path, _managementObject.Options is null ? null : DefaultObjectGetOptionsDeepClone(_managementObject.Options));

//        };

//        public static ManagementClassDeepClone DefaultManagementClassDeepCloneDelegate { get; } = (ManagementClass managementClass, SecureString password) =>

//        {

//            ManagementPath path = DefaultManagementPathDeepClone(managementClass.Scope?.Path ?? managementClass.Path);

//            return new ManagementClass(
//                new ManagementScope(
//    path,
//                    managementClass?.Scope?.Options is null ? null : DefaultConnectionOptionsDeepClone(managementClass?.Scope?.Options, password)
//                    ), path, managementClass.Options is null ? null : DefaultObjectGetOptionsDeepClone(managementClass.Options));

//        };

//        public static WMIItemInfoComparer<IWMIItemInfo> GetDefaultWMIItemInfoComparer() => new WMIItemInfoComparer<IWMIItemInfo>();

//        /// <summary>
//        /// Gets the name of the given <see cref="ManagementBaseObject"/>.
//        /// </summary>
//        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> for which get the name.</param>
//        /// <param name="wmiItemType">The <see cref="IO.WMIItemType"/> of <paramref name="managementObject"/>.</param>
//        /// <returns>The name of the given <see cref="ManagementBaseObject"/>.</returns>
//        public static string GetName(ManagementBaseObject managementObject, WMIItemType wmiItemType)

//        {

//            (managementObject as ManagementClass)?.Get();

//            const string name = NameConst;

//            return wmiItemType == WMIItemType.Namespace ? (string)managementObject[name] : managementObject.ClassPath.ClassName;

//        }

//        /// <summary>
//        /// Gets the path of the given <see cref="ManagementBaseObject"/>.
//        /// </summary>
//        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> for which get the path.</param>
//        /// <param name="wmiItemType">The <see cref="IO.WMIItemType"/> of <paramref name="managementObject"/>.</param>
//        /// <returns>The path of the given <see cref="ManagementBaseObject"/>.</returns>
//        public static string GetPath(ManagementBaseObject managementObject, WMIItemType wmiItemType)

//        {

//            string path = PathSeparator + managementObject.ClassPath.Server + PathSeparator + managementObject.ClassPath.NamespacePath;

//            string name = GetName(managementObject, wmiItemType);

//            if (name != null)

//                path += PathSeparator + name;

//            path += ":" + managementObject.ClassPath.ClassName;

//            return path;

//        }

//    }

//    public class WMIItemInfo<TItems, TFactory> : BrowsableObjectInfo<TItems, TFactory>, IWMIItemInfo where TItems : BrowsableObjectInfo<TItems, TFactory>, IWMIItemInfo where TFactory : IWMIItemInfoFactory
//    {

//        // public override bool IsRenamingSupported => false;

//        private DeepClone<ManagementBaseObject> _managementObjectDelegate;

//        /// <summary>
//        /// Gets the <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TItems, TFactory}"/> represents.
//        /// </summary>
//        public ManagementBaseObject ManagementObject { get; private set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfo{TItems, TFactory}"/> class as the root WMI item.
//        /// </summary>
//        public WMIItemInfo() : this(default) { }

//        // todo: throw exception if the given factory has not TParent and TItems as generic arguments

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfo{TItems, TFactory}"/> class as the root WMI item using a custom factory.
//        /// </summary>
//        public WMIItemInfo(TFactory factory) : this(RootPath, WMIItemType.Namespace, new ManagementClass(RootPath), (ManagementBaseObject managementObject) => DefaultManagementClassDeepCloneDelegate((ManagementClass)managementObject, null), factory) => IsRootNode = true;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfo{TItems, TFactory}"/> class. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo{TItems, TFactory}()"/> constructor.
//        /// </summary>
//        /// <param name="path">The path of this <see cref="WMIItemInfo{TItems, TFactory}"/></param>.
//        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{TItems, TFactory}"/>.</param>
//        /// <param name="managementObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method to get a new <see cref="ManagementBaseObject"/>.</param>
//        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TItems, TFactory}"/> represents.</param>
//        public WMIItemInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate) : this(path, wmiItemType, managementObject, managementObjectDelegate, default) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfo{TItems, TFactory}"/> class using a custom <see cref="IWMIItemInfoFactory"/>. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo{TItems, TFactory}()"/> constructor.
//        /// </summary>
//        /// <param name="path">The path of this <see cref="WMIItemInfo{TItems, TFactory}"/></param>.
//        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{TItems, TFactory}"/>.</param>
//        /// <param name="managementObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method to get a new <see cref="ManagementBaseObject"/>.</param>
//        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TItems, TFactory}"/> represents.</param>
//        /// <param name="factory">The factory this <see cref="WMIItemInfo{TItems, TFactory}"/> and associated <see cref="WMILoader{TPath, TItems, TFactory}"/> use to create new instances of the <see cref="WMIItemInfo{TItems, TFactory}"/> class.</param>
//        public WMIItemInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate, TFactory factory) : base(path, FileType.SpecialFolder, object.ReferenceEquals(factory, null) ? (TFactory)(IWMIItemInfoFactory)new WMIItemInfoFactory<TItems>() : factory)

//        {

//            ThrowIfNull(managementObjectDelegate, nameof(managementObjectDelegate));

//            ThrowIfNull(managementObject, nameof(managementObject));

//            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

//            _managementObjectDelegate = managementObjectDelegate;

//            ManagementObject = managementObject;

//            if (wmiItemType != WMIItemType.Instance)

//                Name = GetName(ManagementObject, wmiItemType);

//            WMIItemType = wmiItemType;

//            if (wmiItemType == WMIItemType.Namespace && Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

//                IsRootNode = true;

//        }

//        /// <summary>
//        /// Gets a new <see cref="WMIItemInfo{TItems, TFactory}"/> that corresponds to the given server name and relative path.
//        /// </summary>
//        /// <param name="serverName">The server name.</param>
//        /// <param name="serverRelativePath">The server relative path.</param>
//        /// <returns>A new <see cref="WMIItemInfo{TItems, TFactory}"/> that corresponds to the given server name and relative path.</returns>
//        /// <seealso cref="WMIItemInfo{TItems, TFactory}()"/>
//        /// <seealso cref="WMIItemInfo{TItems, TFactory}(string, WMIItemType, ManagementBaseObject, DeepClone{ManagementBaseObject})"/>
//        public static WMIItemInfo<TItems, TFactory> GetWMIItemInfo(string serverName, string serverRelativePath) => GetWMIItemInfo(serverName, serverRelativePath, (TFactory)(IWMIItemInfoFactory)new WMIItemInfoFactory<TItems>());

//        /// <summary>
//        /// Gets a new <see cref="WMIItemInfo{TItems, TFactory}"/> that corresponds to the given server name and relative path using a custom <see cref="WMIItemInfoFactory{TParent, TItems}"/>.
//        /// </summary>
//        /// <param name="serverName">The server name.</param>
//        /// <param name="serverRelativePath">The server relative path.</param>
//        /// <param name="factory">A custom factory.</param>
//        /// <returns>A new <see cref="WMIItemInfo{TItems, TFactory}"/> that corresponds to the given server name and relative path.</returns>
//        /// <seealso cref="WMIItemInfo{TItems, TFactory}()"/>
//        /// <seealso cref="WMIItemInfo{TItems, TFactory}(string, WMIItemType, ManagementBaseObject, DeepClone{ManagementBaseObject}, TFactory)"/>
//        public static WMIItemInfo<TItems, TFactory> GetWMIItemInfo(string serverName, string serverRelativePath, TFactory factory)

//        {

//            var stringBuilder = new StringBuilder();

//            _ = stringBuilder.Append(PathSeparator);

//            _ = stringBuilder.Append(PathSeparator);

//            _ = stringBuilder.Append(serverName);

//            _ = stringBuilder.Append(PathSeparator);

//            _ = stringBuilder.Append(IsNullEmptyOrWhiteSpace(serverRelativePath) ? ROOT : serverRelativePath);

//            _ = stringBuilder.Append(NamespacePath);

//            string path = stringBuilder.ToString();

//            return new WMIItemInfo<TItems, TFactory>(path, WMIItemType.Namespace, new ManagementClass(path), managementObject => DefaultManagementClassDeepCloneDelegate((ManagementClass)managementObject, null), factory);

//        }

//        //public static WMIItemInfo GetWMIItemInfo(string computerName, string serverClassRelativeName, string classMemberName)

//        //{

//        //    StringBuilder stringBuilder = new StringBuilder();

//        //    stringBuilder.Append(@IO.Path.PathSeparator);

//        //    stringBuilder.Append(computerName);

//        //    stringBuilder.Append(IO.Path.PathSeparator);

//        //    stringBuilder.Append(WinCopies.Util.Util.IsNullEmptyOrWhiteSpace(serverClassRelativeName) ? "ROOT" : serverClassRelativeName);

//        //    stringBuilder.Append(":");

//        //    stringBuilder.Append(classMemberName);

//        //    return new WMIItemInfo(stringBuilder.ToString(), WMIItemType.Class);

//        //}

//        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

//        {

//            int iconIndex = 0;

//            if (IsRootNode)

//                iconIndex = 15;

//            else if (WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class)

//                iconIndex = 3;

//            using (Icon icon = TryGetIcon(iconIndex, "shell32.dll", size))

//                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

//        }

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="WMIItemInfo{TItems, TFactory}"/> represents a root node.
//        /// </summary>
//        public bool IsRootNode { get; }

//        /// <summary>
//        /// Gets the localized path of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override string LocalizedName => Name;

//        /// <summary>
//        /// Gets the name of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override string Name { get; }

//        /// <summary>
//        /// Gets the small <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

//        /// <summary>
//        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

//        /// <summary>
//        /// Gets the large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

//        /// <summary>
//        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="WMIItemInfo{TItems, TFactory}"/> is browsable.
//        /// </summary>
//        public override bool IsBrowsable => WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class;

//        public WMIItemType WMIItemType { get; }

//        //public new WMIItemInfoFactory Factory { get => (WMIItemInfoFactory)base.Factory; set => base.Factory = value; }

//        protected override BrowsableObjectInfo DeepCloneOverride() => IsRootNode ? new WMIItemInfo<TItems, TFactory>((TFactory)Factory.DeepClone()) : new WMIItemInfo<TItems, TFactory>(Path, WMIItemType, _managementObjectDelegate(ManagementObject), _managementObjectDelegate, (TFactory)Factory.DeepClone());

//        public override bool NeedsObjectsOrValuesReconstruction => true;

//        protected override IBrowsableObjectInfo GetParent()
//        {

//            if (IsRootNode) return null;

//            string path;

//            switch (WMIItemType)

//            {

//                case WMIItemType.Namespace:

//                    path = Path.Substring(0, Path.LastIndexOf(PathSeparator)) + NamespacePath;

//                    return path.EndsWith(RootNamespace, true, CultureInfo.InvariantCulture)
//                        ? Factory.GetBrowsableObjectInfo()
//                        : Factory.GetBrowsableObjectInfo(path, WMIItemType.Namespace);

//                case WMIItemType.Class:

//                    return Path.EndsWith("root:" + Name, true, CultureInfo.InvariantCulture)
//                        ? Factory.GetBrowsableObjectInfo()
//                        : Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.IndexOf(':')) + NamespacePath, WMIItemType.Namespace);

//                case WMIItemType.Instance:

//                    path = Path.Substring(0, Path.IndexOf(':'));

//                    path = path.Substring(0, path.LastIndexOf(PathSeparator)) + ':' + path.Substring(path.LastIndexOf(PathSeparator) + 1);

//                    return Factory.GetBrowsableObjectInfo(path, WMIItemType.Class);

//                default: // We souldn't reach this point.

//                    return null;

//            }

//        }

//        private WMILoader<WMIItemInfo<TItems, TFactory>, TItems, TFactory> GetDefaultWMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) => new WMILoader<WMIItemInfo<TItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<WMIItemTypes>(), workerReportsProgress, workerSupportsCancellation);

//#pragma warning disable IDE0067 // Dispose objects before losing scope
//        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems(GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation));

//        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync(GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation));
//#pragma warning restore IDE0067 // Dispose objects before losing scope

//        ///// <summary>
//        ///// Not implemented.
//        ///// </summary>
//        ///// <param name="newValue"></param>
//        //public override void Rename(string newValue) => throw new NotImplementedException();

//        public override bool Equals(object obj) => ReferenceEquals(this, obj)
//                ? true : obj is IWMIItemInfo _obj ? WMIItemType == _obj.WMIItemType && Path.ToLower() == _obj.Path.ToLower()
//                : false;

//        public int CompareTo(IWMIItemInfo other) => GetDefaultWMIItemInfoComparer().Compare(this, other);

//        /// <summary>
//        /// Determines whether the specified <see cref="IWMIItemInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
//        /// </summary>
//        /// <param name="wmiItemInfo">The <see cref="IWMIItemInfo"/> to compare with the current object.</param>
//        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
//        public bool Equals(IWMIItemInfo wmiItemInfo) => Equals(wmiItemInfo as object);

//        /// <summary>
//        /// Gets an hash code for this <see cref="WMIItemInfo{TItems, TFactory}"/>.
//        /// </summary>
//        /// <returns>The hash code returned by the <see cref="FileSystemObject.GetHashCode"/> and the hash code of the <see cref="WMIItemType"/>.</returns>
//        public override int GetHashCode() => base.GetHashCode() ^ WMIItemType.GetHashCode();

//        /// <summary>
//        /// Disposes the current <see cref="WMIItemInfo{TItems, TFactory}"/> and its parent and items recursively.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy and does not support cancellation.</exception>
//        protected override void Dispose(bool disposing)
//        {

//            base.Dispose(disposing);

//            ManagementObject.Dispose();

//            if (disposing)

//            {

//                ManagementObject = null;

//                _managementObjectDelegate = null;

//            }

//        }

//    }

//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// Provides a base class for <see cref="BrowsableObjectInfo"/> factories.
//    /// </summary>
//    public abstract class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory

//    {

//        /// <summary>
//        /// Gets the <see cref="IBrowsableObjectInfo"/> associated to this <see cref="BrowsableObjectInfoFactory"/>.
//        /// </summary>
//        public IBrowsableObjectInfo Path { get; private set; }

//        void IBrowsableObjectInfoFactory.RegisterPath(IBrowsableObjectInfo path)
//        {

//            if ( HasPathRegistered )

//                throw new InvalidOperationException("A path is already registered.");

//            Path = object.ReferenceEquals(path.Factory, this) ? path : throw new InvalidOperationException("Can not make a reference to the given path; the given path has to have registered the current factory before calling the RegisterPath method.");

//            HasPathRegistered = true;

//        }

//        void IBrowsableObjectInfoFactory.UnregisterPath()

//        {

//            if (object.ReferenceEquals(Path.Factory, this))

//                throw new InvalidOperationException("Can not unregister the current path because it still references the current factory. You need to unregister the current factory from the current path before calling the UnregisterPath method.");

//            Path = null;

//            HasPathRegistered = false;

//        }

//        private bool _useRecursively;

//        /// <summary>
//        /// Whether to add the current <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from this <see cref="BrowsableObjectInfoFactory"/>.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">On setting: The <see cref="Path"/>'s <see cref="IBrowsableObjectInfo.ItemsLoader"/> of the current <see cref="BrowsableObjectInfoFactory"/> is busy.</exception>
//        public bool UseRecursively
//        {
//            get => _useRecursively; set

//            {

//                ThrowOnInvalidPropertySet(Path);

//                _useRecursively = value;

//            }
//        }

//        internal static void ThrowOnInvalidPropertySet(IBrowsableObjectInfo path)

//        {

//            if (path?.ItemsLoader?.IsBusy == true)

//                throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class and sets the <see cref="UseRecursively"/> property to <see langword="true"/>.
//        /// </summary>
//        protected BrowsableObjectInfoFactory() : this(false) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
//        /// </summary>
//        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="BrowsableObjectInfoFactory"/>.</param>
//        protected BrowsableObjectInfoFactory(bool useRecursively) => UseRecursively = useRecursively;

//        protected virtual void OnDeepClone(BrowsableObjectInfoFactory factory) { }

//        protected abstract BrowsableObjectInfoFactory DeepCloneOverride();

//        public virtual object DeepClone()

//        {

//            BrowsableObjectInfoFactory browsableObjectInfoFactory = DeepCloneOverride();

//            OnDeepClone(browsableObjectInfoFactory);

//            return browsableObjectInfoFactory;

//        }

//        /// <summary>
//        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        /// </summary>
//        public virtual bool NeedsObjectsOrValuesReconstruction => false;

//    }

//}

///// <summary>
///// The base class for all browsable items of the WinCopies framework.
///// </summary>
//public abstract class BrowsableObjectInfo<TItems, TFactory> : BrowsableObjectInfo, IBrowsableObjectInfo<TItems, TFactory> where TItems : BrowsableObjectInfo where TFactory : BrowsableObjectInfoFactory
//{

//    // IBrowsableObjectInfoFactory IBrowsableObjectInfo.Factory => _factory;

//    private TFactory _factory;

//    /// <summary>
//    /// Gets or sets the factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.
//    /// </summary>
//    /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo.ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
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
//    /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
//    /// </summary>
//    public ReadOnlyBrowsableObjectInfoCollection<TItems> Items { get; }

//    IReadOnlyBrowsableObjectInfoCollection<TItems> IBrowsableObjectInfo<TItems, TFactory>.Items => Items;

//    /// <summary>
//    /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
//    /// </summary>
//    /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
//    /// <param name="factory">The factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
//    /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
//    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
//    // /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
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
//    /// Checks if an <see cref="IBrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo"/> and throw an exception if the validation failed.
//    /// </summary>
//    /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo"/> and in its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
//    /// <param name="paramName">The parameter name to include in error messages.</param>
//    /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
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

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Collections;
//using WinCopies.Util;
//using static WinCopies.Util.Util;
//using IList = System.Collections.IList;

//namespace WinCopies.IO
//{

//    // todo: paths must have a protected virtual method to create their items collection. the path's constructor must call this method. using this implementation, a Unregister method does not make sense for the IBrowsableObjectInfoCollection interface.

//    //internal class BrowsableObjectInfoCollectionInternal<TOwner, TItems> : Collection<IBrowsableObjectInfoModifier<TOwner, TItems>> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
//    //{

//    //    internal new IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => base.Items;

//    //}

//    public interface IBrowsableObjectInfoCollection<TItems> :     WinCopies.Collections.IList<TItems>     where TItems : IBrowsableObjectInfo

//    {

//        IBrowsableObjectInfo Owner { get; }

//        void Remove(TItems item);

//    }

//    public interface IBrowsableObjectInfoCollection<TOwner, TItems> : IBrowsableObjectInfoCollection<TItems> /*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/ where TOwner : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

//    {

//        TOwner Owner { get; }

//        // void RegisterOwner(IPathModifier<TOwner, TItems> modifier);

//    }

//    public interface IReadOnlyBrowsableObjectInfoCollection<TItems> : WinCopies.Collections.IReadOnlyList<TItems>/*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/
//    {

//        new IBrowsableObjectInfo Owner { get; }

//    }

//    public class BrowsableObjectInfoCollection<TOwner, TItems> : Collection<TItems>, IBrowsableObjectInfoCollection<TOwner, TItems> where TOwner : BrowsableObjectInfo where TItems : BrowsableObjectInfo
//    {

//        //public void RegisterOwner(IPathModifier<TOwner, TItems> modifier)

//        //{

//        //    // if (object.ReferenceEquals(modifier.Owner, accessor.Owner))

//        //    if (object.ReferenceEquals(this, modifier.Accessor.ItemCollection))

//        //    {

//        //        _modifier = modifier;

//        //        // _innerList = new BrowsableObjectInfoCollectionInternal<TOwner, TItems>();

//        //    }

//        //    else

//        //        throw new ArgumentException("Invalid owner.");

//        //    // else

//        //    // throw new ArgumentException("Invalid owner.");

//        //}

//        public BrowsableObjectInfoCollection(TOwner owner) : this(new List<TItems>(), owner) { }

//        public BrowsableObjectInfoCollection(List<TItems> items, TOwner owner) : base(items) => Owner = !(Owner is null) ? owner : throw new InvalidOperationException("This collection already has an owner.");

//        // todo: check if is registered

//        public TOwner Owner { get; }

//        IBrowsableObjectInfo IBrowsableObjectInfoCollection<TItems>.Owner => Owner;

//        // private IPathModifier<TOwner, TItems> _modifier;

//        public virtual bool IsReadOnly => false;

//        public void Sort(int index, int count, System.Collections.Generic.IComparer<TItems> comparer) => ((List<TItems>)Items).Sort(index, count, comparer);

//        protected override void SetItem(int index, TItems item)
//        {

//            if (item.HasParent)

//                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

//            this[index].Parent = null;

//            this[index].HasParent = false;

//            base.SetItem(index, item);

//            this[index].Parent = Owner;

//            this[index].HasParent = true;

//        }

//        protected override void InsertItem(int index, TItems item)
//        {

//            if (item.HasParent)

//                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

//            base.InsertItem(index, item);

//            item.Parent = Owner;

//            item.HasParent = true;

//        }

//        protected override void RemoveItem(int index)

//        {

//            this[index].Parent = null;

//            this[index].HasParent = false;

//            base.RemoveItem(index);

//        }

//        void IBrowsableObjectInfoCollection<TItems>.Remove(TItems item) => Remove(item);

//        protected override void ClearItems()

//        {

//            for (int i = 0; i < Count; i++)

//            {

//                this[i].Parent = null;

//                this[i].HasParent = false;

//            }

//            base.ClearItems();

//            Owner.AreItemsLoaded = false;

//        }

//    }

//    public class ReadOnlyBrowsableObjectInfoCollection<TItems> : System.Collections.ObjectModel.ReadOnlyCollection<TItems>, IReadOnlyBrowsableObjectInfoCollection<TItems> where TItems : BrowsableObjectInfo

//    {

//        public ReadOnlyBrowsableObjectInfoCollection(IBrowsableObjectInfoCollection<TItems> items) : base(items) { }

//        // todo: test

//        public IBrowsableObjectInfo Owner => ((BrowsableObjectInfoCollection<BrowsableObjectInfo, TItems>)Items).Owner;

//        TItems Collections.IReadOnlyList<TItems>.this[int index] { get => this[index]; set => ((IList)this)[index] = value; }

//        void Collections.IReadOnlyList<TItems>.Clear() => ((IList)this).Clear();

//        void Collections.IReadOnlyList<TItems>.RemoveAt(int index) => ((IList)this).RemoveAt(index);

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using SevenZip;
//using System;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// A factory to create new <see cref="ArchiveItemInfo"/>s.
//    /// </summary>
//    public interface IArchiveItemInfoFactory : IBrowsableObjectInfoFactory
//    {

//        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate);

//    }

//    /// <summary>
//    /// A factory for creating new <see cref="ShellObjectInfo"/>s.
//    /// </summary>
//    public class ArchiveItemInfoFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
//    {

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ArchiveItemInfoFactory"/> class.
//        /// </summary>
//        public ArchiveItemInfoFactory() : base() { }

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) => new ArchiveItemInfo(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate);

//        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ArchiveItemInfoFactory();

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using SevenZip;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.IO;
//using System.Security;
//using WinCopies.Util;
//using static WinCopies.Util.Util;
//using static WinCopies.IO.FolderLoader;
//using WinCopies.Collections;

//namespace WinCopies.IO
//{

//    // todo: does not work with encrypted archives

//    public class ArchiveLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory> : FileSystemObjectLoader<TPath, TItems, TSubItems, TFactory> where TPath : ArchiveItemInfoProvider where TItems : ArchiveItemInfo where TSubItems : ArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory where TItemsFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
//    {

//        protected override BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> DeepCloneOverride() => new ArchiveLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>(default, FileTypes, (IFileSystemObjectComparer<IFileSystemObject>)FileSystemObjectComparer.DeepClone(), WorkerReportsProgress, WorkerSupportsCancellation);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> class.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="fileTypes">The file types to load.</param>
//        public ArchiveLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> class using a custom comparer.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
//        /// <param name="fileTypes">The file types to load.</param>
//        public ArchiveLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer, bool workerReportsProgress, bool workerSupportsCancellation) : base(path, fileTypes, (IFileSystemObjectComparer<IFileSystemObject>)fileSystemObjectComparer, workerReportsProgress, workerSupportsCancellation) { }

//        protected override void OnPathChanging( BrowsableObjectTreeNode< TPath, TItems, TFactory > path )

//        {

//            // if ((path is ShellObjectInfo && ((ShellObjectInfo)path).FileType == FileTypes.Archive) || path is ArchiveItemInfo)

//            // {

//            // this.path = path;

//            // PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(nameof(Path), null, path));

//            // }

//            /*else*/
//            if (!(path is null) && path.Value.FileType != FileType.Archive)

//                throw new ArgumentException("'Path' is not an Archive or a Folder.");

//            // _Paths = new ObservableCollection<IBrowsableObjectInfo>();

//        }

//        // protected override void OnProgressChanged(object sender, ProgressChangedEventArgs e) => PathsOverride.Add((ArchiveItemInfo)e.UserState);

//        protected override void OnDoWork(DoWorkEventArgs e)

//        {

//            if (FileTypes == FileTypes.None) return;

//            //else if (FileTypes.HasFlag(GetAllEnumFlags<FileTypes>()) && FileTypes.HasMultipleFlags())

//            //    throw new InvalidOperationException("FileTypes cannot have the All flag in combination with other flags.");

//#if DEBUG

//            Debug.WriteLine("Dowork event started.");

//            Debug.WriteLine(FileTypes);

//            try
//            {

//                Debug.WriteLine("Path == null: " + (Path == null).ToString());

//                Debug.WriteLine("Path.Path: " + Path?. Value. Path);

//                Debug.WriteLine("Path.ShellObject: " + (Path as IShellObjectInfo)?.ShellObject.ToString());

//            }
//#pragma warning disable CA1031 // Do not catch general exception types
//            catch (Exception) { }
//#pragma warning restore CA1031 // Do not catch general exception types

//#endif

//#if DEBUG

//            Debug.WriteLine("Dowork event started.");

//#endif

//            //List<FolderLoader.IPathInfo> directories = new List<FolderLoader.IPathInfo>();

//            //List<FolderLoader.IPathInfo> files = new List<FolderLoader.IPathInfo>();

//            var paths = new ArrayBuilder<PathInfo>();

//#if DEBUG

//            Debug.WriteLine("Path == null: " + (Path == null).ToString());

//            Debug.WriteLine("Path.Path: " + Path.Value.Path);

//            if (Path is IShellObjectInfo) Debug.WriteLine("Path.ShellObject: " + ((IShellObjectInfo)Path).ShellObject.ToString());

//#endif

//            // ShellObjectInfo archiveShellObject = Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject;

//            string archiveFileName = (Path is IShellObjectInfo ? (IShellObjectInfo)Path : ((IArchiveItemInfo)Path).ArchiveShellObject).Path;

//            try

//            {

//#if NETFRAMEWORK

//                using (var archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))

//                using (var archiveExtractor = new SevenZipExtractor(archiveFileStream))

//                {

//#else

//                using var archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

//                using var archiveExtractor = new SevenZipExtractor(archiveFileStream);

//#endif

//                //try
//                //{

//                // archiveShellObject.ArchiveFileStream = archiveFileStream;
//                void AddPath(ref string _path, FileType fileType, ref ArchiveFileInfo? archiveFileInfo)

//                {

//                    if (fileType == FileType.Other || (FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

//                    // We only make a normalized path if we add the path to the paths to load.

//                    string __path = string.Copy(_path);

//                    paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), fileType, archiveFileInfo, _archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(_archiveFileInfo, __path)));

//                }

//                void AddDirectory(string _path, ArchiveFileInfo? archiveFileInfo) =>

//                    // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

//                    AddPath(ref _path, FileType.Folder, ref archiveFileInfo);

//                void AddFile(string _path, ArchiveFileInfo? archiveFileInfo) =>

//                    // We only make a normalized path if we add the path to the paths to load.

//                    AddPath(ref _path, _path.Substring(_path.Length).EndsWith(".lnk")
//                        ? FileType.Link
//                        : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(_path)) ? FileType.Archive : FileType.File, ref archiveFileInfo);

//                System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

//                string fileName = "";

//                string relativePath = Path is IShellObjectInfo ? "" : Path.Value.Path.Substring(archiveFileName.Length + 1);

//                // PathInfo path;

//#if DEBUG

//                        foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

//                            Debug.WriteLine(archiveFileInfo.FileName);

//#endif

//                void addPath(ArchiveFileInfo archiveFileInfo)

//                {

//                    if (archiveFileInfo.FileName.StartsWith(relativePath) && archiveFileInfo.FileName.Length > relativePath.Length)

//                    {

//                        fileName = archiveFileInfo.FileName.Substring(relativePath.Length);

//                        if (fileName.StartsWith(IO.Path.PathSeparator))

//                            fileName = fileName.Substring(1);

//                        if (fileName.Contains(IO.Path.PathSeparator))

//                            fileName = fileName.Substring(0, fileName.IndexOf(IO.Path.PathSeparator));

//                        /*if (!archiveFileInfo.FileName.Substring(archiveFileInfo.FileName.Length).Contains(IO.Path.PathSeparator))*/

//                        // {

//                        foreach (IFileSystemObject pathInfo in (IEnumerable<IFileSystemObject>)paths)

//                            if (pathInfo.Path == fileName)

//                                return;

//                        if (fileName.ToLower() == archiveFileInfo.FileName.ToLower())

//                        {

//                            if (archiveFileInfo.IsDirectory)

//                                AddDirectory(fileName, archiveFileInfo);

//                            else if (CheckFilter(archiveFileInfo.FileName))

//                                AddFile(fileName, archiveFileInfo);

//                        }

//                        else

//                            AddDirectory(fileName, archiveFileInfo);

//                        // }

//                    }

//                }

//                foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

//                    // _path = archiveFileInfo.FileName.Replace('/', IO.Path.PathSeparator);

//                    addPath(archiveFileInfo);

//#if NETFRAMEWORK

//                }

//#endif

//            }

//            catch (Exception ex) when (ex.Is(false, typeof(IOException), typeof(SecurityException), typeof(UnauthorizedAccessException), typeof(SevenZipException))) { return; }

//            // for (int i = 0; i < paths.Count; i++)

//            // {

//            // PathInfo directory = (PathInfo)paths[i];

//            // string CurrentFile_Normalized = "";

//            // CurrentFile_Normalized = Util.GetNormalizedPath(directory.Path);

//            // directory.Normalized_Path = CurrentFile_Normalized;

//            // paths[i] = directory;

//            // }

//            IEnumerable<PathInfo> pathInfos;

//            if (FileSystemObjectComparer == null)

//                pathInfos = (IEnumerable<PathInfo>)paths;

//            else

//            {

//                var sortedPaths = paths.ToList();

//                sortedPaths.Sort(FileSystemObjectComparer);

//                pathInfos = (IEnumerable<PathInfo>)paths;

//            }

//            // for (int i = 0; i < files.Count; i++)

//            // {

//            // var file = (PathInfo)files[i];

//            // string CurrentFile_Normalized = "";

//            // CurrentFile_Normalized = FolderLoader.PathInfo.NormalizePath(file.Path);

//            // file.Normalized_Path = CurrentFile_Normalized;

//            // files[i] = file;

//            // }

//            // files.Sort(comp);



//#if DEBUG

//            void reportProgress(PathInfo path)

//            {

//                Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
//                Debug.WriteLine("path_.Path: " + path.Path);
//                Debug.WriteLine("path_.Normalized_Path: " + path.NormalizedPath);
//                // Debug.WriteLine("path_.Shell_Object: " + path.ArchiveShellObject);

//                // var new_Path = ((ArchiveItemInfo)Path).ArchiveShellObject;
//                // new_Path.LoadThumbnail();

//                ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>(    (TItems)Path.Factory.GetBrowsableObjectInfo(Path.Value.Path + IO.Path.PathSeparator + path.Path, path.FileType, Path.Value.ArchiveShellObject, path.ArchiveFileInfo, archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(archiveFileInfo, Path.Value.ArchiveShellObject.Path)), (TItemsFactory) Path.Factory.DeepClone()));

//                // #if DEBUG

//                // Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

//                // #endif

//            }

//#endif

//            // this._Paths = new ObservableCollection<IBrowsableObjectInfo>();



//            PathInfo path_;



//#if NETFRAMEWORK

//            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())

//#else

//            using IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator();

//#endif

//            while (_paths.MoveNext())

//                try

//                {

//                    do

//                    {

//                        path_ = _paths.Current;

//#if DEBUG

//                            reportProgress(path_);

//#else

//                        ReportProgress(0, ((IArchiveItemInfoProvider)Path).Factory.GetBrowsableObjectInfo(((IArchiveItemInfoProvider)Path).ArchiveShellObject, path.ArchiveFileInfo, Path.Path + IO.Path.PathSeparator + path.Path, path.FileType));

//#endif

//                    } while (_paths.MoveNext());

//                }
//                catch (Exception ex) when (HandleIOException(ex)) { }

//            //foreach (FolderLoader.PathInfo path_ in files)

//            //    reportProgressAndAddNewPathToObservableCollection(path_);

//        }

//        protected class PathInfo : IO.PathInfo

//        {

//            public FileType FileType { get; }

//            public ArchiveFileInfo? ArchiveFileInfo { get; }

//            public DeepClone<ArchiveFileInfo?> ArchiveFileInfoDelegate { get; }

//            /// <summary>
//            /// Gets the localized name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string LocalizedName => Name;

//            /// <summary>
//            /// Gets the name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string Name => System.IO.Path.GetFileName(Path);

//            public PathInfo(string path, string normalizedPath, FileType fileType, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) : base(path, normalizedPath)
//            {

//                ArchiveFileInfo = archiveFileInfo;

//                ArchiveFileInfoDelegate = archiveFileInfoDelegate;

//                FileType = fileType;

//            }

//            //public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
//            //        ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
//            //        : false;

//            //public int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

//        }

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.IO
//{

//    /// <summary>
//    /// Provides a base class for <see cref="BrowsableObjectInfo"/> factories.
//    /// </summary>
//    public abstract class BrowsableObjectInfoFactory /*: IBrowsableObjectInfoFactory*/

//    {

//        //public static bool ValidatePropertySet(IBrowsableObjectInfoFactory factory) => factory.Loader?.IsBusy == false;

//        //public static void ThrowOnInvalidPropertySet(IBrowsableObjectInfoFactory factory)

//        //{

//        //    if ( factory.Loader?.IsBusy == true)

//        //        throw new InvalidOperationException($"The Path's ItemsLoader of the current {nameof(BrowsableObjectInfoFactory)} is busy.");

//        //}

//        public IBrowsableObjectInfo Path { get; }

//        //protected virtual void OnDeepClone(BrowsableObjectInfoFactory factory) { }

//        //protected abstract BrowsableObjectInfoFactory DeepCloneOverride();

//        //public virtual object DeepClone()

//        //{

//        //    BrowsableObjectInfoFactory browsableObjectInfoFactory = DeepCloneOverride();

//        //    OnDeepClone(browsableObjectInfoFactory);

//        //    return browsableObjectInfoFactory;

//        //}

//        ///// <summary>
//        ///// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        ///// </summary>
//        //public virtual bool NeedsObjectsOrValuesReconstruction => false;

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;
//using WinCopies.Collections;
//using WinCopies.Util;

//using IDisposable = WinCopies.Util.IDisposable;

//namespace WinCopies.IO
//{

//    //    /// <summary>
//    //    /// Provides a base class for <see cref="IBrowsableObjectInfo"/> loading. See the Remarks section.
//    //    /// </summary>
//    //    /// <remarks>This class provides the <see cref="PathOverride"/> protected property. This property is for interoperability with generic classes based on this one. You should override this property in order to seal it.</remarks>
//    //    public abstract class BrowsableObjectInfoLoader : IBrowsableObjectInfoLoader/*, IBackgroundWorker2*/

//    //    {

//    //#pragma warning disable IDE0069 // Disposed in the Dispose method overrides.
//    //        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
//    //#pragma warning restore IDE0069
//    //#pragma warning disable CS0649 // Set up using reflection
//    //        private readonly IFileSystemObjectComparer<IFileSystemObject> _fileSystemObjectComparer;
//    //        private readonly IEnumerable<string> _filter;
//    //#pragma warning restore CS0649

//    //        public IFileSystemObjectComparer<IFileSystemObject> FileSystemObjectComparer { get => _fileSystemObjectComparer; set => this.SetBackgroundWorkerProperty(nameof(FileSystemObjectComparer), nameof(_fileSystemObjectComparer), value, typeof(BrowsableObjectInfoLoader), true); }

//    //        //public void changePath(IBrowsableObjectInfo newValue)

//    //        //{



//    //        //}

//    //        //protected virtual BrowsableObjectInfo PathOverride { get => _path; set => _path = value; }

//    //        public IEnumerable<string> Filter { get => _filter; set => this.SetBackgroundWorkerProperty(nameof(Filter), nameof(_filter), value, typeof(BrowsableObjectInfoLoader), true); }

//    //        /// <summary>
//    //        /// Gets a value that indicates whether the thread is busy.
//    //        /// </summary>
//    //        public bool IsBusy => backgroundWorker.IsBusy;

//    //        /// <summary>
//    //        /// Gets or sets a value that indicates whether the thread can notify of the progress.
//    //        /// </summary>
//    //        public bool WorkerReportsProgress { get => backgroundWorker.WorkerReportsProgress; set => backgroundWorker.WorkerReportsProgress = value; }

//    //        /// <summary>
//    //        /// Gets or sets a value that indicates whether the thread supports cancellation.
//    //        /// </summary>
//    //        public bool WorkerSupportsCancellation { get => backgroundWorker.WorkerSupportsCancellation; set => backgroundWorker.WorkerSupportsCancellation = value; }

//    //        /// <summary>
//    //        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
//    //        /// </summary>
//    //        public ApartmentState ApartmentState { get => backgroundWorker.ApartmentState; set => backgroundWorker.ApartmentState = value; }

//    //        /// <summary>
//    //        /// Gets a value that indicates whether the thread must try to cancel before finished the background tasks.
//    //        /// </summary>
//    //        public bool CancellationPending => backgroundWorker.CancellationPending;

//    //        /// <summary>
//    //        /// Gets a value that indicates whether the working has been cancelled.
//    //        /// </summary>
//    //        public bool IsCancelled => backgroundWorker.IsCancelled;

//    //        /// <summary>
//    //        /// Gets the current progress of the working in percent.
//    //        /// </summary>
//    //        public int Progress => backgroundWorker.Progress;

//    //        /// <summary>
//    //        /// Gets or sets the <see cref="ISite"/> associated with the <see cref="IComponent"/>.
//    //        /// </summary>
//    //        /// <value>The <see cref="ISite"/> object associated with the component; or <see langword="null"/>, if the component does not have a site.</value>
//    //        /// <remarks>Sites can also serve as a repository for container-specific, per-component information, such as the component name.</remarks>
//    //        public ISite Site { get => backgroundWorker.Site; set => backgroundWorker.Site = value; }

//    //        /// <summary>
//    //        /// <para>Called when the background thread starts. Put your background working code here.</para>
//    //        /// <para>The event handler is running in the background thread.</para>
//    //        /// </summary>
//    //        public event DoWorkEventHandler DoWork;

//    //        /// <summary>
//    //        /// <para>Called when the background thread reports progress.</para>
//    //        /// <para>The event handler is running in the main thread.</para>
//    //        /// </summary>
//    //        public event ProgressChangedEventHandler ProgressChanged;

//    //        /// <summary>
//    //        /// <para>Called when the background thread has finished working.</para>
//    //        /// <para>The event handler is running in the background thread.</para>
//    //        /// </summary>
//    //        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

//    //        /// <summary>
//    //        /// Represents the method that handles the <see cref="Disposed"/> event of a component.
//    //        /// </summary>
//    //        /// <remarks>When you create a <see cref="Disposed"/> delegate, you identify the method that handles the event. To associate the event with your event handler, add an instance of the delegate to the event. The event handler is called whenever the event occurs, unless you remove the delegate. For more information about event handler delegates, see <a href="https://docs.microsoft.com/en-us/dotnet/standard/events/index?view=netframework-4.8">Handling and Raising Events</a>.</remarks>
//    //        public event EventHandler Disposed;

//    //        /// <summary>
//    //        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class.
//    //        /// </summary>
//    //        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//    //        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//    //        protected BrowsableObjectInfoLoader(bool workerReportsProgress, bool workerSupportsCancellation) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IFileSystemObject>()) { }

//    //        /// <summary>
//    //        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class using a custom comparer.
//    //        /// </summary>
//    //        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//    //        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//    //        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
//    //        protected BrowsableObjectInfoLoader(bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer)
//    //        {

//    //            WorkerReportsProgress = workerReportsProgress;

//    //            WorkerSupportsCancellation = workerSupportsCancellation;

//    //            FileSystemObjectComparer = fileSystemObjectComparer;

//    //            ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProgressChanged(e);

//    //            backgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged(this, e);

//    //            DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

//    //            backgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => DoWork(this, e);

//    //            RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerCompleted(e);

//    //            backgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => RunWorkerCompleted(this, e);

//    //            backgroundWorker.Disposed += (object sender, EventArgs e) => Disposed?.Invoke(this, e);

//    //        }

//    //        public bool IsDisposing { get; internal set; }

//    //        public bool IsDisposed { get; internal set; }

//    //        /// <summary>
//    //        /// This method does anything because it is designed for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation, and is here for overriding only. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool)"/>, you'll have to override this method if your class has to reinitialize members.
//    //        /// </summary>
//    //        /// <param name="browsableObjectInfoLoader">The cloned <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>.</param>
//    //        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
//    //        protected virtual void OnDeepClone(BrowsableObjectInfoLoader browsableObjectInfoLoader, bool? preserveIds) { }

//    //        /// <summary>
//    //        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>. The <see cref="OnDeepClone(BrowsableObjectInfoLoader{TPath}, bool)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool)"/>, you'll have to override this method if your class has to reinitialize members.
//    //        /// </summary>
//    //        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
//    //        protected abstract BrowsableObjectInfoLoader DeepCloneOverride(bool? preserveIds);

//    //        public object DeepClone(bool? preserveIds)

//    //        {

//    //            ((IDisposable)this).ThrowIfDisposingOrDisposed();

//    //            BrowsableObjectInfoLoader browsableObjectInfoLoader = DeepCloneOverride(preserveIds);

//    //            OnDeepClone(browsableObjectInfoLoader, preserveIds);

//    //            return browsableObjectInfoLoader;

//    //        }

//    //        public virtual bool NeedsObjectsOrValuesReconstruction => FileSystemObjectComparer.NeedsObjectsOrValuesReconstruction;

//    //        public virtual bool CheckFilter(string path)

//    //        {

//    //            if (Filter is null) return true;

//    //            foreach (string filter in Filter)

//    //                if (!IO.Path.MatchToFilter(path, filter)) return false;

//    //            return true;

//    //        }

//    //        /// <summary>
//    //        /// Notifies of the progress.
//    //        /// </summary>
//    //        /// <param name="percentProgress">
//    //        /// Progress percentage.
//    //        /// </param>
//    //        public void ReportProgress(int percentProgress) => backgroundWorker.ReportProgress(percentProgress);

//    //        /// <summary>
//    //        /// Notifies of the progress.
//    //        /// </summary>
//    //        /// <param name="percentProgress">
//    //        /// Progress percentage.
//    //        /// </param>
//    //        /// <param name="userState">
//    //        /// User object.
//    //        /// </param>
//    //        public void ReportProgress(int percentProgress, object userState) => backgroundWorker.ReportProgress(percentProgress, userState);

//    //        public static FileTypes FileTypeToFileTypeFlags(FileType fileType)

//    //        {

//    //            fileType.ThrowIfNotValidEnumValue();

//    //            if (fileType == FileType.SpecialFolder) throw new ArgumentException("'" + nameof(fileType) + "' must be None, Folder, File, Drive, Link or Archive. '" + nameof(fileType) + "' is " + fileType.ToString() + ".");

//    //            switch (fileType)

//    //            {

//    //                case FileType.Other:

//    //                    return FileTypes.None;

//    //                case FileType.Folder:

//    //                    return FileTypes.Folder;

//    //                case FileType.File:

//    //                    return FileTypes.File;

//    //                case FileType.Drive:

//    //                    return FileTypes.Drive;

//    //                case FileType.Link:

//    //                    return FileTypes.Link;

//    //                case FileType.Archive:

//    //                    return FileTypes.Archive;

//    //                default:

//    //                    // This point should never be reached.

//    //                    throw new NotImplementedException();

//    //            }

//    //        }

//    //        /// <summary>
//    //        /// When overridden in a derived class, provides a handler for the <see cref="DoWork"/> event.
//    //        /// </summary>
//    //        /// <param name="e">Event args for the current event</param>
//    //        protected abstract void OnDoWork(DoWorkEventArgs e);

//    //        // /// <summary>
//    //        // /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class with an <see cref="IBrowsableObjectInfo"/>.
//    //        // /// </summary>
//    //        // /// <param name="path">The path from which load items.</param>
//    //        // public BrowsableObjectInfoItemsLoader(IBrowsableObjectInfo path) { path.ItemsLoader = this; }

//    //        /// <summary>
//    //        /// Loads the items of the <see cref="IBrowsableObjectInfoLoader.Path"/> object.
//    //        /// </summary>
//    //        public virtual void LoadItems() => OnDoWork(new DoWorkEventArgs(null));

//    //        /// <summary>
//    //        /// Loads the items of the <see cref="IBrowsableObjectInfoLoader.Path"/> object asynchronously.
//    //        /// </summary>
//    //        public virtual void LoadItemsAsync() => backgroundWorker.RunWorkerAsync();

//    //        /// <summary>
//    //        /// Cancels the working asynchronously.
//    //        /// </summary>
//    //        public void CancelAsync(object stateInfo) => backgroundWorker.CancelAsync(stateInfo);

//    //        public void CancelAsync() => CancelAsync(null);

//    //        /// <summary>
//    //        /// Cancels the working.
//    //        /// </summary>
//    //        public void Cancel(object stateInfo) => backgroundWorker.Cancel(stateInfo);

//    //        public void Cancel() => Cancel(null);

//    //        /// <summary>
//    //        /// Suspends the current thread.
//    //        /// </summary>
//    //        public void Suspend() => backgroundWorker.Suspend();

//    //        /// <summary>
//    //        /// Resumes the current thread.
//    //        /// </summary>
//    //        public void Resume() => backgroundWorker.Resume();

//    //        IBrowsableObjectInfo Path => PathOverride;

//    //        /// <summary>
//    //        /// This property is only itented for use in this class and in classes that derive directly from this class and sould be sealed in derived classes and not used directly. You can use the <see cref="Path"/> property instead.
//    //        /// </summary>
//    //        /// <seealso cref="Path"/>
//    //        protected abstract IBrowsableObjectInfo PathOverride { get; set; }

//    //#pragma warning disable CA1063 // Implement IDisposable Correctly: Implementation of IDisposable is enhanced for this class.
//    //        /// <summary>
//    //        /// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>.
//    //        /// </summary>
//    //        public void Dispose() => Dispose(false);

//    //        /// <summary>
//    //        /// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>.
//    //        /// </summary>
//    //        /// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//    //        public void Dispose(bool disposePath)

//    //        {

//    //            IsDisposing = true;

//    //            Dispose(true, disposePath);

//    //            GC.SuppressFinalize(this);

//    //            IsDisposed = true;

//    //            IsDisposing = false;

//    //        }

//    //        ///// <summary>
//    //        ///// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and optionally disposes the related <see cref="Path"/>.
//    //        ///// </summary>
//    //        ///// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//    //        //protected sealed override void DisposeOverride(bool disposing) => Dispose(disposing, false);

//    //        /// <summary>
//    //        /// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and optionally disposes the related <see cref="Path"/>.
//    //        /// </summary>
//    //        /// <param name="disposePath">Whether to dispose the related <see cref="Path"/>. If this parameter is set to <see langword="true"/>, the <see cref="IBrowsableObjectInfo.ItemsLoader"/>s of the parent and childs of the related <see cref="Path"/> will be disposed recursively.</param>
//    //        /// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//    //        protected virtual void Dispose(bool disposing, bool disposePath)

//    //        {

//    //            // base.DisposeOverride(disposing);

//    //            if (disposePath)

//    //                PathOverride.Dispose();

//    //            if (disposing)

//    //                PathOverride = null;

//    //        }

//    //        protected abstract void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e);
//    //        protected abstract void OnAddingPath(IBrowsableObjectInfo path);
//    //        protected abstract void OnProgressChanged(ProgressChangedEventArgs e);

//    //#pragma warning restore CA1063 // Implement IDisposable Correctly: Implementation of IDisposable is enhanced for this class.

//    //        /// <summary>
//    //        /// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and optionally disposes the related <see cref="Path"/>.
//    //        /// </summary>
//    //        /// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//    //        protected virtual void DisposeOverride(bool disposing) => backgroundWorker.Dispose();

//    //        ~BrowsableObjectInfoLoader()

//    //        {

//    //            DisposeOverride(false);

//    //        }

//    //    }

//    public abstract class BrowsableObjectInfoLoader<TPath, TItems> : IBrowsableObjectInfoLoader<TPath,TItems> where TPath : BrowsableObjectInfo where TItems : BrowsableObjectInfo 
//    {
//        public IFileSystemObjectComparer<TItems> FileSystemObjectComparer { get ; set ; }

//        //public void changePath(IBrowsableObjectInfo newValue)

//        //{



//        //}

//        //protected virtual BrowsableObjectInfo PathOverride { get => _path; set => _path = value; }

//        public IEnumerable<IBrowsableObjectInfoLoaderFilter> Filter { get ; set ; }

//        public TPath Path { get; }

//        protected System.Collections.Generic.LinkedList<TItems> Items { get; }

//        ///// <summary>
//        ///// This method does anything because it is designed for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation, and is here for overriding only. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//        ///// </summary>
//        ///// <param name="browsableObjectInfoLoader">The cloned <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>.</param>
//        //protected virtual void OnDeepClone(BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> browsableObjectInfoLoader) { }

//        ///// <summary>
//        ///// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>. The <see cref="OnDeepClone(BrowsableObjectInfoLoader{TPath, TItems, TFactory})"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
//        ///// </summary>
//        //protected abstract BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> DeepCloneOverride();

//        //public object DeepClone()

//        //{

//        //    Util.Util.throw

//        //    ((IDisposable)this).ThrowIfDisposingOrDisposed();

//        //    BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> browsableObjectInfoLoader = DeepCloneOverride();

//        //    OnDeepClone(browsableObjectInfoLoader);

//        //    return browsableObjectInfoLoader;

//        //}

//        //public virtual bool NeedsObjectsOrValuesReconstruction => FileSystemObjectComparer.NeedsObjectsOrValuesReconstruction;

//        public virtual bool CheckFilter(string path)

//        {

//            if (Filter is null) return true;

//            foreach (string filter in Filter)

//                if (!IO.Path.MatchToFilter(path, filter)) return false;

//            return true;

//        }

//        public static FileTypes FileTypeToFileTypeFlags(FileType fileType)

//        {

//            fileType.ThrowIfNotValidEnumValue();

//            if (fileType == FileType.SpecialFolder) throw new ArgumentException("'" + nameof(fileType) + "' must be None, Folder, File, Drive, Link or Archive. '" + nameof(fileType) + "' is " + fileType.ToString() + ".");

//#if NETFRAMEWORK

//            switch (fileType)
//            {
//                case FileType.Other:
//                    return FileTypes.None;
//                case FileType.Folder:
//                    return FileTypes.Folder;
//                case FileType.File:
//                    return FileTypes.File;
//                case FileType.Drive:
//                    return FileTypes.Drive;
//                case FileType.Link:
//                    return FileTypes.Link;
//                case FileType.Archive:
//                    return FileTypes.Archive;
//                default:
//                    throw new NotImplementedException(); // This point should never be reached.
//            };

//#else

//            return fileType switch
//            {
//                FileType.Other => FileTypes.None,
//                FileType.Folder => FileTypes.Folder,
//                FileType.File => FileTypes.File,
//                FileType.Drive => FileTypes.Drive,
//                FileType.Link => FileTypes.Link,
//                FileType.Archive => FileTypes.Archive,
//                _ => throw new NotImplementedException() // This point should never be reached.
//            };

//#endif
//        }

//        // /// <summary>
//        // /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class with an <see cref="IBrowsableObjectInfo"/>.
//        // /// </summary>
//        // /// <param name="path">The path from which load items.</param>
//        // public BrowsableObjectInfoItemsLoader(IBrowsableObjectInfo path) { path.ItemsLoader = this; }

//        public abstract IList<TItems> PreloadItems();

//        ///// <summary>
//        ///// Loads the items of the <see cref="IBrowsableObjectInfoLoader.Path"/> object.
//        ///// </summary>
//        public abstract void LoadItems();

//        /// <summary>
//        /// Suspends the current thread.
//        /// </summary>
//        public abstract void Suspend();

//        /// <summary>
//        /// Resumes the current thread.
//        /// </summary>
//        public abstract void Resume();

//        /// <summary>
//        /// Cancels the current thread.
//        /// </summary>
//        public abstract void Cancel();

//        //IBrowsableObjectInfo Path => PathOverride;

//        ///// <summary>
//        ///// This property is only itented for use in this class and in classes that derive directly from this class and sould be sealed in derived classes and not used directly. You can use the <see cref="Path"/> property instead.
//        ///// </summary>
//        ///// <seealso cref="Path"/>
//        //protected abstract IBrowsableObjectInfo PathOverride { get; set; }

//        ///// <summary>
//        ///// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/>.
//        ///// </summary>
//        ///// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//        //public void Dispose()

//        //{

//        //    IsDisposing = true;

//        //    Dispose(true);

//        //    GC.SuppressFinalize(this);

//        //    IsDisposed = true;

//        //    IsDisposing = false;

//        //}

//        ///// <summary>
//        ///// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and optionally disposes the related <see cref="Path"/>.
//        ///// </summary>
//        ///// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//        //protected sealed override void DisposeOverride(bool disposing) => Dispose(disposing, false);

//        ///// <summary>
//        ///// Disposes the current <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and optionally disposes the related <see cref="Path"/>.
//        ///// </summary>
//        ///// <exception cref="InvalidOperationException">This <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> is busy and does not support cancellation.</exception>
//        //protected virtual void Dispose(bool disposing)

//        //{

//        //    // base.DisposeOverride(disposing);

//        //    backgroundWorker.Dispose();

//        //    if (disposing)

//        //        Path = null;

//        //}

//        //protected abstract void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e);

//        // protected abstract void OnAddingPath(IBrowsableObjectInfo path);

//        //protected abstract void OnProgressChanged(ProgressChangedEventArgs e);

//        //#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
//        //        ~BrowsableObjectInfoLoader()
//        //#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

//        //        {

//        //            Dispose(false);

//        //        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        protected BrowsableObjectInfoLoader(BrowsableObjectTreeNode path, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class using a custom comparer.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
//        protected BrowsableObjectInfoLoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer, bool workerReportsProgress, bool workerSupportsCancellation)

//        {

//            WorkerReportsProgress = workerReportsProgress;

//            WorkerSupportsCancellation = workerSupportsCancellation;

//            FileSystemObjectComparer = fileSystemObjectComparer;

//            ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProgressChanged(e);

//            backgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged(this, e);

//            DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

//            backgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => DoWork(this, e);

//            RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerCompleted(e);

//            backgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => RunWorkerCompleted(this, e);

//            backgroundWorker.Disposed += (object sender, EventArgs e) => Disposed?.Invoke(this, e);

//            // todo: internal set because this is an initialization

//            Path = path;

//        }

//        // protected IPathModifier<TPath, TItems> PathModifier { get; private set; }

//        private BrowsableObjectTreeNode<TPath, TItems, TFactory> _path;

//        ///// <summary>
//        ///// This property is here only for interoperability with the non-generic base class <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> and is only intented for use in the <see cref="BrowsableObjectInfoLoader{TPath}"/> generic class and should not be used directly.
//        ///// </summary>
//        ///// <seealso cref="BrowsableObjectInfoLoader.Path"/>
//        ///// <seealso cref="BrowsableObjectInfoLoader{TPath}.Path"/>
//        //protected sealed override IBrowsableObjectInfo PathOverride { get => Path; set => Path = (TPath) value; } 

//        IBrowsableObjectTreeNode<TPath, TItems> IBrowsableObjectInfoLoader<TPath, TItems>.Path => Path;

//        /// <summary>
//        /// Gets the path from which to load the items.
//        /// </summary>
//        public BrowsableObjectTreeNode<TPath, TItems, TFactory> Path
//        {
//            get => _path; set

//            {

//                if (IsBusy)

//                    throw new InvalidOperationException("The items loader is busy.");

//                if (!(value?.ItemsLoader is null))

//                    throw new InvalidOperationException("The given path has already been added to an items loader.");

//                OnPathChanging(value);

//                if (_path is object)

//                    _path.ItemsLoader = null;

//                _path = value;

//                if (value is object)

//                    value.ItemsLoader = this;

//                OnPathChanged(value);

//            }
//        }

//        //protected void Reset()

//        //{

//        //    if (object.ReferenceEquals(_path, null)) return;

//        //    if (!_path.IsBrowsable)

//        //        throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, _path.FileType.ToString(), _path.ToString()));

//        //    else if (IsBusy)

//        //        Cancel();

//        //    Path. ItemCollection.Clear();

//        //}

//        protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e) => Path.AreItemsLoaded = true;

//        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
//        {

//            Debug.WriteLine("e.UserState is BrowsableObjectTreeNode<TItems, TSubItems, TFactory>: " + (e.UserState is BrowsableObjectTreeNode<TItems, TSubItems, TFactory>).ToString());

//            Debug.WriteLine("e.UserState.GetType(): " + e.UserState.GetType().ToString());

//            try
//            {

//                if (e.UserState is ITreeNode item)

//                    Path.Insert(Path.Count, (ReadOnlyTreeNode<TItems>)item);

//                Debug.WriteLine("azerty: " + Path.Count);

//            }
//            catch (Exception ex) { Debug.WriteLine("azerty: " + ex.Message); }

//        }

//        /// <summary>
//        /// Provides ability for classes that derive from this one to do operations when the path is changing.
//        /// </summary>
//        /// <param name="path">The new path to set the <see cref="Path"/> property with.</param>
//        protected virtual void OnPathChanging(BrowsableObjectTreeNode<TPath, TItems, TFactory> path) { }

//        protected virtual void OnPathChanged(BrowsableObjectTreeNode<TPath, TItems, TFactory> path) { }

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System.Collections.Generic;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    public interface IFileSystemObjectLoader : IBrowsableObjectInfoLoader
//    {

//        FileTypes FileTypes { get; set; }

//    }

//    public abstract class FileSystemObjectLoader<TPath, TItems, TSubItems, TFactory> : BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory>, IFileSystemObjectLoader where TPath : FileSystemObjectInfo where TItems : FileSystemObjectInfo where TSubItems : FileSystemObjectInfo where TFactory : BrowsableObjectInfoFactory
//    {

//        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

//        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<TPath, TItems, TSubItems, TFactory>), true); }

//        protected FileSystemObjectLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) { }

//        protected FileSystemObjectLoader( BrowsableObjectTreeNode< TPath, TItems, TFactory > path, FileTypes fileTypes, IFileSystemObjectComparer<IFileSystemObject> browsableObjectInfoComparer, bool workerReportsProgress, bool workerSupportsCancellation) : base(path, browsableObjectInfoComparer, workerReportsProgress, workerSupportsCancellation) => _fileTypes = fileTypes;

//    }
//}

/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    ///// <summary>
//    ///// Provides common properties for <see cref="IBrowsableObjectInfo"/> factories.
//    ///// </summary>
//    //public interface IBrowsableObjectInfoFactory /*: IDeepCloneable*/

//    //{

//    //    // TODO:

//    //    // IBackgroundWorker Loader { get; }

//    //}

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Collections;
//using WinCopies.Util;
//using IDisposable = WinCopies.Util.IDisposable;

//namespace WinCopies.IO
//{

//    public interface IBrowsableObjectInfoLoaderFilter
//    {

//        string FileName { get; }

//    }

//    public class BrowsableObjectInfoLoaderFilter : IBrowsableObjectInfoLoaderFilter
//    {

//        public string FileName { get; }

//        public BrowsableObjectInfoLoaderFilter(string fileName) => FileName = fileName;

//    }

//    ///// <summary>
//    ///// Represents a loader that can be used to load <see cref="IBrowsableObjectInfo"/>. Note: An <see cref="InvalidOperationException"/> is thrown when the <see cref="System.IDisposable.Dispose"/> method is called if this <see cref="IBrowsableObjectInfoLoader{T}"/> is busy and does not support cancellation.
//    ///// </summary>
//    public interface IBrowsableObjectInfoLoader<TPath, TItems> /* : IBackgroundWorker, IDeepCloneable, IDisposable*/ where TPath : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

//    {

//        IFileSystemObjectComparer<TItems> FileSystemObjectComparer { get; set; }

//        IEnumerable<IBrowsableObjectInfoLoaderFilter> Filter { get; set; }

//        TPath Path { get; }

//        //bool CheckFilter(string path);

//        int PreloadItems();

//        void LoadItems();

//    }

//    //public interface IBrowsableObjectInfoLoader<TPath, TItems> : IBrowsableObjectInfoLoader where TPath : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

//    //{

//    //    IBrowsableObjectTreeNode<TPath, TItems> Path { get; }

//    //}

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using Microsoft.Win32;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;

//namespace WinCopies.IO
//{
//    public interface IRegistryItemInfoFactory : IBrowsableObjectInfoFactory
//    {

//        IBrowsableObjectInfo GetBrowsableObjectInfo();

//        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate);

//        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath);

//        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName);

//        IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName);

//    }
//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using Microsoft.WindowsAPICodePack.Shell;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;

//namespace WinCopies.IO
//{
//    /// <summary>
//    /// A factory to create new <see cref="IBrowsableObjectInfo"/>s.
//    /// </summary>
//    public interface IShellObjectInfoFactory : IBrowsableObjectInfoFactory
//    {

//        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

//        /// <summary>
//        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
//        /// </summary>
//        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
//        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
//        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate);

//    }
//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Management;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    public interface IWMIItemInfoFactory : IBrowsableObjectInfoFactory
//    {

//        IWMIItemInfoFactoryOptions Options { get; }

//        /// <summary>
//        /// Gets a new instance of the <see cref="IBrowsableObjectInfo"/> class.
//        /// </summary>
//        /// <returns>A new instance of the <see cref="IBrowsableObjectInfo"/> class.</returns>
//        IBrowsableObjectInfo GetBrowsableObjectInfo();

//        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate);

//        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using Microsoft.Win32;
//using System;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    public class RegistryItemInfoFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
//    {

//        /// <summary>
//        /// Initializes a new instance of the <see cref="RegistryItemInfoFactory"/> class.
//        /// </summary>
//        public RegistryItemInfoFactory() : base() { }

//        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new RegistryItemInfoFactory();

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new RegistryItemInfo();

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate) => new RegistryItemInfo(registryKey, registryKeyDelegate);

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath) => new RegistryItemInfo(registryKeyPath);

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, string valueName) => new RegistryItemInfo(registryKey, registryKeyDelegate, valueName);

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string registryKeyPath, string valueName) => new RegistryItemInfo(registryKeyPath, valueName);

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Reflection;
//using System.Security;

//using Microsoft.Win32;
//using WinCopies.Collections;
//using WinCopies.Util;
//using static WinCopies.Util.Util;

//namespace WinCopies.IO
//{

//    [Flags]
//    public enum RegistryItemTypes

//    {

//        None = 0,

//        RegistryKey = 1,

//        RegistryValue = 2

//    }

//    public interface IRegistryKeyLoader : IBrowsableObjectInfoLoader
//    {

//        RegistryItemTypes RegistryItemTypes { get; set; }

//    }

//    public class RegistryKeyLoader: BrowsableObjectInfoLoader, IRegistryKeyLoader where TPath : RegistryItemInfo where TItems : RegistryItemInfo where TSubItems : RegistryItemInfo where TFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory where TItemsFactory : BrowsableObjectInfoFactory, IRegistryItemInfoFactory
//    {

//        //protected override BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> DeepCloneOverride() => new RegistryKeyLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>(default, RegistryItemTypes, (IFileSystemObjectComparer<IFileSystemObject>)FileSystemObjectComparer.DeepClone(), WorkerReportsProgress, WorkerSupportsCancellation);

//        private readonly RegistryItemTypes _registryItemTypes = RegistryItemTypes.None;

//        public RegistryItemTypes RegistryItemTypes
//        {

//            get => _registryItemTypes;

//            set => _ = this.SetBackgroundWorkerProperty(nameof(RegistryItemTypes), nameof(_registryItemTypes), value, typeof(RegistryKeyLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>), true);

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="RegistryKeyLoader{TPath, TItems, TSubItems, TFactory, TItemsFactory}"/> class.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="registryItemTypes">The registry item types to load.</param>
//        public RegistryKeyLoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, RegistryItemTypes registryItemTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, registryItemTypes, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) => RegistryItemTypes = registryItemTypes;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="RegistryKeyLoader{TPath, TItems, TSubItems, TFactory, TItemsFactory}"/> class using a custom comparer.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
//        /// <param name="registryItemTypes">The registry item types to load.</param>
//        public RegistryKeyLoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, RegistryItemTypes registryItemTypes, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer, bool workerReportsProgress, bool workerSupportsCancellation) : base(path, (IFileSystemObjectComparer<IFileSystemObject>)fileSystemObjectComparer, workerReportsProgress, workerSupportsCancellation) => _registryItemTypes = registryItemTypes;

//        //public override bool CheckFilter(string path)

//        //{

//        //    if (Filter == null) return true;

//        //    foreach (string filter in Filter)

//        //    {

//        //        bool checkFilters(string[] filters)

//        //        {

//        //            foreach (string _filter in filters)

//        //            {

//        //                if ( string.IsNullOrEmpty( _filter ) ) continue;

//        //                if (path.Length >= _filter.Length && path.Contains(_filter))

//        //                    path = path.Substring(path.IndexOf(_filter) + _filter.Length);

//        //                else return false;

//        //            }

//        //            return true;

//        //        }

//        //        return checkFilters(filter.Split('*'));

//        //    }

//        //    return true;

//        //}

//        protected override void OnDoWork(DoWorkEventArgs e)
//        {

//            if (RegistryItemTypes == RegistryItemTypes.None)

//                return;

//            // todo: 'if' to remove if not necessary:

//            // if (Path is IRegistryItemInfo registryItemInfo)

//            // {

//            var paths = new ArrayBuilder<PathInfo>();

//            PathInfo pathInfo;

//            void checkAndAppend(string pathWithoutName, string name, bool isValue)

//            {

//                string path = pathWithoutName + IO.Path.PathSeparator + name;

//                if (CheckFilter(path))

//                    _ = paths.AddLast(pathInfo = new PathInfo(path, path.RemoveAccents(), name, null, RegistryItemInfo.DefaultRegistryKeyDeepClone, isValue));

//            }

//            switch (Path.Value.RegistryItemType)

//            {

//                case RegistryItemType.RegistryRoot:

//                    if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

//                    {

//                        FieldInfo[] _registryKeyFields = typeof(Microsoft.Win32.Registry).GetFields();

//                        string name;

//                        foreach (FieldInfo fieldInfo in _registryKeyFields)

//                        {

//                            name = ((RegistryKey)fieldInfo.GetValue(null)).Name;

//                            checkAndAppend(name, name, false);

//                        }

//                    }

//                    break;

//                case RegistryItemType.RegistryKey:

//                    string[] items;

//                    if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

//                        try

//                        {

//                            items = Path.Value.RegistryKey.GetSubKeyNames();

//                            foreach (string item in items)

//                                checkAndAppend(item.Substring(0, item.LastIndexOf(IO.Path.PathSeparator)), item.Substring(item.LastIndexOf(IO.Path.PathSeparator) + 1), false);

//                        }

//                        catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

//                    if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryValue))

//                        try

//                        {

//                            items = Path.Value.RegistryKey.GetValueNames();

//                            foreach (string item in items)

//                                checkAndAppend(Path.Value.RegistryKey.Name, item, true);

//                        }

//                        catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

//                    break;

//            }



//            IEnumerable<PathInfo> pathInfos;



//            if (FileSystemObjectComparer == null)

//                pathInfos = (IEnumerable<PathInfo>)paths;

//            else

//            {

//                var _paths = paths.ToList();

//                _paths.Sort(FileSystemObjectComparer);

//                pathInfos = (IEnumerable<PathInfo>)_paths;

//            }



//            using IEnumerator<PathInfo> pathsEnum = pathInfos.GetEnumerator();

//            while (pathsEnum.MoveNext())

//                try

//                {

//                    do

//                        ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>((TItems)(pathsEnum.Current.IsValue ? ((IRegistryItemInfoFactory)Path.Factory).GetBrowsableObjectInfo(pathsEnum.Current.Path.Substring(0, pathsEnum.Current.Path.Length - pathsEnum.Current.Name.Length - 1 /* We remove one more character to remove the backslash between the registry key path and the registry key value name. */ ), pathsEnum.Current.Name) : Path.Factory.GetBrowsableObjectInfo(pathsEnum.Current.Path)), (TItemsFactory)Path.Factory.DeepClone()));

//                    while (pathsEnum.MoveNext());

//                }

//                catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }



//            // }

//        }

//        protected class PathInfo : IO.PathInfo
//        {

//            /// <summary>
//            /// Gets the localized name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string LocalizedName => Name;

//            /// <summary>
//            /// Gets the name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string Name { get; }

//            public bool IsValue { get; }

//            public RegistryKey RegistryKey { get; }

//            public DeepClone<RegistryKey> RegistryKeyDelegate { get; }

//            public PathInfo(string path, string normalizedPath, string name, RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, bool isValue) : base(path, normalizedPath)
//            {

//                Name = name;

//                RegistryKey = registryKey;

//                RegistryKeyDelegate = registryKeyDelegate;

//                IsValue = isValue;

//            }
//        }
//    }
//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using Microsoft.WindowsAPICodePack.Shell;
//using System;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    // todo: create a read-only wrapper for shellobjectinfo

//    /// <summary>
//    /// A factory to create new <see cref="ShellObjectInfo"/>s.
//    /// </summary>
//    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
//    {

//        /// <summary>
//        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        /// </summary>
//        public override bool NeedsObjectsOrValuesReconstruction => ArchiveItemInfoFactory?.NeedsObjectsOrValuesReconstruction == true;

//        public IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

//        // IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ShellObjectInfoFactory"/> class.
//        /// </summary>
//        public ShellObjectInfoFactory() : this(null) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
//        /// </summary>
//        /// <param name="archiveItemInfoFactory">The <see cref="ArchiveItemInfoFactory"/> this factory will use for creating new items.</param>
//        public ShellObjectInfoFactory(ArchiveItemInfoFactory archiveItemInfoFactory) : base() => ArchiveItemInfoFactory = archiveItemInfoFactory;

//        /// <summary>
//        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
//        /// </summary>
//        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
//        /// <param name="fileType">The file type of the new item.</param>
//        /// <param name="specialFolder">The special folder type of the new item.</param>
//        /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) => new ShellObjectInfo(path, fileType, specialFolder, shellObject, shellObjectDelegate);

//        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ShellObjectInfoFactory((ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone());

//    }

//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Management;
//using WinCopies.Util;

//namespace WinCopies.IO
//{

//    public class WMIItemInfoFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory // where TItems : BrowsableObjectInfo, IWMIItemInfo
//    {

//        /// <summary>
//        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
//        /// </summary>
//        public override bool NeedsObjectsOrValuesReconstruction => !(Options is null);

//        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new WMIItemInfoFactory((WMIItemInfoFactoryOptions)Options?.DeepClone());

//        private WMIItemInfoFactoryOptions _options;

//        public WMIItemInfoFactoryOptions Options
//        {

//            get => _options; set
//            {

//                // ThrowOnInvalidPropertySet(this);

//                _options.Factory = null;

//                value.Factory = this;

//                _options = value;

//            }

//        }

//        IWMIItemInfoFactoryOptions IWMIItemInfoFactory.Options => _options;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class.
//        /// </summary>
//        public WMIItemInfoFactory() : base() { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WMIItemInfoFactory"/> class using custom options.
//        /// </summary>
//        public WMIItemInfoFactory(WMIItemInfoFactoryOptions options) : base() => _options = options;

//        /// <summary>
//        /// Gets a new instance of the <see cref="WMIItemInfo"/> class.
//        /// </summary>
//        /// <returns>A new instance of the <see cref="WMIItemInfo"/> class.</returns>
//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new WMIItemInfo();

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType) => GetBrowsableObjectInfo(path, wmiItemType, new ManagementObject(new ManagementScope(path, _options?.ConnectionOptions is null ? null : WMIItemInfo.DefaultConnectionOptionsDeepClone(_options?.ConnectionOptions, null)), new ManagementPath(path), _options?.ObjectGetOptions is null ? null : WMIItemInfo.DefaultObjectGetOptionsDeepClone(_options?.ObjectGetOptions)), _managementObject => _managementObject is ManagementClass managementClass ? WMIItemInfo.DefaultManagementClassDeepCloneDelegate(managementClass, null) : _managementObject is ManagementObject __managementObject ? WMIItemInfo.DefaultManagementObjectDeepClone(__managementObject, null) : throw new ArgumentException("The given object must be a ManagementClass or a ManagementObject.", "managementObject"));

//        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate) => new WMIItemInfo(path, wmiItemType, managementObject, managementObjectDelegate);
//    }
//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Management;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;

//namespace WinCopies.IO
//{
//    public interface IWMIItemInfoFactoryOptions : IDeepCloneable
//    {

//        IWMIItemInfoFactory Factory { get; }

//        ConnectionOptions ConnectionOptions { get; set; }

//        ObjectGetOptions ObjectGetOptions { get; set; }

//        EnumerationOptions EnumerationOptions { get; set; }

//    }

//    public class WMIItemInfoFactoryOptions : IWMIItemInfoFactoryOptions
//    {

//        public IWMIItemInfoFactory Factory { get; internal set; }

//        private ConnectionOptions _connectionOptions;

//        private readonly DeepClone<ConnectionOptions> _connectionOptionsDelegate;

//        /// <summary>
//        /// Gets or sets options for the WMI connections.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        protected virtual ConnectionOptions ConnectionOptionsOverride { get => _connectionOptions; set => _connectionOptions = value; }

//        /// <summary>
//        /// Gets or sets options for the WMI connections.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        public ConnectionOptions ConnectionOptions
//        {
//            get => ConnectionOptionsOverride; set
//            {

//                if (!(Factory is null))

//                    // BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory);

//                ConnectionOptionsOverride = value;

//            }
//        }

//        private ObjectGetOptions _objectGetOptions;

//        private readonly DeepClone<ObjectGetOptions> _objectGetOptionsDelegate;

//        /// <summary>
//        /// Gets or sets options for getting management objects.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        protected virtual ObjectGetOptions ObjectGetOptionsOverride
//        {
//            get => _objectGetOptions; set =>

//                // if (!(Factory is null))

//                // BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory);

//                _objectGetOptions = value;
//        }

//        /// <summary>
//        /// Gets or sets options for getting management objects.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        public ObjectGetOptions ObjectGetOptions

//        {

//            get => ObjectGetOptionsOverride; set =>

//                // if (!(Factory is null))

//                // BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory);

//                ObjectGetOptionsOverride = value;

//        }

//        private EnumerationOptions _enumerationOptions;

//        private readonly DeepClone<EnumerationOptions> _enumerationOptionsDelegate;

//        /// <summary>
//        /// Gets or sets options for management objects.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        protected virtual EnumerationOptions EnumerationOptionsOverride { get => _enumerationOptions; set => _enumerationOptions = value; }

//        /// <summary>
//        /// Gets or sets options for management objects.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IBrowsableObjectInfoLoader{T}"/> is busy.</exception>
//        public virtual EnumerationOptions EnumerationOptions
//        {
//            get => EnumerationOptionsOverride; set =>

//                // if (!(Factory is null))

//                // BrowsableObjectInfoFactory.ThrowOnInvalidPropertySet(Factory);

//                EnumerationOptionsOverride = value;
//        }

//        public bool NeedsObjectsOrValuesReconstruction => true;

//        public WMIItemInfoFactoryOptions() : this(null, null, null) { }

//        public WMIItemInfoFactoryOptions(DeepClone<ConnectionOptions> connectionOptions, DeepClone<ObjectGetOptions> objectGetOptions, DeepClone<EnumerationOptions> enumerationOptions)

//        {

//            _connectionOptionsDelegate = connectionOptions;

//            _connectionOptions = connectionOptions?.Invoke(null);

//            _objectGetOptionsDelegate = objectGetOptions;

//            _objectGetOptions = objectGetOptions?.Invoke(null);

//            _enumerationOptionsDelegate = enumerationOptions;

//            _enumerationOptions = enumerationOptions?.Invoke(null);

//        }

//        protected virtual void OnDeepClone(WMIItemInfoFactoryOptions wMIItemInfoFactoryOptions) { }

//        protected virtual WMIItemInfoFactoryOptions DeepCloneOverride() => new WMIItemInfoFactoryOptions(_connectionOptionsDelegate, _objectGetOptionsDelegate, _enumerationOptionsDelegate);

//        public object DeepClone()

//        {

//            WMIItemInfoFactoryOptions options = DeepCloneOverride();

//            OnDeepClone(options);

//            return options;

//        }

//    }
//}

///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Management;
//using System.Threading;
//using WinCopies.Collections;
//using WinCopies.Util;

//namespace WinCopies.IO
//{
//    public interface IWMILoader : IBrowsableObjectInfoLoader
//    {

//        /// <summary>
//        /// Gets or sets the WMI item types to load.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IWMILoader"/> is busy.</exception>
//        WMIItemTypes WMIItemTypes { get; set; }

//    }

//    /// <summary>
//    /// A class for easier <see cref="ManagementBaseObject"/> items loading.
//    /// </summary>
//    public class WMILoader<TPath, TItems, TSubItems, TFactory, TItemsFactory> : BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory>, IWMILoader where TPath : WMIItemInfo where TItems : WMIItemInfo where TSubItems : WMIItemInfo where TFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory where TItemsFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
//    {

//        protected override BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> DeepCloneOverride() => new WMILoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>(default, WMIItemTypes, (IFileSystemObjectComparer<IFileSystemObject>)FileSystemObjectComparer.DeepClone(), WorkerReportsProgress, WorkerSupportsCancellation);

//        private readonly WMIItemTypes _wmiItemTypes;

//        /// <summary>
//        /// Gets or sets the WMI item types to load.
//        /// </summary>
//        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMILoader{TPath, TItems, TSubItems, TFactory}"/> is busy.</exception>
//        public WMIItemTypes WMIItemTypes { get => _wmiItemTypes; set => this.SetBackgroundWorkerProperty(nameof(WMIItemTypes), nameof(_wmiItemTypes), value, typeof(WMILoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>), true); }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="wmiItemTypes">The WMI item types to load.</param>
//        public WMILoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, WMIItemTypes wmiItemTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, wmiItemTypes, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> class using a custom comparer.
//        /// </summary>
//        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
//        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
//        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
//        /// <param name="wmiItemTypes">The WMI item types to load.</param>
//        public WMILoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, WMIItemTypes wmiItemTypes, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer, bool workerReportsProgress, bool workerSupportsCancellation) : base(path, (IFileSystemObjectComparer<IFileSystemObject>)fileSystemObjectComparer, workerReportsProgress, workerSupportsCancellation) => _wmiItemTypes = wmiItemTypes;

//        //public override bool CheckFilter(string path) => throw new NotImplementedException();

//        protected override void OnDoWork(DoWorkEventArgs e)
//        {

//            var paths = new ArrayBuilder<PathInfo>();

//            string _path;

//            bool dispose = false;

//            // #pragma warning disable IDE0019 // Pattern Matching
//            var managementClass = Path.Value.ManagementObject as ManagementClass;
//            // #pragma warning restore IDE0019 // Pattern Matching

//            if (managementClass == null)

//            {

//                dispose = true;

//                // #pragma warning disable IDE0067 // Dispose objects before losing scope
//                managementClass = new ManagementClass(new ManagementScope(Path.Value.Path, Path.Factory.Options?.ConnectionOptions), new ManagementPath(Path.Value.Path), Path.Factory.Options?.ObjectGetOptions);
//                // #pragma warning restore IDE0067 // Dispose objects before losing scope

//            }

//            if (Path.Value.WMIItemType == WMIItemType.Namespace)

//            {

//                // managementClass = Path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.Factory?.Options?.ObjectGetOptions);

//                if (WMIItemTypes.HasFlag(WMIItemTypes.Namespace))

//                    try
//                    {

//                        managementClass.Get();

//                        using ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.Factory.Options?.EnumerationOptions)).GetEnumerator();
//                        ManagementBaseObject instance;

//                        while (instances.MoveNext())

//                            try

//                            {

//                                do

//                                {

//                                    instance = instances.Current;

//                                    _path = WMIItemInfo.GetPath(instance, WMIItemType.Namespace);

//                                    if (CheckFilter(_path))

//                                        _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Namespace), WMIItemType.Namespace, instance, managementBaseObject => WMIItemInfo.DefaultManagementObjectDeepClone(managementBaseObject as ManagementClass ?? throw new NotSupportedException("The object is not a ManagementObject."), null)));

//                                }

//                                while (instances.MoveNext());

//                            }

//                            // #pragma warning disable CA1031 // Do not catch general exception types
//                            catch (Exception ex) when (!(ex is ThreadAbortException)) { }

//                    }

//                    // #pragma warning disable CA1031 // Do not catch general exception types
//                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
//                // #pragma warning restore CA1031 // Do not catch general exception types

//                if (WMIItemTypes.HasFlag(WMIItemTypes.Class))

//                    try

//                    {

//                        // MessageBox.Show(wmiItemInfo.Path.Substring(0, wmiItemInfo.Path.Length - ":__NAMESPACE".Length));
//                        // managementClass = new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path.Substring(0, Path.Path.Length - ":__NAMESPACE".Length)), Path.Factory?.Options?.ObjectGetOptions);

//                        //#if DEBUG
//                        //                        if (Path.Path.Contains("CIM"))

//                        //                            MessageBox.Show(instances.Count.ToString());
//                        //#endif

//                        ManagementBaseObject instance;

//                        using ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory?.Options?.EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(Path.Factory?.Options?.EnumerationOptions)).GetEnumerator();
//                        while (instances.MoveNext())

//                            try

//                            {

//                                do

//                                {

//                                    instance = instances.Current;

//                                    _path = WMIItemInfo.GetPath(instance, WMIItemType.Class);

//                                    if (CheckFilter(_path))

//                                        _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Class), WMIItemType.Class, instance, managementBaseObject => WMIItemInfo.DefaultManagementObjectDeepClone(managementBaseObject as ManagementClass ?? throw new NotSupportedException("The object is not a ManagementObject."), null)));

//                                } while (instances.MoveNext());

//                            }

//#pragma warning disable CA1031 // Do not catch general exception types
//                            catch (Exception ex) when (!(ex is ThreadAbortException)) { }

//                    }

//#pragma warning disable CA1031 // Do not catch general exception types
//                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
//#pragma warning restore CA1031 // Do not catch general exception types

//            }

//            else if (Path.Value.WMIItemType == WMIItemType.Class && WMIItemTypes.HasFlag(WMIItemTypes.Instance))

//            {

//                try
//                {

//                    managementClass.Get();

//                    ManagementBaseObject instance;

//                    using ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.Factory?.Options?.EnumerationOptions)).GetEnumerator();
//                    while (instances.MoveNext())

//                        try

//                        {

//                            do

//                            {

//                                instance = instances.Current;

//                                _path = WMIItemInfo.GetPath(instance, WMIItemType.Instance);

//                                if (CheckFilter(_path))

//                                    _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Instance), WMIItemType.Instance, instance, managementBaseObject => WMIItemInfo.DefaultManagementObjectDeepClone(managementBaseObject as ManagementClass ?? throw new NotSupportedException("The object is not a ManagementObject."), null)));

//                            } while (instances.MoveNext());

//                        }

//                        // #pragma warning disable CA1031 // Do not catch general exception types
//                        catch (Exception ex) when (!(ex is ThreadAbortException)) { }
//                    // #pragma warning restore CA1031 // Do not catch general exception types

//                }

//                // #pragma warning disable CA1031 // Do not catch general exception types
//                catch (Exception ex) when (!(ex is ThreadAbortException)) { }
//                // #pragma warning restore CA1031 // Do not catch general exception types

//                if (dispose)

//                    managementClass.Dispose();

//            }



//            IEnumerable<PathInfo> pathInfos;



//            if (FileSystemObjectComparer == null)

//                pathInfos = paths;

//            else

//            {

//                var _paths = paths.ToList();

//                _paths.Sort((System.Collections.Generic.IComparer<PathInfo>)FileSystemObjectComparer);

//                pathInfos = _paths;

//            }



//            PathInfo path_;



//            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())

//                while (_paths.MoveNext())

//                    try

//                    {

//                        do

//                        {

//                            path_ = _paths.Current;

//                            // new_Path.LoadThumbnail();

//                            ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>((TItems)(IWMIItemInfo)Path.Factory.GetBrowsableObjectInfo(path_.Path, path_.WMIItemType, path_.ManagementObject, path_.ManagementObjectDelegate /*managementObject => WMIItemInfo.DefaultManagementObjectDeepClone( (ManagementObject) path_.ManagementObject, null )*/), (TItemsFactory)Path.Factory.DeepClone()));

//                        } while (_paths.MoveNext());

//                    }

//#pragma warning disable CA1031 // Do not catch general exception types
//                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
//#pragma warning restore CA1031 // Do not catch general exception types

//        }

//        protected class PathInfo : IO.PathInfo
//        {

//            /// <summary>
//            /// Gets the localized name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string LocalizedName => Name;

//            /// <summary>
//            /// Gets the name of this <see cref="PathInfo"/>.
//            /// </summary>
//            public override string Name { get; }

//            public DeepClone<ManagementBaseObject> ManagementObjectDelegate { get; }

//            public ManagementBaseObject ManagementObject { get; }

//            public WMIItemType WMIItemType { get; }

//            public PathInfo(string path, string normalizedPath, string name, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate) : base(path, normalizedPath)
//            {

//                Name = name;

//                ManagementObject = managementObject;

//                ManagementObjectDelegate = managementObjectDelegate;

//                WMIItemType = wmiItemType;

//            }

//        }

//    }

//}
