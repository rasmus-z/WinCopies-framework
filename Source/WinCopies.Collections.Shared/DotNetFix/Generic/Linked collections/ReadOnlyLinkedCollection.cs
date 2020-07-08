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
    public class ReadOnlyLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => true;

        public ReadOnlyLinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        public ReadOnlyLinkedCollection(in LinkedCollection<T> listCollection) : this(listCollection.InnerList) { }

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

        void ICollection<T>.Add(T item) => throw new InvalidOperationException("The collection is read-only.");

        void ICollection<T>.Clear() => throw new InvalidOperationException("The collection is read-only.");

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException("The collection is read-only.");
    }
}

#endif
