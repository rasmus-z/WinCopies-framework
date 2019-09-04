using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{

    // todo: paths must have a protected virtual method to create their items collection. the path's constructor must call this method. using this implementation, a Unregister method does not make sense for the IBrowsableObjectInfoCollection interface.

    //internal class BrowsableObjectInfoCollectionInternal<TOwner, TItems> : Collection<IBrowsableObjectInfoModifier<TOwner, TItems>> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
    //{

    //    internal new IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => base.Items;

    //}

    public interface IBrowsableObjectInfoCollection : Collections.IList /*WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<T>>, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>> where T : class, IBrowsableObjectInfo*/
    {

    }

    public interface IBrowsableObjectInfoCollection< TOwner, TItems> : IBrowsableObjectInfoCollection, WinCopies.Collections.IList<IBrowsableObjectInfoModifier<TItems>>/*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/ where TItems : class, IBrowsableObjectInfo

    {

        TOwner Owner { get; }

        void RegisterOwner(IPathModifier modifier);

    }

    public interface IReadOnlyBrowsableObjectInfoCollection<TItems> : WinCopies.Collections.IReadOnlyList<TItems>/*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/ where TItems : class, IBrowsableObjectInfo
    {

        IBrowsableObjectInfo Owner { get; }

    }

    public interface IPathModifier
    {

        bool AreItemsLoaded { set; }

        IBrowsableObjectInfoAccessor Accessor { get; }

        // IBrowsableObjectInfo Parent { set; }

        // IBrowsableObjectInfoCollection<IBrowsableObjectInfo> Items { get; }

    }

    public class BrowsableObjectInfoCollection<T> : Collection<IBrowsableObjectInfoModifier<T>>, IBrowsableObjectInfoCollection<T> where T : class, IBrowsableObjectInfo
    {

        public void RegisterOwner(IPathModifier modifier)

        {

            // if (object.ReferenceEquals(modifier.Owner, accessor.Owner))

                if (object.ReferenceEquals(this, modifier.Accessor .ItemCollection))

                {

                    _modifier = modifier;

                    // _innerList = new BrowsableObjectInfoCollectionInternal<TOwner, TItems>();

                }

                else

                    throw new ArgumentException("Invalid owner.");

            // else

                // throw new ArgumentException("Invalid owner.");

        }

        // todo: check if is registered

        public IBrowsableObjectInfo Owner => _modifier.Accessor.Owner;

        private IPathModifier _modifier;

        public virtual bool IsReadOnly => false;

        protected override void SetItem(int index, IBrowsableObjectInfoModifier<T> item)
        {

            this[index].Reset(_modifier.Accessor);

            if (item.Item.HasParent)

                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

            base.SetItem(index, item);

            this[index].SetParent(_modifier.Accessor, index);

        }

        protected override void InsertItem(int index, IBrowsableObjectInfoModifier<T> item)
        {

            if (item.Item.HasParent)

                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

            base.InsertItem(index, item);

            item.SetParent(_modifier.Accessor, index);

        }

        protected override void RemoveItem(int index)

        {

            this[index].Reset(_modifier.Accessor);

            base.RemoveItem(index);

        }

        protected override void ClearItems()

        {

            for (int i = 0; i < Count; i++)

                this[i].Reset(_modifier.Accessor);

            base.ClearItems();

            _modifier.AreItemsLoaded = false;

        }

    }

    public class ReadOnlyBrowsableObjectInfoCollection<TItems> : IReadOnlyBrowsableObjectInfoCollection<TItems> where TItems : class, IBrowsableObjectInfo
    {

        protected IBrowsableObjectInfoCollection<TItems> Items { get; }

        public IBrowsableObjectInfo Owner => ((IBrowsableObjectInfoCollection<TItems>)this.Items).Owner;

        public int Count => Items.Count;

        bool WinCopies.Collections.IReadOnlyList<TItems>.IsReadOnly => true;

        bool System.Collections.Generic.ICollection<TItems>.IsReadOnly => true;

        bool System.Collections.IList.IsReadOnly => true;

        bool System.Collections.IList.IsFixedSize => Items.IsFixedSize;

        object ICollection.SyncRoot => Items.SyncRoot;

        bool ICollection.IsSynchronized => Items.IsSynchronized;

        // TItems IReadOnlyList<TItems>.this[int index] => throw new NotImplementedException();

        object System.Collections.IList.this[int index] { get => this[index]; set => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection); }

        public TItems this[int index] { get => Items[index].Item; set => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection); }

        public ReadOnlyBrowsableObjectInfoCollection(IBrowsableObjectInfoCollection<TItems> list) => Items = list;

        void Collections.IReadOnlyList<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        //void WinCopies.Collections.IReadOnlyList<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        //void System.Collections.Generic.ICollection<TItems>.Clear() => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        public IEnumerator<TItems> GetEnumerator() => new Enumerator(Items);

        void System.Collections.IList.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        void System.Collections.Generic.IList<TItems>.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        void WinCopies.Collections.IReadOnlyList<TItems>.RemoveAt(int index) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        public int IndexOf(TItems item)
        {

            for (int i = 0; i < Count; i++)

                if (Items[i].Item.Equals(item))

                    return i;

            return -1;

        }

        void System.Collections.Generic.IList<TItems>.Insert(int index, TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        void System.Collections.Generic.ICollection<TItems>.Add(TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        public bool Contains(TItems item)
        {

            foreach (IBrowsableObjectInfoModifier<TItems> _item in Items)

                if (_item.Item.Equals(item))

                    return true;

            return false;

        }

        public void CopyTo(TItems[] array, int arrayIndex)

        {

            if (arrayIndex < 0 || arrayIndex > array.Length)

                throw new IndexOutOfRangeException("arrayIndex is not in the required value range.");

            if (Count > array.Length - arrayIndex)

                throw new InvalidOperationException("There is not enough space starting with the given index in the given array.");

            foreach (var item in Items)

                array[arrayIndex++] = item.Item;

        }

        bool System.Collections.Generic.ICollection<TItems>.Remove(TItems item) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        int System.Collections.IList.Add(object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        bool System.Collections.IList.Contains(object value) => value is TItems item ? Contains(item) : false;

        int System.Collections.IList.IndexOf(object value) => value is TItems item ? IndexOf(item) : -1;

        void System.Collections.IList.Insert(int index, object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        void System.Collections.IList.Remove(object value) => throw new InvalidOperationException(WinCopies.Util.Generic.ReadOnlyCollection);

        void ICollection.CopyTo(Array array, int index) => CopyTo((TItems[])array, index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [Serializable]
        public struct Enumerator : IEnumerator<TItems>, IEnumerator
        {

            private readonly IEnumerator<IBrowsableObjectInfoModifier<TItems>> _enumerator;

            public TItems Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(IEnumerable<IBrowsableObjectInfoModifier<TItems>> innerEnumerable)

            {

                _enumerator = innerEnumerable.GetEnumerator();

                Current = default;

            }

            public bool MoveNext()
            {

                if (_enumerator.MoveNext())

                {

                    Current = _enumerator.Current.Item;

                    return true;

                }

                else

                    return false;

            }

            public void Reset() => Dispose();

            public void Dispose()
            {

                _enumerator.Dispose();

                Current = default;

            }

        }

    }

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

}
