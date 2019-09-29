using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;
using static WinCopies.Util.Util;
// using WinCopies.Util.Data;

namespace WinCopies.Collections
{

    // todo: make non-generic

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public abstract class TreeNode<T> : ITreeNode<T>

    {

        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeNode{T}"/> is read-only. This value is always <see langword="false"/> for this class.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(IValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(IValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        /// <summary>
        /// Gets the parent of the current node.
        /// </summary>
        public virtual ITreeNode Parent { get; protected internal set; }

        private T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => SetValue(value); }

        protected virtual void SetValue(T newValue) => _value = newValue;

        object IValueObject.Value { get => Value; set => Value = (T)value; }

        protected TreeNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value of the new <see cref="TreeNode{T}"/>.</param>
        protected TreeNode(T value) => Value = value;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the current object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is IValueObject _obj ? Equals(_obj) : obj is T __obj ? Value?.Equals(__obj) == true : obj is null ? !(Value is object) : false;

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Value is object ? Value.GetHashCode() : base.GetHashCode();

        public override string ToString() => Value?.ToString() ?? base.ToString();

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool disposing)
        {

            if (disposedValue)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            this.Parent = null;

            disposedValue = true;

        }

        ~TreeNode()
        {

            Dispose(false);

        }

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);

        }
        #endregion

    }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TItems">The items type.</typeparam>
    [Serializable]
    [DebuggerDisplay("Value = {Value}, Count = {Count}")]
    public class TreeNode<TValue, TItems> : TreeNode<TValue>, ITreeNode<TValue, TItems>, ICollection<TreeNode<TItems>>, System.Collections.Generic.IList<TreeNode<TItems>>, ICollection, System.Collections.IList, IReadOnlyCollection<TreeNode<TItems>>, System.Collections.Generic.IReadOnlyList<TreeNode<TItems>>, IReadOnlyCollection<TItems>, System.Collections.Generic.IReadOnlyList<TItems>
    {

        /// <summary>
        /// Returns the default comparer for <see cref="TreeNode{TValue, TItems}"/> objects.
        /// </summary>
        /// <returns>The default comparer for <see cref="TreeNode{TValue, TItems}"/> objects.</returns>
        protected virtual IEqualityComparer<TreeNode<TItems>> GetDefaultTreeNodeItemsComparer() => new ValueObjectEqualityComparer<TItems>();

        /// <summary>
        /// Gets the inner <see cref="System.Collections.Generic.IList{T}"/> of this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        protected System.Collections.Generic.IList<TreeNode<TItems>> Items { get; }

        // protected virtual ITreeCollection<TItems> GetDefaultItemCollection() => new TreeCollection<TItems>(this);

        public TreeNode() : this(value: default) { }

        public TreeNode(System.Collections.Generic.IList<TreeNode<TItems>> items) : this(default, items) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{TValue, TItems}"/> class using a custom value.
        /// </summary>
        /// <param name="value">The value of the new <see cref="TreeNode{TValue, TItems}"/>.</param>
        public TreeNode(TValue value) : this(value, new List<TreeNode<TItems>>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{TValue, TItems}"/> class using a custom value and inner <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="value">The value of the new <see cref="TreeNode{TValue, TItems}"/>.</param>
        /// <param name="items">A custom inner <see cref="IList{T}"/>.</param>
        public TreeNode(TValue value, System.Collections.Generic.IList<TreeNode<TItems>> items) : base(value)
        {
            ThrowIfNull(items, nameof(items));

            Items = items;
        }

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected override void Dispose(bool disposing)
        {

            Clear();

            base.Dispose(disposing);

        }

        [NonSerialized]
        private object _syncRoot;

        object ICollection.SyncRoot => _syncRoot ?? (_syncRoot = Items is ICollection collection ? collection.SyncRoot : System.Threading.Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null));

        bool ICollection.IsSynchronized => false;

        /// <summary>
        /// Gets or sets the item at the specified index in this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at the given index.</returns>
        /// <exception cref="IndexOutOfRangeException">The given index is lesser than 0 or greater than <see cref="Count"/>.</exception>
        /// <seealso cref="SetItem(int, TreeNode{TItems})"/>
        public TreeNode<TItems> this[int index] { get => Items[index]; set => SetItem(index, value); }

        TItems System.Collections.Generic.IReadOnlyList<TItems>.this[int index] => this[index].Value;

        object System.Collections.IList.this[int index] { get => this[index]; set => this[index] = GetOrThrowIfNotType<TreeNode<TItems>>(value, nameof(value)); }

        /// <summary>
        /// Gets the number of items that this <see cref="TreeNode{TValue, TItems}"/> directly contains.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeNode{TValue, TItems}"/> is fixed-size.
        /// </summary>
        public bool IsFixedSize => Items is IList _items ? _items.IsFixedSize : false /*Items.IsReadOnly*/;

        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> for this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <returns><see cref="IEnumerator{T}"/> for this <see cref="TreeNode{TValue, TItems}"/>.</returns>
        public IEnumerator<TreeNode<TItems>> GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TItems> IEnumerable<TItems>.GetEnumerator() => new ValueObjectEnumerator<TItems>(GetEnumerator());

        IEnumerator<ITreeNode<TItems>> IEnumerable<ITreeNode<TItems>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds a new item to the end of this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <seealso cref="InsertItem(int, TreeNode{TItems})"/>
        public void Add(TreeNode<TItems> item) => InsertItem(Count, item);

        int System.Collections.IList.Add(object value)
        {
            Add(GetOrThrowIfNotType<TreeNode<TItems>>(value, nameof(value)));

            return Count - 1;
        }

        void ICollection<ITreeNode<TItems>>.Add(ITreeNode<TItems> item) => Add(GetOrThrowIfNotType<TreeNode<TItems>>(item, nameof(item)));

        /// <summary>
        /// Checks if this <see cref="TreeNode{TValue, TItems}"/> directly contains a given <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="item">The <see cref="TreeNode{T}"/> to look for.</param>
        /// <returns><see langword="true"/> if this <see cref="TreeNode{TValue, TItems}"/> directly contains the given <see cref="TreeNode{T}"/>, otherwise <see langword="false"/>.</returns>
        public bool Contains(TreeNode<TItems> item)

        {

            if (item is null)

                return false;

            IEqualityComparer<TreeNode<TItems>> comp = GetDefaultTreeNodeItemsComparer();

            foreach (TreeNode<TItems> _item in this)

                if (comp.Equals(_item, item))

                    return true;

            return false;

        }

        /// <summary>
        /// Checks if this <see cref="TreeNode{TValue, TItems}"/> directly contains a given item.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns><see langword="true"/> if this <see cref="TreeNode{TValue, TItems}"/> directly contains the given item, otherwise <see langword="false"/>.</returns>
        public bool Contains(TItems item)

        {

            if (!(item is object))

                return false;

            EqualityComparer<TItems> comp = EqualityComparer<TItems>.Default;

            foreach (TreeNode<TItems> _item in this)

                if (comp.Equals(_item.Value, item))

                    return true;

            return false;

        }

        bool System.Collections.IList.Contains(object value) => value is TItems item ? Contains(item) : value is TreeNode<TItems> node ? Contains(node) : false;

        bool ICollection<ITreeNode<TItems>>.Contains(ITreeNode<TItems> item) => item is TreeNode<TItems> _item ? Contains(_item) : false;

        /// <summary>
        /// Returns the idnex of a given item in this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <param name="item">The item for which to find the index.</param>
        /// <returns>The index of <paramref name="item"/> if this <see cref="TreeNode{TValue, TItems}"/> contains <paramref name="item"/>, otherwise -1.</returns>
        public int IndexOf(TreeNode<TItems> item)

        {

            if (item is null)

                return -1;

            IEqualityComparer<TreeNode<TItems>> comp = GetDefaultTreeNodeItemsComparer();

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i], item))

                    return i;

            return -1;

        }

        /// <summary>
        /// Returns the idnex of a given item in this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <param name="item">The item for which to find out the index.</param>
        /// <returns>The index of <paramref name="item"/> if this <see cref="TreeNode{TValue, TItems}"/> contains <paramref name="item"/>, otherwise -1.</returns>
        public int IndexOf(TItems item)

        {

            if (!(item is object))

                return -1;

            EqualityComparer<TItems> comp = EqualityComparer<TItems>.Default;

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i].Value, item))

                    return i;

            return -1;

        }

        int System.Collections.IList.IndexOf(object value) => value is TItems item ? IndexOf(item) : value is TreeNode<TItems> node ? IndexOf(node) : -1;

        /// <summary>
        /// Removes the item at a given index.
        /// </summary>
        /// <param name="index">The index from which to remove the item.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is lesser than 0 or greater than <see cref="Count"/>.</exception>
        /// <exception cref="NotSupportedException">This <see cref="TreeNode{TValue, TItems}"/> is fixed-size.</exception>
        /// <seealso cref="RemoveItem(int)"/>
        public void RemoveAt(int index) => RemoveItem(index);

        /// <summary>
        /// Removes a given item from the node. The current node must directly contains the given item. This function removes <paramref name="item"/> and returns <see langword="true"/> if <paramref name="item"/> is found, otherwise <see langword="false"/> is returned.
        /// </summary>
        /// <param name="item">The item to remove from the current node.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is removed, otherwise <see langword="false"/>.</returns>
        /// <seealso cref="RemoveItem(int)"/>
        public bool Remove(TreeNode<TItems> item)

        {

            int index = IndexOf(item);

            if (index > -1)

            {

                RemoveItem(index);

                return true;

            }

            return false;

        }

        /// <summary>
        /// Removes a given item from the node. The current node must directly contains the given item. This function removes <paramref name="item"/> and returns <see langword="true"/> if <paramref name="item"/> is found, otherwise <see langword="false"/> is returned.
        /// </summary>
        /// <param name="item">The item to remove from the current node.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is removed, otherwise <see langword="false"/>.</returns>
        /// <seealso cref="RemoveItem(int)"/>
        public bool Remove(TItems item)

        {

            if (!(item is object))

                return false;

            EqualityComparer<TItems> comp = EqualityComparer<TItems>.Default;

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i].Value, item))

                {

                    RemoveItem(i);

                    return true;

                }

            return false;

        }

        bool ICollection<ITreeNode<TItems>>.Remove(ITreeNode<TItems> item) => Remove(item as TreeNode<TItems> ?? throw new ArgumentException($"The given item is not a {typeof(TreeNode<TItems>).FullName}."));

        void System.Collections.IList.Remove(object value)
        {
            if (value is TItems item)

                _ = Remove(item);

            else if (value is TreeNode<TItems> node)

                _ = Remove(node);
        }

        /// <summary>
        /// Inserts a given item at a specified index in this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The item to insert in this <see cref="TreeNode{TValue, TItems}"/>.</param>
        /// <seealso cref="InsertItem(int, TreeNode{TItems})"/>
        public void Insert(int index, TreeNode<TItems> item) => InsertItem(index, item);

        void System.Collections.IList.Insert(int index, object value) => InsertItem(index, value as TreeNode<TItems> ?? throw new ArgumentException($"The given item is not a {typeof(TreeNode<TItems>).FullName}."));

        /// <summary>
        /// Performs a shallow copy of the items that the current <see cref="TreeNode{TValue, TItems}"/> directly contains starting at a given index of a given array of <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="array">The array in which to store the shallow copies of the items that <see cref="TreeNode{TValue, TItems}"/> directly contains.</param>
        /// <param name="arrayIndex">The index from which to store the items in <paramref name="array"/>.</param>
        public void CopyTo(TreeNode<TItems>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        void ICollection<ITreeNode<TItems>>.CopyTo(ITreeNode<TItems>[] array, int arrayIndex)
        {

            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

            if (array is TreeNode<TItems>[] _array)

            {

                CopyTo(_array, arrayIndex);

                return;

            }

            //if (array is TItems[] itemsArray)

            //{

            //    foreach (var item in this)

            //        itemsArray[arrayIndex++] = item.Value;

            //    return;

            //}

            // todo: make better checks

            try
            {

                foreach (TreeNode<TItems> item in this)

                    array[arrayIndex++] = item;

            }

            catch (ArrayTypeMismatchException)

            {

                throw new ArgumentException("Invalid array type.");

            }

        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {

            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

            if (array is TreeNode<TItems>[] _array)

            {

                CopyTo(_array, arrayIndex);

                return;

            }

            //if (array is TItems[] itemsArray)

            //{

            //    foreach (var item in this)

            //        itemsArray[arrayIndex++] = item.Value;

            //    return;

            //}

            // todo: make better checks

            try
            {

                foreach (TreeNode<TItems> item in this)

                    array.SetValue(item, arrayIndex++);

            }

            catch (ArrayTypeMismatchException)

            {

                try

                {

                    foreach (TreeNode<TItems> item in this)

                        array.SetValue(item.Value, arrayIndex++);

                }

                catch (ArrayTypeMismatchException)

                {

                    throw new ArgumentException("Invalid array type.");

                }

            }

        }

        private void ThrowOnInvalidItem(TreeNode<TItems> item)

        {

            if (item.Parent is object)

                throw new InvalidOperationException("The given item already has a parent node.");

            if (item.IsReadOnly)

                throw new ArgumentException("The given item is read-only.");

        }

        /// <summary>
        /// Inserts a given item at a specified index in this <see cref="TreeNode{TValue, TItems}"/>. You can override this method in order to change the behavior of the <see cref="Add(TreeNode{TItems})"/> and <see cref="Insert(int, TreeNode{TItems})"/> methods.
        /// </summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The item to insert in this <see cref="TreeNode{TValue, TItems}"/>.</param>
        protected virtual void InsertItem(int index, TreeNode<TItems> item)
        {

            ThrowOnInvalidItem(item);

            item.Parent = this;

            if (index == Count)

                Items.Add(item);

            else

                Items.Insert(index, item);

        }

        /// <summary>
        /// Removes all items of this <see cref="TreeNode{TValue, TItems}"/>.
        /// </summary>
        /// <seealso cref="ClearItems"/>
        public void Clear() => ClearItems();

        /// <summary>
        /// Removes all items of this <see cref="TreeNode{TValue, TItems}"/>. You can override this method in order to change the behavior of the <see cref="Clear"/> method.
        /// </summary>
        protected virtual void ClearItems()
        {
            foreach (TreeNode<TItems> item in this)

                item.Parent = null;

            Items.Clear();
        }

        /// <summary>
        /// Removes the item at a given index. You can override this method in order to change the behavior of the Remove methods.
        /// </summary>
        /// <param name="index">The index from which to remove the item.</param>
        protected virtual void RemoveItem(int index)
        {
            this[index].Parent = null;

            Items.RemoveAt(index);
        }

        /// <summary>
        /// Sets a given item at a specified index of this <see cref="TreeNode{TValue, TItems}"/>. This method sets <paramref name="item"/> directly in the current <see cref="TreeNode{TValue, TItems}"/>. You can override this method in order to change the behavior of the <see cref="this[int]"/> method.
        /// </summary>
        /// <param name="index">The index at which to set <paramref name="item"/>.</param>
        /// <param name="item">The item to update with.</param>
        protected virtual void SetItem(int index, TreeNode<TItems> item)
        {
            ThrowOnInvalidItem(item);

            this[index].Parent = null;

            item.Parent = this;

            Items[index] = item;
        }

    }
}
