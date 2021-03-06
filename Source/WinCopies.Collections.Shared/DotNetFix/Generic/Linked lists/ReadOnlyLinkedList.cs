﻿/* Copyright © Pierre Sprimont, 2020
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
using System.Diagnostics;
using System.Runtime.Serialization;

using WinCopies.Collections.Resources;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>
    {
        public bool IsReadOnly => true;

        protected ILinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => InnerList.SyncRoot;

        bool ICollection.IsSynchronized => InnerList.IsSynchronized;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        // todo: to remove

        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        void ICollection<T>.Add(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection); // todo: to ThrowHelper

        void ICollection<T>.Clear() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddLast(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void Remove(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveFirst() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveLast() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
    }
}

#endif
