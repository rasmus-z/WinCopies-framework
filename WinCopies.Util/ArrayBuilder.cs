/* Copyright © Pierre Sprimont, 2019
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections.DotNetFix;
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{

    public interface ILinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback

    {

        LinkedListNode<T> Last { get; }

        LinkedListNode<T> First { get; }

        new int Count { get; }

        LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value);

        void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);

        LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);

        void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);

        LinkedListNode<T> AddFirst(T value);

        void AddFirst(LinkedListNode<T> node);

        LinkedListNode<T> AddLast(T value);

        void AddLast(LinkedListNode<T> node);

        LinkedListNode<T> Find(T value);

        LinkedListNode<T> FindLast(T value);

        System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();

        void Remove(LinkedListNode<T> node);

        void RemoveFirst();

        void RemoveLast();

    }

    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T> : System.Collections.Generic.LinkedList<T>, ILinkedList<T>

    {



    }

}

namespace WinCopies.Collections
{

    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T> : ILinkedList<T>
    {

        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedList(System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => false;

        object ICollection. SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection. IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        void ICollection<T>.Add(T item) => ((ICollection<T>)InnerList).Add(item);

        public virtual LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        public virtual void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        public virtual LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        public virtual void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        public virtual LinkedListNode<T> AddFirst(T value) => InnerList.AddFirst(value);

        public virtual void AddFirst(LinkedListNode<T> node) => InnerList.AddFirst(node);

        public virtual LinkedListNode<T> AddLast(T value) => InnerList.AddLast(value);

        public virtual void AddLast(LinkedListNode<T> node) => InnerList.AddLast(node);

        public virtual void Clear() => InnerList.Clear();

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public virtual bool Remove(T value) => InnerList.Remove(value);

        public virtual void Remove(LinkedListNode<T> node) => InnerList.Remove(node);

        public virtual void RemoveFirst() => InnerList.RemoveFirst();

        public virtual void RemoveLast() => InnerList.RemoveLast();

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index) ;

    }

    public class ReadOnlyLinkedList<T> : ILinkedList<T>

    {

        protected ILinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        void ICollection<T>.Add(T item) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        void ICollection<T>.Clear() => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(LinkedListNode<T> node) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void AddLast(LinkedListNode<T> node) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void Remove(LinkedListNode<T> node) => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void RemoveFirst() => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

        public void RemoveLast() => throw new InvalidOperationException(Resources.ExceptionMessages.ReadOnlyCollection);

    }

    public class EnumeratorCollection : Collection<IEnumerator>
    {

        private WrapperStruct<int> _wrapperStruct;

        protected ref WrapperStruct<int> WrapperStruct => ref _wrapperStruct;

        public EnumeratorCollection(List<IEnumerator> list, ref WrapperStruct<int> wrapperStruct) : base(list) => _wrapperStruct = wrapperStruct;

        protected virtual void Reset() => _wrapperStruct.Value = 0;

        protected virtual void TryReset()

        {

            if (Count == 0)

                Reset();

        }

        protected override void ClearItems()
        {
            base.ClearItems();

            Reset();
        }

        protected override void RemoveItem(int index)
        {
            RemoveItem(index);

            TryReset();
        }

    }

    /// <summary>
    /// Builds arrays and lists by sizing them only when required. This class can be used to initialize your array or list before adding or removing values to it.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ArrayBuilder<T> : ILinkedList<T>
    {

        public EnumeratorCollection Enumerators { get; }

        protected WrapperStruct<int> _wrapperStruct = new WrapperStruct<int>(0);

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.LinkedList{T}"/> that is used to build the arrays and collections.
        /// </summary>
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        public ArrayBuilder()
        {
            Enumerators = new EnumeratorCollection(new System.Collections.Generic.List<IEnumerator>(), ref _wrapperStruct);

            InnerList = new System.Collections.Generic.LinkedList<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBuilder{T}"/> class with a given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable"></param>
        public ArrayBuilder(IEnumerable<T> enumerable) => InnerList = new System.Collections.Generic.LinkedList<T>(enumerable);

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBuilder{T}"/> class using a custom <see cref="System.Collections.Generic.LinkedList{T}"/> to build the arrays and collections.
        /// </summary>
        /// <param name="innerList"></param>
        protected ArrayBuilder(System.Collections.Generic.LinkedList<T> innerList) => InnerList = innerList;

        /// <summary>
        /// Gets the last node of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>The last <see cref="LinkedListNode{T}"/> of this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual LinkedListNode<T> Last => InnerList.Last;

        /// <summary>
        /// Gets the first node of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>The first <see cref="LinkedListNode{T}"/> of this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual LinkedListNode<T> First => InnerList.First;

        /// <summary>
        /// Gets the number of nodes actually contained in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>The number of nodes actually contained in this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual int Count => InnerList.Count;

        bool ICollection<T>. IsReadOnly => false ; 

        object ICollection. SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection. IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        protected virtual void OnUpdate() => _wrapperStruct.Value++;

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> after which to insert a new <see cref="LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="ArrayBuilder{T}"/>.</exception>
        public virtual LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = InnerList.AddAfter(node, value);

            OnUpdate();

            return result;
        }

        /// <summary>
        /// Adds the specified new node after the specified existing node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> after which to insert newNode.</param>
        /// <param name="newNode">The new <see cref="LinkedListNode{T}"/> to add to this <see cref="ArrayBuilder{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null. -or- newNode is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="ArrayBuilder{T}"/>. -or- newNode belongs to another <see cref="ILinkedList{T}"/>.</exception>
        public virtual void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            InnerList.AddAfter(node, newNode);

            OnUpdate();
        }

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> before which to insert a new <see cref="LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="ArrayBuilder{T}"/>.</exception>
        public virtual LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            var result = InnerList.AddBefore(node, value);

            OnUpdate();

            return result;
        }

        /// <summary>
        /// Adds the specified new node before the specified existing node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> before which to insert newNode.</param>
        /// <param name="newNode">The new <see cref="LinkedListNode{T}"/> to add to this <see cref="ArrayBuilder{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null. -or- newNode is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="ArrayBuilder{T}"/>. -or- newNode belongs to another <see cref="ILinkedList{T}"/>.</exception>
        public virtual void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            InnerList.AddBefore(node, newNode);

            OnUpdate();
        }

        /// <summary>
        /// Adds a new node containing the specified value at the start of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="value">The value to add at the start of this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        public virtual LinkedListNode<T> AddFirst(T value)
        {
            var result = InnerList.AddFirst(value);

            OnUpdate();

            return result;
        }

        /// <summary>
        /// Adds the specified new node at the start of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The new <see cref="LinkedListNode{T}"/> to add at the start of this <see cref="ArrayBuilder{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node belongs to another <see cref="ILinkedList{T}"/>.</exception>
        public virtual void AddFirst(LinkedListNode<T> node)
        {
            InnerList.AddFirst(node);

            OnUpdate();
        }

        /// <summary>
        /// Adds a new node containing the specified value at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="value">The value to add at the end of this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        public virtual LinkedListNode<T> AddLast(T value)
        {
            var result = InnerList.AddLast(value);

            OnUpdate();

            return result;
        }

        /// <summary>
        /// Adds the specified new node at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The new <see cref="LinkedListNode{T}"/> to add at the end of this <see cref="ArrayBuilder{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node belongs to another <see cref="ArrayBuilder{T}"/>.</exception>
        public virtual void AddLast(LinkedListNode<T> node)
        {
            InnerList.AddLast(node);

            OnUpdate();
        }

        #region AddRange methods

        /// <summary>
        /// Add multiple values at the top of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="values">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeFirst(params T[] values) => InnerList.First == null ? AddRangeLast(values) : AddRangeBefore(InnerList.First, values);

        /// <summary>
        /// Add multiple values at the top of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeFirst(IEnumerable<T> array) => InnerList.First == null ? AddRangeLast(array) : AddRangeBefore(InnerList.First, array);

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the top of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeFirst(params LinkedListNode<T>[] nodes) { if (InnerList.First == null) AddRangeLast(nodes); else AddRangeBefore(InnerList.First, nodes); }

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the top of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeFirst(IEnumerable<LinkedListNode<T>> array) { if (InnerList.First == null) AddRangeLast(array); else AddRangeBefore(InnerList.First, array); }

        /// <summary>
        /// Add multiple values at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="values">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeLast(params T[] values)
        {

            var result = new LinkedListNode<T>[values.Length];

            for (int i = 0; i < values.Length; i++)

                result[i] = AddLast(values[i]);

            return result;

        }

        /// <summary>
        /// Add multiple values at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeLast(IEnumerable<T> array)

        {

            if (array is T[] _array) return AddRangeLast(_array);

            LinkedListNode<T>[] result;

            int i = 0;

            if (array is IList<T> __array)

            {

                result = new LinkedListNode<T>[__array.Count];

                for (; i < __array.Count; i++)

                    result[i] = AddLast(__array[i]);

                return result;

            }

            var values = new System.Collections.Generic. LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                _ = values.AddLast(AddLast(item));

            result = new LinkedListNode<T>[values.Count];

            foreach (LinkedListNode<T> item in values)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual void AddRangeLast(params LinkedListNode<T>[] nodes) => AddRangeLast((IEnumerable<LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeLast(IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddLast(item);

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="values">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, params T[] values)
        {

            var result = new LinkedListNode<T>[values.Length];

            for (int i = 0; i < values.Length; i++)

                result[i] = AddBefore(node, values[i]);

            return result;

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, IEnumerable<T> array)

        {

            var values = new System.Collections.Generic. LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                _ = values.AddLast(AddBefore(node, item));

            var result = new LinkedListNode<T>[values.Count];

            int i = 0;

            foreach (LinkedListNode<T> item in values)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeBefore(LinkedListNode<T> node, params LinkedListNode<T>[] nodes) => AddRangeBefore(node, (IEnumerable<LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeBefore(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddBefore(node, item);

        }

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="values">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, params T[] values) => node.Next == null ? AddRangeLast(values) : AddRangeBefore(node.Next, values);

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, IEnumerable<T> array) => node.Next == null ? AddRangeLast(array) : AddRangeBefore(node.Next, array);

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="nodes">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeAfter(LinkedListNode<T> node, params LinkedListNode<T>[] nodes) { if (node.Next == null) AddRangeLast(nodes); else AddRangeBefore(node.Next, nodes); }

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayBuilder{T}"/></param>
        public virtual void AddRangeAfter(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array) { if (node.Next == null) AddRangeLast(array); else AddRangeBefore(node.Next, array); }

        #endregion

        /// <summary>
        /// Removes all nodes from this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        public virtual void Clear()
        {
            InnerList.Clear();

            OnUpdate();
        }

        /// <summary>
        /// Determines whether a value is in this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="value">The value to locate in this <see cref="ArrayBuilder{T}"/>. The value can be null for reference types.</param>
        /// <returns>true if value is found in this <see cref="ArrayBuilder{T}"/>; otherwise, false.</returns>
        public virtual bool Contains(T value) => InnerList.Contains(value);

        /// <summary>
        /// Copies the entire <see cref="ArrayBuilder{T}"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ArrayBuilder{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="ArrayBuilder{T}"/> is greater than the available space from index to the end of the destination array.</exception>
        public virtual void CopyTo(T[] array, int index) => InnerList.Contains(array, index);

        /// <summary>
        /// Finds the first node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The first <see cref="LinkedListNode{T}"/> that contains the specified value, if found; otherwise, null.</returns>
        public virtual LinkedListNode<T> Find(T value) => InnerList.Find(value);

        /// <summary>
        /// Finds the last node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns>The last <see cref="LinkedListNode{T}"/> that contains the specified value, if found; otherwise, null.</returns>
        public virtual LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        /// <summary>
        /// Returns an enumerator that iterates through this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.LinkedList{T}"/>.Enumerator for this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual Enumerator GetEnumerator()
        {
            var enumerator = new Enumerator(this, Enumerators.Count, _wrapperStruct.Value);

            Enumerators.Add(enumerator);

            return enumerator;
        }

        /// <summary>
        /// Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize this <see cref="ArrayBuilder{T}"/> instance.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> object that contains the information required to serialize this <see cref="ArrayBuilder{T}"/> instance.</param>
        /// <param name="context">A <see cref="StreamingContext"/> object that contains the source and destination of the serialized stream associated with this <see cref="ArrayBuilder{T}"/> instance.</param>
        /// <exception cref="ArgumentNullException">info is null.</exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        /// <summary>
        /// Implements the System.Runtime.Serialization.ISerializable interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="SerializationException">The <see cref="SerializationInfo"/> object associated with the current <see cref="ArrayBuilder{T}"/> instance is invalid.</exception>
        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        /// <summary>
        /// Removes the first occurrence of the specified value from this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="value">The value to remove from this <see cref="ArrayBuilder{T}"/>.</param>
        /// <returns><see langword="true"/> if the element containing value is successfully removed; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if value was not found in the original <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual bool Remove(T value)
        {
            var result = InnerList.Remove(value);

            OnUpdate();

            return result;
        }

        /// <summary>
        /// Removes the specified node from this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> to remove from this <see cref="ArrayBuilder{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="ArrayBuilder{T}"/>.</exception>
        public virtual void Remove(LinkedListNode<T> node)
        {
            InnerList.Remove(node);

            OnUpdate();
        }

        /// <summary>
        /// Removes the node at the start of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ArrayBuilder{T}"/> is empty.</exception>
        public virtual void RemoveFirst()
        {
            InnerList.RemoveFirst();

            OnUpdate();
        }

        /// <summary>
        /// Removes the node at the end of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ArrayBuilder{T}"/> is empty.</exception>
        public virtual void RemoveLast()
        {
            InnerList.RemoveLast();

            OnUpdate();
        }

        System.Collections.Generic.LinkedList<T>.Enumerator ILinkedList<T>.GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        /// <summary>
        /// Returns an array with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>An array with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual T[] ToArray()

        {

            var result = new T[InnerList.Count];

            int i = 0;

            foreach (T item in InnerList)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Returns an <see cref="ArrayList"/> with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>An <see cref="ArrayList"/> with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual ArrayList ToArrayList()
        {

            var result = new ArrayList(InnerList.Count);

            foreach (T item in InnerList)

                _ = result.Add(item);

            return result;

        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public virtual List<T> ToList()

        {

            var result = new List<T>(Count);

            foreach (T item in InnerList)

                result.Add(item);

            return result;

        }

        void ICollection<T>. Add(T item) => ((ICollection<T>)InnerList).Add(item) ;

        void ICollection. CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index) ;

        public struct Enumerator : IEnumerator<LinkedListNode<T>>

        {

            private ArrayBuilder<T> _arrayBuilder;

            private int _enumeratorIndex;

            private int _version;

            public LinkedListNode<T> Current { get; private set; }

            object IEnumerator.Current => Current;

            internal Enumerator(ArrayBuilder<T> arrayBuilder, int enumeratorIndex, int version)

            {

                _arrayBuilder = arrayBuilder;

                _enumeratorIndex = enumeratorIndex;

                _version = version ;

                Current = null;

            }

            public void Dispose()
            {

                Reset();

                _arrayBuilder.Enumerators.RemoveAt(_enumeratorIndex);

                _enumeratorIndex = -1;

                _arrayBuilder = null;

            }

            public bool MoveNext()
            {

                if (_arrayBuilder._wrapperStruct.Value != _version)

                    throw new InvalidOperationException("The collection has changed during enumeration.");

                if (Current is null)

                {

                    if (_arrayBuilder.First is null)

                        return false;

                    Current = _arrayBuilder.First;

                    return true;

                }

                else if (Current.Next is null)

                    return false;

                else

                {

                    Current = Current.Next;

                    return true;

                }

            }

            public void Reset() => Current = null;

        }
    }
}
