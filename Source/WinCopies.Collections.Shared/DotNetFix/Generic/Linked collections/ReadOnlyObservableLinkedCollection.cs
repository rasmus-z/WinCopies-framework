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
using System.ComponentModel;
using System.Runtime.Serialization;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ReadOnlyObservableLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>, ISerializable
    {
        protected ObservableLinkedCollection<T> InnerLinkedCollection { get; }

        public int Count => InnerLinkedCollection.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerLinkedCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerLinkedCollection).SyncRoot;

        public event PropertyChangedEventHandler PropertyChanged;

        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ReadOnlyObservableLinkedCollection(in ObservableLinkedCollection<T> linkedCollection)
        {
            InnerLinkedCollection = linkedCollection ?? throw GetArgumentNullException(nameof(linkedCollection));

            InnerLinkedCollection.CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

            InnerLinkedCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in LinkedListNode<T> addedBefore, in LinkedListNode<T> addedAfter, in LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

        public IEnumerator<T> GetEnumerator() => InnerLinkedCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerLinkedCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerLinkedCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerLinkedCollection).CopyTo(array, index);

        void ICollection<T>.Add(T item) => ((ICollection<T>)InnerLinkedCollection).Add(item);

        void ICollection<T>.Clear() => InnerLinkedCollection.Clear();

        public bool Contains(T item) => InnerLinkedCollection.Contains(item);

        bool ICollection<T>.Remove(T item) => InnerLinkedCollection.Remove(item);

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerLinkedCollection.GetObjectData(info, context);
    }
}

#endif
