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
using System.Collections.Specialized;
using System.ComponentModel;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyObservableQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        protected ObservableQueueCollection<T> InnerQueueCollection { get; }

        public int Count => InnerQueueCollection.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueueCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueueCollection).SyncRoot;

        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ReadOnlyObservableQueueCollection(in ObservableQueueCollection<T> queueCollection)
        {
            InnerQueueCollection = queueCollection ?? throw GetArgumentNullException(nameof(queueCollection));

            InnerQueueCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

            InnerQueueCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

        public IEnumerator<T> GetEnumerator() => InnerQueueCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueueCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerQueueCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueueCollection).CopyTo(array, index);
    }
}

#endif
