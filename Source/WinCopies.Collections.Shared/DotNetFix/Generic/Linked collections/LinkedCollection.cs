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

#if !WinCopies2

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected internal System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => false;

        public LinkedCollection() : this(new System.Collections.Generic.LinkedList<T>()) { }

        public LinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

        void ICollection<T>.Add(T item) => AddItem(item);

        protected virtual void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddAfter(LinkedListNode{T}, T)"/>
        public void AddAfter(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

        protected virtual LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddAfter(in LinkedListNode{T}, in LinkedListNode{T})"/>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => AddItemAfter(node, value);

        protected virtual void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddBefore(in LinkedListNode{T}, in T)"/>
        public void AddBefore(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

        protected virtual LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddBefore(in LinkedListNode{T}, in LinkedListNode{T})"/>
        public LinkedListNode<T> AddBefore(in LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

        protected virtual void AddFirstItem(LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddFirst(in T)"/>
        public void AddFirst(in LinkedListNode<T> node) => AddFirstItem(node);

        protected virtual LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

        /// <summary>
        /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddFirst(in T)"/>
        public LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

        protected virtual void AddLastItem(LinkedListNode<T> node) => InnerList.AddLast(node);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddLast(in T)"/>
        public void AddLast(in LinkedListNode<T> node) => AddLastItem(node);

        protected virtual LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddLast(in LinkedListNode{T})"/>
        public LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        protected virtual void ClearItems() => InnerList.Clear();

        public void Clear() => ClearItems();

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

        public bool Remove(T item) => RemoveItem(item);

        protected virtual void RemoveItem(LinkedListNode<T> node) => InnerList.Remove(node);

        public void Remove(LinkedListNode<T> node) => RemoveItem(node);

        protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

        public void RemoveFirst() => RemoveFirstItem();

        protected virtual void RemoveLastItem() => InnerList.RemoveLast();

        public void RemoveLast() => RemoveLastItem();

        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
    }
}

#endif
