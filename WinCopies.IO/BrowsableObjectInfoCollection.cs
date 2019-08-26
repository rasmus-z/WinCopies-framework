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

    public interface IBrowsableObjectInfoCollection : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T> where T : IBrowsableObjectInfo
    {

        IBrowsableObjectInfo Owner { get; }

    }

    public interface IBrowsableObjectInfoAccessor
    {

        IBrowsableObjectInfo Owner { get; }

        IBrowsableObjectInfoCollection ItemsCollection { get; }

    }

    internal class BrowsableObjectInfoCollectionInternal : Collection<IBrowsableObjectInfoModifier>
    {

        internal new IList<T> Items => base.Items;

    }

    public class BrowsableObjectInfoCollection<T> : IBrowsableObjectInfoCollection where T : IBrowsableObjectInfo
    {

        // todo: Owner must be accessible from the read-only wrapper of this collection

        public IBrowsableObjectInfo Owner { get; }

        private Func<IBrowsableObjectInfo, IBrowsableObjectInfoModifier> _getModifier;

        private BrowsableObjectInfoCollectionInternal _innerList;

        protected IList<IBrowsableObjectInfoModifier> Items => _innerList.Items;

        public BrowsableObjectInfoCollection(IBrowsableObjectInfoAccessor accessor, Func<IBrowsableObjectInfo, IBrowsableObjectInfoModifier> getModifier)
        {

            if (object.ReferenceEquals(this, accessor.ItemsCollection))

            {

                Owner = accessor.Owner;

                innerList = new Collection<IBrowsableObjectInfoModifier>();

                this.getModifier = getModifier;

            }

            else

                throw new ArgumentException("Invalid owner.");

        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="BrowsableObjectInfoCollection"/>.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="BrowsableObjectInfoCollection"/> is read-only.</exception>
        T this[int index] { get => GetItem(index); set => SetItem(index, value); }

        protected virtual T GetItem(int index) => _innerList[index].Item;

        protected virtual bool SetItem(int index, T item)
        {

            _innerList[index] = _getModifier(item);

            return true;

        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="BrowsableObjectInfoCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BrowsableObjectInfoCollection"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        int IndexOf(T item) => IndexOfItem(item);

        protected virtual int IndexOfItem(T item)
        {
            for (int i = 0; i < Count; i++)

                if (_innerList[i].Item?.Equals(item) == true)

                    return i;

            return -1;
        }

        //
        // Résumé :
        //     Inserts an item to the BrowsableObjectInfoCollection at the specified index.
        //
        // Paramètres :
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert into the BrowsableObjectInfoCollection.
        //
        // Exceptions :
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the BrowsableObjectInfoCollection.
        //
        //   T:System.NotSupportedException:
        //     The BrowsableObjectInfoCollection is read-only.
        void Insert(int index, T item) => InsertItem(index, item);

        protected virtual bool InsertItem(int index, T item)
        {

            items.Insert(index, _getModifier(item));

            return true;

        }

        //
        // Résumé :
        //     Removes the BrowsableObjectInfoCollection item at the specified index.
        //
        // Paramètres :
        //   index:
        //     The zero-based index of the item to remove.
        //
        // Exceptions :
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the BrowsableObjectInfoCollection.
        //
        //   T:System.NotSupportedException:
        //     The BrowsableObjectInfoCollection is read-only.
        void RemoveAt(int index);

        [Serializable]
        public struct Enumerator<TIn, TOut> : IEnumerator<TOut>, IEnumerator
        {

            private IEnumerator<TIn> _enumerator;

            public TOut Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(IEnumerable<TIn> innerEnumerable)

            {

                _enumerator = innerEnumerable.GetEnumerator();

                Current = default;

            }

            public bool MoveNext()
            {

                if (_enumerator.MoveNext())

                {

                    Current = MoveNextDelegate(_enumerator.Current);

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
