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

    // todo: the paths must have a protected virtual method to create their items collection. the path's constructor must call this method. using this implementation, a Unregister method does not make sense for the IBrowsableObjectInfoCollection interface.

    public interface IBrowsableObjectInfoCollection<TItems> : ICollection<TItems>, IEnumerable<TItems>, IList<TItems>, IReadOnlyCollection<TItems>, IReadOnlyList<TItems>, ICollection, IEnumerable, IList where TItems : IBrowsableObjectInfo
    {

    }

    public interface IBrowsableObjectInfoAccessor<TOwner, TItems> where TOwner : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo
    {

        TOwner Owner { get; }

        IBrowsableObjectInfoCollection<TItems> ItemCollection { get; }

    }

    internal class BrowsableObjectInfoCollectionInternal<TOwner, TItems> : Collection<IBrowsableObjectInfoModifier<TOwner, TItems>> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
    {

        internal new IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => base.Items;

    }

    public class BrowsableObjectInfoCollection<TOwner, TItems> : IBrowsableObjectInfoCollection<TItems> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
    {

        // todo: Owner must be accessible from the read-only wrapper of this collection

        public TOwner Owner => _accessor.Owner;

        private IBrowsableObjectInfoAccessor<TOwner, TItems> _accessor;

        private BrowsableObjectInfoCollectionInternal<TOwner, TItems> _innerList;

        protected IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => _innerList.Items;

        public BrowsableObjectInfoCollection(IBrowsableObjectInfoAccessor<TOwner, TItems> accessor)
        {

            if (object.ReferenceEquals(this, accessor.ItemCollection))

            {

                _accessor = accessor;

                _innerList = new BrowsableObjectInfoCollectionInternal<TOwner, TItems>();

            }

            else

                throw new ArgumentException("Invalid owner.");

        }

        // protected virtual T GetItem(int index) => _innerList[index].Item;

        public virtual void SetItem(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item)
        {

            _innerList[index].Reset(_accessor, index);

            _innerList[index] = item;

            _innerList[index].SetParent(_accessor, index);

        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(TItems item) => IndexOfItem(item);

        int IList.IndexOf(object value) => value is TItems item ? IndexOfItem(item) : -1;

        protected virtual int IndexOfItem(TItems item)
        {
            for (int i = 0; i < Count; i++)

                if (_innerList[i].Item?.Equals(item) == true)

                    return i;

            return -1;
        }

        public bool IsReadOnly => true;

        public int Count => _innerList.Count;

        public virtual bool IsFixedSize => false;

        public object SyncRoot => _innerList.sync;

        public bool IsSynchronized => sync

        object IList.this[int index] { get => _innerList[index].Item; set => throw new InvalidOperationException("This collection is read-only."); }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
        public TItems this[int index] { get => _innerList[index].Item; set => throw new InvalidOperationException("This collection is read-only."); }

        /// <summary>
        /// Inserts an item to the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
        /// <exception cref="NotSupportedException">The <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
        public void Insert(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item) => InsertItem(index, item);

        protected virtual void InsertItem(int index, IBrowsableObjectInfoModifier<TOwner, TItems> item)
        {

            _innerList.Insert(index, item);

            item.SetParent(_accessor, index);

        }

        /// <summary>
        /// Removes the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/>.</exception>
        /// <exception cref="NotSupportedException">The <see cref="BrowsableObjectInfoCollection{TOwner, TItems}"/> is read-only.</exception>
        public void RemoveAt(int index) => RemoveItem(index);

        protected virtual void RemoveItem(int index)

        {

            _innerList[index].Reset(_accessor, index);

            _innerList.RemoveAt(index);

        }

        public void Clear() => ClearItems();

        protected virtual void ClearItems()

        {

            for (int i = 0; i < _innerList.Count; i++)

                _innerList[i].Reset(_accessor, i);

            _innerList.Clear();

        }

        public void Insert(int index, TItems item) => throw new InvalidOperationException("This collection is read-only.");

        public void Add(TItems item) => throw new InvalidOperationException("This collection is read-only.");

        public void Add(IBrowsableObjectInfoModifier<TOwner, TItems> item) => InsertItem(Count, item);

        public bool Contains(TItems item)
        {
            for (int i = 0; i < Count; i++)

                if (_innerList[i].Item?.Equals(item) == true)

                    return true;

            return false;
        }

        public void CopyTo(TItems[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex > Count)

                throw new ArgumentException("array as not the required length.");

            for (int i = 0; i < Count; i++)

                array[arrayIndex + i] = _innerList[i].Item;
        }

        public bool Remove(TItems item) => throw new InvalidOperationException("This collection is read-only.");

        public IEnumerator<TItems> GetEnumerator() => new Enumerator<TOwner, TItems>(_innerList);

        public int Add(object value) => throw new InvalidOperationException("This collection is read-only.");

        public bool Contains(object value) => value is TItems item ? Contains(item) : false;

        public void Insert(int index, object value) => throw new InvalidOperationException("This collection is read-only.");

        public void Remove(object value) => throw new InvalidOperationException("This collection is read-only.");

        public void CopyTo(Array array, int index) => CopyTo((TItems[])array, index);

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

        [Serializable]
        public struct Enumerator : IEnumerator<TItems>, IEnumerator
        {

            private IEnumerator<IBrowsableObjectInfoModifier<TOwner, TItems>> _enumerator;

            public TItems Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(IEnumerable<IBrowsableObjectInfoModifier<TOwner, TItems>> innerEnumerable)

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

}
